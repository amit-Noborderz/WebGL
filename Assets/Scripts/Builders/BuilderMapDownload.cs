using MD_Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class BuilderMapDownload : MonoBehaviour
{

    public const string prefabPrefix = "pf";
    private float progressPlusValue = 100;
    public Transform builderAssetsParent;
    public GameObject terrainPlane;
    public GameObject waterPlane;

    public SkyBoxesData skyBoxData;
    public Color skyBoxColor;
    public Light directionalLight;
    public Light characterLight;

    public Volume postProcessVol;
    public VolumeProfile DefaultProfile;

    #region PRIVATE_VAR
    private ServerData serverData;
    private LevelData levelData;
    #endregion
    public string response;
    #region UNITY_METHOD
    private void OnEnable()
    {
        BuilderEventManager.OnBuilderDataFetch += OnBuilderDataFetch;
        BuilderEventManager.ApplySkyoxSettings += SetSkyProperties;
        BuilderEventManager.AfterPlayerInstantiated += SetPlayerProperties;
    }

    private void OnDisable()
    {
        BuilderEventManager.OnBuilderDataFetch -= OnBuilderDataFetch;
        BuilderEventManager.ApplySkyoxSettings -= SetSkyProperties;
        BuilderEventManager.AfterPlayerInstantiated -= SetPlayerProperties;
        BuilderData.spawnPoint.Clear();
    }

    private void Start()
    {


      //  XanaConstants.xanaConstants.builderMapID = int.Parse(Jammer.PlayerPrefs.GetString("IsBuilderMapCode"));
        //Debug.Log("mapcode=======" + Jammer.PlayerPrefs.GetString("IsBuilderMapCode"));
       // XanaConstants.xanaConstants.builderMapID = 1935;
        BuilderEventManager.OnBuilderDataFetch?.Invoke(XanaConstants.xanaConstants.builderMapID, SetConstant.isLogin);

        //code to build a scene using json only locally.
        //serverData = JsonUtility.FromJson<ServerData>(System.IO.File.ReadAllText(Application.persistentDataPath + "/Builder.json"));
        //BuilderData.mapData = serverData;
        //PopulateLevel();
    }


    #endregion

    #region PRIVATE_METHODS
    void OnBuilderDataFetch(int id, string token)
    {
        Debug.Log("call hua builder 8====");
        StartCoroutine(FetchUserMapFromServer(id, token));
        Debug.Log("call hua builder 10====" + id+"====="+token);
    }

    //the api is set we just have to get the map
    IEnumerator FetchUserMapFromServer(int _mapId, string userToken)
    {
        string _url = ConstantsGod.API_BASEURL + ConstantsGod.BUILDERGETSINGLEWORLDBYID + _mapId;
        using (UnityWebRequest www = UnityWebRequest.Get(_url))
        {
            www.SetRequestHeader("Authorization", userToken);
            www.SendWebRequest();
            while (!www.isDone)
            {
                yield return null;
            }
            if ((www.result == UnityWebRequest.Result.ConnectionError) || (www.result == UnityWebRequest.Result.ProtocolError))
            {
                response = www.downloadHandler.text;
                Debug.Log("call hua builder 9===="+response);
            }
            else
            {
                response = www.downloadHandler.text;
                Debug.Log("");
                //System.IO.File.WriteAllText(Application.persistentDataPath +"/"+ "Builder.json", www.downloadHandler.text);
                serverData = JsonUtility.FromJson<ServerData>(www.downloadHandler.text);
                BuilderData.mapData = serverData;
                Debug.Log("call hua builder 7====");
                StartCoroutine(PopulateLevel());
            }
        }
    }

    IEnumerator DownloadLevelDataJson(string _url, Action<string> OnSuccess, Action<string> OnError)
    {
        Debug.Log("call hua builder 25===== ");
        string path = Application.persistentDataPath + "/_LevelData.nbz";
        Debug.Log("call hua builder 26===== ");
        // WebClient client = new WebClient();
        Debug.Log("call hua builder 27===== ");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_url))
        {
            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download file: " + webRequest.error);
            }
            else
            {
                // Save the downloaded file to disk
                File.WriteAllBytes(path, webRequest.downloadHandler.data);
                Debug.Log("File downloaded to: " + path);
            }
        }
        //  client.DownloadFile(_url, path);
        Debug.Log("call hua builder 28===== ");
        using (FileStream fs = new FileStream(path, FileMode.Open))
        using (ZipArchive zip = new ZipArchive(fs))
        {
            var entry = zip.Entries.First();
            using (StreamReader sr = new StreamReader(entry.Open()))
            {
                try
                {
                    Debug.Log("call hua builder 4====");
                    string compressData = sr.ReadToEnd();
                    string deCompressData = DecompressString(compressData);
                    OnSuccess.Invoke(deCompressData);
                    response = deCompressData;
                }
                catch (Exception e)
                {
                    Debug.Log("call hua builder 5====");
                }

            }
        }
        File.Delete(path);
        yield return null;
    }

    IEnumerator PopulateLevel()
    {
        Debug.Log("call hua builder 2====");
        if (string.IsNullOrEmpty(serverData.data.map_json_link))
        {
            levelData = serverData.data.json;
        }
        else
        {
            Debug.Log("call hua builder 6====");
            yield return StartCoroutine(DownloadLevelDataJson(serverData.data.map_json_link, (sucess) =>
            {
                levelData = GetDecompressJson(sucess);
            },
            (onfalse) =>
            {
                Debug.LogError("Failed to load json....");
            }));
        }


        StartCoroutine(DownloadAssetsData(() =>
        {
            LoadAddressableSceneAfterDownload();
            SetSkyProperties();
        }));

        if (!string.IsNullOrEmpty(levelData.terrainProperties.meshDeformationPath))
            StartCoroutine(LoadMeshDeformationFile(levelData.terrainProperties.meshDeformationPath, GetTerrainDeformation));
        if (!string.IsNullOrEmpty(levelData.terrainProperties.texturePath))
            SetTerrainTexture(levelData.terrainProperties.texturePath);
        if (!string.IsNullOrEmpty(levelData.terrainProperties.waterTexturePath))
            SetWaterTexture(levelData.terrainProperties.waterTexturePath);

        SetPlaneScaleAndPosition(levelData.terrainProperties.planeScale, levelData.terrainProperties.planePos);
    }


    public IEnumerator DownloadAssetsData(Action CallBack)
    {
        Debug.Log("call hua builder 1====");
        int count = levelData.otherItems.Count;
        progressPlusValue /= count;
        LoadingHandler.Instance.UpdateLoadingStatusText("Downloading Assets...");
        for (int i = 0; i < count; i++)
        {
            AsyncOperationHandle<GameObject> _async = Addressables.LoadAssetAsync<GameObject>(prefabPrefix + levelData.otherItems[i].ItemID + "_XANA");
            while (!_async.IsDone)
            {
                yield return null;
            }
            if (_async.Status == AsyncOperationStatus.Succeeded)
            {
                GetObject(_async, levelData.otherItems[i]);
            }

            LoadingHandler.Instance.UpdateLoadingSlider(i * (.7f / count) + .2f);
        }

        CallBack();
    }

    public string DecompressString(string compressedText)
    {
        byte[] gZipBuffer = Convert.FromBase64String(compressedText);
        using (var memoryStream = new MemoryStream())
        {
            int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
            memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
            var buffer = new byte[dataLength];
            memoryStream.Position = 0;
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                gZipStream.Read(buffer, 0, buffer.Length);
            }

            // response = Encoding.UTF8.GetString(buffer);
            return Encoding.UTF8.GetString(buffer);
        }
    }

    LevelData GetDecompressJson(string LevelDataJson)
    {
        LevelData levelData = new LevelData();
        levelData = JsonUtility.FromJson<LevelData>(LevelDataJson);
        return levelData;
    }

    public static IEnumerator LoadMeshDeformationFile(string path, Action<byte[]> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(path);
        www.SendWebRequest();
        while (!www.isDone)
        {
            yield return null;
        }


        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            callback?.Invoke(results);
        }
    }

    public void GetTerrainDeformation(byte[] meshDeformation)
    {
        var deformedMeshData = System.Text.Encoding.UTF8.GetString(meshDeformation);
        if (deformedMeshData.Length <= 10) return;
        terrainPlane.GetComponent<MeshFilter>().mesh.vertices = DeserializeVector3Array(deformedMeshData);
        terrainPlane.GetComponent<MD_MeshColliderRefresher>().MeshCollider_UpdateMeshCollider();
    }
    public static Vector3[] DeserializeVector3Array(string vertexData)
    {
        string[] vectors = vertexData.Split('|');
        Vector3[] result = new Vector3[vectors.Length];
        for (int i = 0; i < vectors.Length; i++)
        {
            string[] values = vectors[i].Split(' ');
            result[i] = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
        }
        return result;
    }



    void SetTerrainTexture(string textureUrl)
    {
        MeshRenderer meshRenderer = terrainPlane.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            Debug.Log(textureUrl);
            StartCoroutine(GetTexture(textureUrl, (Texture tex) =>
            {
                meshRenderer.material.SetTexture("_MainTex", tex);
            }));
        }
    }

    void SetWaterTexture(string textureUrl)
    {
        MeshRenderer meshRenderer = waterPlane.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            StartCoroutine(GetTexture(textureUrl, (Texture tex) =>
            {
                meshRenderer.material.SetTexture("_MainTex", tex);
            }));
        }
    }

    void SetPlaneScaleAndPosition(Vector3 scale, Vector3 pos)
    {
        terrainPlane.transform.localScale = scale;
        terrainPlane.transform.position = pos;
    }

    void SetSkyProperties()
    {
        //SkyProperties skyProperties = levelData.skyProperties;
        //Camera.main.clearFlags = CameraClearFlags.Skybox;
        //if (skyProperties.skyId != -1)
        //{
        //    SkyBoxItem skyBoxItem = skyBoxData.skyBoxes.Find(x => x.skyId == skyProperties.skyId);
        //    RenderSettings.skybox = skyBoxItem.skyMaterial;
        //    directionalLight.intensity = skyBoxItem.directionalLightData.lightIntensity;
        //    characterLight.intensity = skyBoxItem.directionalLightData.character_directionLightIntensity;
        //    directionalLight.shadowStrength = skyBoxItem.directionalLightData.directionLightShadowStrength;
        //    directionalLight.color = skyBoxItem.directionalLightData.directionLightColor;
        //    SetPostProcessProperties(skyBoxItem.ppVolumeProfile);

        //    if (skyBoxItem.directionalLightData.lensFlareData.falreData != null)
        //        SetLensFlareData(skyBoxItem.directionalLightData.lensFlareData.falreData, skyBoxItem.directionalLightData.lensFlareData.flareScale);
        //}
        //else
        //{
        //    Color topColor, middleColor, bottomColor;
        //    ColorUtility.TryParseHtmlString("#" + skyProperties.skyColorTop, out topColor);
        //    ColorUtility.TryParseHtmlString("#" + skyProperties.skyColorMiddle, out middleColor);
        //    ColorUtility.TryParseHtmlString("#" + skyProperties.skyColorBottom, out bottomColor);

        //    Material mat = new Material(Shader.Find("GradientSkybox/Linear/Three Color"));
        //    mat.name = "Gradient Skybox";
        //    mat.SetColor("_TopColor", topColor);
        //    mat.SetColor("_MiddleColor", middleColor);
        //    mat.SetColor("_BottomColor", bottomColor);
        //    SetPostProcessProperties(DefaultProfile);
        //    RenderSettings.skybox = mat;
        //    directionalLight.color = skyBoxColor;
        //    directionalLight.intensity = 1f;
        //    directionalLight.shadowStrength = .2f;
        //    characterLight.intensity = .15f;
        //}
        //DynamicGI.UpdateEnvironment();



        SkyProperties skyProperties = levelData.skyProperties;
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        if (skyProperties.skyId != -1)
        {
            SkyBoxItem skyBoxItem = skyBoxData.skyBoxes.Find(x => x.skyId == skyProperties.skyId);
            string skyboxMatKey = skyBoxItem.skyName.Replace(" ", "");
            AsyncOperationHandle<Material> loadSkyBox = Addressables.LoadAssetAsync<Material>(skyboxMatKey);
            loadSkyBox.Completed += LoadSkyBox_Completed;
            //RenderSettings.skybox = skyBoxItem.skyMaterial;
            directionalLight.intensity = skyBoxItem.directionalLightData.lightIntensity;
            characterLight.intensity = skyBoxItem.directionalLightData.character_directionLightIntensity;
            directionalLight.shadowStrength = skyBoxItem.directionalLightData.directionLightShadowStrength;
            directionalLight.color = skyBoxItem.directionalLightData.directionLightColor;
            SetPostProcessProperties(skyBoxItem.ppVolumeProfile);

            if (skyBoxItem.directionalLightData.lensFlareData.falreData != null)
                SetLensFlareData(skyBoxItem.directionalLightData.lensFlareData.falreData, skyBoxItem.directionalLightData.lensFlareData.flareScale);
        }
        else
        {
            Color topColor, middleColor, bottomColor;
            ColorUtility.TryParseHtmlString("#" + skyProperties.skyColorTop, out topColor);
            ColorUtility.TryParseHtmlString("#" + skyProperties.skyColorMiddle, out middleColor);
            ColorUtility.TryParseHtmlString("#" + skyProperties.skyColorBottom, out bottomColor);

            Material mat = new Material(Shader.Find("GradientSkybox/Linear/Three Color"));
            mat.name = "Gradient Skybox";
            mat.SetColor("_TopColor", topColor);
            mat.SetColor("_MiddleColor", middleColor);
            mat.SetColor("_BottomColor", bottomColor);
            RenderSettings.skybox = mat;
            directionalLight.color = skyBoxColor;
            directionalLight.intensity = 1f;
            directionalLight.shadowStrength = .2f;
            characterLight.intensity = .15f;
            DynamicGI.UpdateEnvironment();
        }
    }


    private void LoadSkyBox_Completed(AsyncOperationHandle<Material> obj)
    {
        RenderSettings.skybox = obj.Result;
        DynamicGI.UpdateEnvironment();
        throw new NotImplementedException();

    }


    void SetLensFlareData(LensFlareDataSRP lensFlareData, float lensFlareScale)
    {
        if (directionalLight.gameObject.GetComponent<LensFlareComponentSRP>() != null)
        {
            directionalLight.GetComponent<LensFlareComponentSRP>().lensFlareData = lensFlareData;
            directionalLight.GetComponent<LensFlareComponentSRP>().scale = lensFlareScale;
        }
        else
        {
            directionalLight.gameObject.AddComponent<LensFlareComponentSRP>().occlusionRadius = 0.35f;
            directionalLight.GetComponent<LensFlareComponentSRP>().lensFlareData = lensFlareData;
            directionalLight.GetComponent<LensFlareComponentSRP>().scale = lensFlareScale;
        }

    }

    void SetPlayerProperties()
    {
        BuilderEventManager.ApplyPlayerProperties?.Invoke(levelData.playerProperties.jumpMultiplier, levelData.playerProperties.speedMultiplier);
    }


    public void UpdateScene()
    {
        DynamicGI.UpdateEnvironment();
    }
    void SetPostProcessProperties(VolumeProfile _postProcessVol)
    {
        postProcessVol.profile = _postProcessVol;
    }


    private void GetObject(AsyncOperationHandle<GameObject> obj, ItemData _itemData)
    {
        switch (obj.Status)
        {
            case AsyncOperationStatus.Succeeded:
                GameObject tempObject = obj.Result;
                CreateENV(tempObject, _itemData);
                break;

            case AsyncOperationStatus.Failed:
                Debug.Log("Download error");
                break;

            default:
                break;
        }

    }

    private void CreateENV(GameObject objectTobeInstantiate, ItemData _itemData)
    {
        GameObject newObj = Instantiate(objectTobeInstantiate, _itemData.Position, _itemData.Rotation, builderAssetsParent);
        newObj.SetActive(true);
        XanaItem xanaItem = newObj.GetComponent<XanaItem>();
        xanaItem.SetData(_itemData);
        if (_itemData.ItemID.Contains("SPW") || _itemData.spawnComponent)
        {
            SpawnPointData spawnPointData = new SpawnPointData();
            spawnPointData.spawnObject = newObj;
            spawnPointData.IsActive = _itemData.spawnerComponentData.IsActive;
            BuilderData.spawnPoint.Add(spawnPointData);
        }

        //int count = levelData.otherItems.Count;
        //for (int i = 0; i < count; i++)
        //{
        //    GameObject newObj = Instantiate(gameObjects[i], levelData.otherItems[i].Position,levelData.otherItems[i].Rotation);
        //    XanaItem xanaItem = newObj.GetComponent<XanaItem>();
        //    xanaItem.SetData(levelData.otherItems[i]);
        //    if (xanaItem.itemBase.categoryId.Value.Equals("SPW"))
        //    {
        //        Debug.LogError("local pos :- "+ levelData.otherItems[i].Position);
        //        BuilderData.spawnPoint.Add(levelData.otherItems[i].Position);
        //    }
        //}
    }
    #endregion

    #region COROUTINE

    IEnumerator GetTexture(string url, Action<Texture> DownloadedTexture)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if ((request.result == UnityWebRequest.Result.ConnectionError) || (request.result == UnityWebRequest.Result.ProtocolError))
            Debug.Log(request.error);
        else
        {
            DownloadedTexture(((DownloadHandlerTexture)request.downloadHandler).texture);
        }
    }

    void LoadAddressableSceneAfterDownload()
    {
        Debug.Log("call hua builder====");
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        LoadingHandler.Instance.UpdateLoadingSlider(.8f);
        LoadingHandler.Instance.UpdateLoadingStatusText("Getting World Ready....");
    }

    #endregion

    [System.Serializable]
    public class DataTemp
    {
        public GameObject _object;
        public ItemData _itemData;
    }
}

[Serializable]
public class ServerData
{
    public bool success;
    public Data data = new Data();
    public string msg;
}

[Serializable]
public class Data
{
    public int id;
    public string name;
    public string thumbnail;
    public string terrainTexture;
    public LevelData json;
    public string status;
    public string description;
    public string map_json_link;
    public User user;
}

[Serializable]
public class UserMap
{
    public int id;
    public string name;
    public string thumbnail;
    public string terrainTexture;
    public LevelData json = new LevelData();
    public string status;
    public string description;
    public bool isDeleted;
    public int createdBy;
    public string createdAt;
    public string updatedAt;
    public User user = new User();
}

[Serializable]
public class User
{
    public int id;
    public string name;
    public string email;
    public string avatar;
    public string phoneNumber;
    public string coins;
}

[Serializable]
public class LevelData
{
    public string mapCode;
    public string mapName;
    public List<ItemData> groundItems;  //ground items
    public List<ItemData> otherItems;   //other items
    public SkyProperties skyProperties;
    public PlayerProperties playerProperties;
    public TerrainProperties terrainProperties;
}

//User Map Data Model - 
[Serializable]
public class UserMapData
{
    public string mapName;
    public string mapSpritePath;
    public string timestamp;
    public string mapCode;

    public UserMapData()
    {
        mapName = mapSpritePath = timestamp = mapCode = null;
    }
}

//User Maps
[Serializable]
public class UserMaps
{
    public List<UserMapData> userMapList;
}




[Serializable]
public class SkyProperties
{
    #region Data Variables
    //public string skyColor;
    public string skyColorTop;
    public string skyColorMiddle;
    public string skyColorBottom;
    public int skyId;
    #endregion

    public SkyProperties()
    {
        //this.skyColor = ColorUtility.ToHtmlStringRGBA(Color.white);  //init default color as white
        if (skyColorTop == null)
            this.skyColorTop = ColorUtility.ToHtmlStringRGBA(Color.blue);  //init default color as white
        else
        {
            Color color;
            ColorUtility.TryParseHtmlString("#" + "", out color);
            this.skyColorTop = ColorUtility.ToHtmlStringRGBA(color);
            Debug.Log("skyColorTop : " + skyColorTop);
        }
        if (skyColorMiddle == null)
        {
            Color color;
            this.skyColorMiddle = ColorUtility.ToHtmlStringRGBA(Color.blue + Color.white);
        }
        else
        {
            Color color;
            ColorUtility.TryParseHtmlString("#" + "", out color);
            this.skyColorMiddle = ColorUtility.ToHtmlStringRGBA(color);
            Debug.Log("skyColorMiddle : " + skyColorMiddle);
        }
        if (skyColorBottom == null)
            this.skyColorBottom = ColorUtility.ToHtmlStringRGBA(Color.white);  //init default color as white
        else
        {
            Color color;
            ColorUtility.TryParseHtmlString("#" + "", out color);
            this.skyColorBottom = ColorUtility.ToHtmlStringRGBA(color);
            Debug.Log("skyColorBottom : " + skyColorBottom);
        }
    }
}

[Serializable]
public class PlayerProperties
{
    #region Data Variables
    public float speedMultiplier;
    public float jumpMultiplier;
    #endregion

    public PlayerProperties()
    {
        this.speedMultiplier = 1f;
        this.jumpMultiplier = 1f;
    }
}

[Serializable]
public class TerrainProperties
{
    #region Data Variables
    public string materialName;
    public string texturePath;
    public string waterTexturePath;
    public string meshDeformationPath;
    public Vector3[] deformedPlane;
    public Vector3 planeScale;
    public Vector3 planePos;
    public int upMovesAllowed;
    public int downMovesAllowed;
    public int leftMovesAllowed;
    public int rightMovesAllowed;
    public int planeHeightLimit;
    #endregion

    public TerrainProperties()
    {
        materialName = "";
        texturePath = "";
        waterTexturePath = "";
        meshDeformationPath = "";
        planeScale = new Vector3(0f, 1f, 0f);
        planePos = new Vector3(0.0f, 0f, 0.0f);
        upMovesAllowed = downMovesAllowed = leftMovesAllowed = rightMovesAllowed = 2;
        planeHeightLimit = 5;
    }
}

[Serializable]
public class SpawnComponentData
{
    public bool IsActive;
}
