using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using Cinemachine;
using UnityEditor;
using WebSocketSharp;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Photon.Realtime;
using UnityEngine.ResourceManagement.ResourceProviders;
using System;
using UnityEngine.UI;
using System.IO;

public class LoadFromFile : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    [Header("singleton object")]
    public static LoadFromFile instance;

    public GameObject mainPlayer;
    public GameObject mainController;
    private GameObject YoutubeStreamPlayer;

    public CinemachineFreeLook PlayerCamera;
    public CinemachineFreeLook playerCameraCharacterRender;
    public Camera environmentCameraRender;
    public Camera firstPersonCamera;
    [HideInInspector]
    private Transform updatedSpawnpoint;
    private Transform spawnPoint;
    private GameObject currentEnvironment;
    bool isEnvLoaded = false;

    private float fallOffset = 10f;
    public bool setLightOnce = false;
    

    private GameObject player;

    System.DateTime eventUnivEndDateTime, eventlocalEndDateTime;

    [HideInInspector]
    public GameObject leftJoyStick;

    [HideInInspector]
    public float joyStickMovementRange;

    public LayerMask layerMask;

    public string addressableSceneName;

    [SerializeField] Button HomeBtn;

    public double eventRemainingTime;

    public SceneManage _uiReferences;

    private void Awake()
    {
        instance = this;
        //    LoadFile();
        setLightOnce = false;
    }


    private void OnDestroy()
    {
        Physics.autoSimulation = true;
        Resources.UnloadUnusedAssets();
        GC.SuppressFinalize(this);
        GC.Collect(0);

        //    Caching.ClearCache();

    }

    private void Start()
    {
        //if (XanaEventDetails.eventDetails.DataIsInitialized)
        //{
        //    StartEventTimer();
        //}
        Input.multiTouchEnabled = true;
        for (int i = 0; i < SelfieController.Instance.OnFeatures.Length; i++)
        {
            if (SelfieController.Instance.OnFeatures[i] != null)
            {
                if (SelfieController.Instance.OnFeatures[i].name == "LeftJoyStick")
                {
                    leftJoyStick = SelfieController.Instance.OnFeatures[i];
                    break;
                }
            }
        }

        GameObject _updatedSpawnPoint = new GameObject();
        updatedSpawnpoint = _updatedSpawnPoint.transform;
    }

    //public void StartEventTimer()
    //{
    //    eventUnivEndDateTime = System.DateTime.Parse(XanaEventDetails.eventDetails.endTime);
    //    eventlocalEndDateTime = eventUnivEndDateTime.ToLocalTime();

    //    //eventRemainingTime = eventTimeInSeconds;
    //    InvokeRepeating("CalculateEventTime", 0, 1);
    //}

    public void CalculateEventTime()
    {
        int _eventEndSystemDateTimediff = (int)(eventlocalEndDateTime - System.DateTime.Now).TotalMinutes;

        print("===================DIFFEND : " + _eventEndSystemDateTimediff);

        if (_eventEndSystemDateTimediff <= 0)
        {
            print("Event Ended");
            _uiReferences.EventEndedPanel.SetActive(true);
            CancelInvoke("CalculateEventTime");
        }
    }

    public IEnumerator VoidCalculation()
    {
        while (true)
        {
            if (CheckVoid())
            {
                Debug.Log("Resetting Position");
                ResetPlayerPosition();
            }
            yield return new WaitForSeconds(1f);
        }
    }


    public void LoadFile()
    {
        mainPlayer.SetActive(false);
        Debug.Log("Env Name : " + XanaConstants.xanaConstants.EnviornmentName);
        //if (!setLightOnce)
        //{
        //    LoadLightSettings(XanaConstants.xanaConstants.EnviornmentName);
        //    setLightOnce = true;
        //}
        //LoadEnvironment(XanaConstants.xanaConstants.EnviornmentName);
        if (currentEnvironment == null)
        {
            if (XanaConstants.xanaConstants.isBuilderScene)
            {
                Debug.Log("true====");
                SetupEnvirnmentForBuidlerScene();
            }
            else
            {
                LoadEnvironment(XanaConstants.xanaConstants.EnviornmentName);
                CharacterLightCulling();
            }
        }
        else
        {
            StartCoroutine(SpawnPlayer());
        }

        PlayerCamera.gameObject.SetActive(true);
        environmentCameraRender.gameObject.SetActive(true);
        //environmentCameraRender.transform.GetChild(0).gameObject.SetActive(true);

        SelfieController.Instance.DisableSelfieFromStart();



    }

    void InstantiateYoutubePlayer()
    {
        if (YoutubeStreamPlayer == null)
        {
            Debug.Log("DJ Beach====" + XanaConstants.xanaConstants.EnviornmentName);
            if (XanaConstants.xanaConstants.EnviornmentName.Contains("DJ Event"))
            {
                YoutubeStreamPlayer = Instantiate(Resources.Load("DJEventData/YoutubeVideoPlayer") as GameObject);

                //#if UNITY_ANDROID || UNITY_EDITOR
                //                //YoutubeStreamPlayer.transform.localPosition = new Vector3(-0.44f, 0.82f, 14.7f);
                //                //YoutubeStreamPlayer.transform.localScale = new Vector3(0.46f, 0.43f, 0.375f);

                //#else
                //YoutubeStreamPlayer.transform.localPosition = new Vector3(-0.44f, 0.82f, 14.7f);
                //            YoutubeStreamPlayer.transform.localScale = new Vector3(0.46f, 0.43f, 0.375f);
                //#endif

                YoutubeStreamPlayer.transform.localPosition = new Vector3(0f, 0f, 10f);
                YoutubeStreamPlayer.transform.localScale = new Vector3(1f, 1f, 1f);
                YoutubeStreamPlayer.transform.GetChild(0).localScale = new Vector3(24f, 13f, 1f);
                YoutubeStreamPlayer.transform.GetChild(0).localPosition = new Vector3(0f, 12.01f, -25.2f);
                YoutubeStreamPlayer.transform.GetChild(0).gameObject.transform.localRotation = Quaternion.Euler(180f, 180f, 0f);

                YoutubeStreamPlayer.SetActive(false);
                if (YoutubeStreamPlayer)
                {
                    YoutubeStreamPlayer.SetActive(true);
                }
            }
            if (XanaConstants.xanaConstants.EnviornmentName.Contains("XANA Festival Stage") && !XanaConstants.xanaConstants.EnviornmentName.Contains("Dubai"))
            {
                YoutubeStreamPlayer = Instantiate(Resources.Load("XANAFestivalStageData/YoutubeVideoPlayer1") as GameObject);

                //#if UNITY_ANDROID || UNITY_EDITOR
                //                YoutubeStreamPlayer.transform.localPosition = new Vector3(-0.44f, 0.82f, 14.7f);
                //                YoutubeStreamPlayer.transform.localScale = new Vector3(0.46f, 0.43f, 0.375f);
                //#else
                //  YoutubeStreamPlayer.transform.localPosition = new Vector3(-0.44f, 0.82f, 14.7f);
                //            YoutubeStreamPlayer.transform.localScale = new Vector3(0.46f, 0.43f, 0.375f);
                //#endif


                YoutubeStreamPlayer.transform.localPosition = new Vector3(0f, 0f, 10f);
                YoutubeStreamPlayer.transform.localScale = new Vector3(1f, 1f, 1f);
                YoutubeStreamPlayer.transform.GetChild(0).localScale = new Vector3(-23.32f, -13f, -66.09f);
                YoutubeStreamPlayer.transform.GetChild(0).localPosition = new Vector3(0.1379f, 11.81f, -25.26f);

                YoutubeStreamPlayer.SetActive(false);
                if (YoutubeStreamPlayer)
                {
                    YoutubeStreamPlayer.SetActive(true);
                }
            }

            if (XanaConstants.xanaConstants.EnviornmentName.Contains("Xana Festival") || XanaConstants.xanaConstants.EnviornmentName.Contains("NFTDuel Tournament"))
            {
                YoutubeStreamPlayer = Instantiate(Resources.Load("MyBeach/XanaFestivalPlayer") as GameObject);

                //#if UNITY_ANDROID || UNITY_EDITOR
                //                //YoutubeStreamPlayer.transform.localPosition = new Vector3(-0.44f, 0.82f, 14.7f);
                //                //YoutubeStreamPlayer.transform.localScale = new Vector3(0.46f, 0.43f, 0.375f);

                //#else
                //YoutubeStreamPlayer.transform.localPosition = new Vector3(-0.44f, 0.82f, 14.7f);
                //            YoutubeStreamPlayer.transform.localScale = new Vector3(0.46f, 0.43f, 0.375f);
                //#endif

                YoutubeStreamPlayer.transform.localPosition = new Vector3(0f, 0f, 10f);
                YoutubeStreamPlayer.transform.localScale = new Vector3(1f, 1f, 1f);

                YoutubeStreamPlayer.SetActive(false);
                if (YoutubeStreamPlayer)
                {
                    YoutubeStreamPlayer.SetActive(true);
                }
            }
        }
    }
    void CharacterLightCulling()
    {
        if (!XanaConstants.xanaConstants.EnviornmentName.Contains("Xana Festival") || !XanaConstants.xanaConstants.EnviornmentName.Contains("NFTDuel Tournament") && !XanaConstants.xanaConstants.isBuilderScene)
        {
            //riken
            Light[] directionalLightList = FindObjectsOfType<Light>();
            for (int i = 0; i < directionalLightList.Length; i++)
            {
                if (directionalLightList[i].type == LightType.Directional && directionalLightList[i].gameObject.tag != "CharacterLight")
                {
                    directionalLightList[i].cullingMask = layerMask;
                }
            }
        }

        //.......
    }

    bool CheckVoid()
    {
        if (mainController.transform.position.y < (updatedSpawnpoint.transform.position.y-fallOffset))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainController.transform.position, mainController.transform.TransformDirection(Vector3.down), out hit, 1000))
            {
                updatedSpawnpoint.transform.localPosition = new Vector3(spawnPoint.transform.localPosition.x, hit.transform.localPosition.y, spawnPoint.transform.localPosition.z);
                return false;
            }
            else
            {
                updatedSpawnpoint.localPosition = spawnPoint.localPosition;
                return true;
            }
        }
        return false;
    }

    public IEnumerator SpawnPlayer()
    {
        LoadingHandler.Instance.UpdateLoadingSlider(.8f);
        LoadingHandler.Instance.UpdateLoadingStatusText("Joining World...");
        yield return new WaitForSeconds(.2f);
        if (!(SceneManager.GetActiveScene().name.Contains("Museum")))
        {
            if (XanaConstants.xanaConstants.EnviornmentName.Contains("AfterParty"))
            {
                if (XanaConstants.xanaConstants.setIdolVillaPosition)
                {
                    spawnPoint.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + 2, spawnPoint.position.z);
                    XanaConstants.xanaConstants.setIdolVillaPosition = false;
                }
                else
                {
                    for (int i = 0; i < IdolVillaRooms.instance.villaRooms.Length; i++)
                    {
                        if (IdolVillaRooms.instance.villaRooms[i].name == ChracterPosition.currSpwanPos)
                        {
                            spawnPoint.position = IdolVillaRooms.instance.villaRooms[i].gameObject.GetComponent<ChracterPosition>().spawnPos;
                            break;
                        }
                        else
                        {
                            spawnPoint.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + 2, spawnPoint.position.z);
                        }
                    }
                }
            }
            else
            {
                spawnPoint.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + 2, spawnPoint.position.z);
            }
            RaycastHit hit;
            CheckAgain:
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(spawnPoint.position, spawnPoint.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.tag == "PhotonLocalPlayer")
                {
                    spawnPoint.gameObject.transform.position = new Vector3(spawnPoint.position.x + UnityEngine.Random.Range(-1f, 1f), spawnPoint.position.y, spawnPoint.position.z + UnityEngine.Random.Range(-1f, 1f));
                    goto CheckAgain;
                } //else if()

                else if (hit.collider.gameObject.GetComponent<NPCRandomMovement>())
                {
                    spawnPoint.gameObject.transform.position = new Vector3(spawnPoint.position.x + UnityEngine.Random.Range(-2, 2), spawnPoint.position.y, spawnPoint.position.z + UnityEngine.Random.Range(-2, 2));
                    goto CheckAgain;
                }

                spawnPoint.gameObject.transform.position = new Vector3(spawnPoint.position.x, hit.point.y, spawnPoint.position.z);
            }
            if (XanaConstants.xanaConstants.EnviornmentName.Contains("XANALIA NFTART AWARD 2021"))
            {
                mainPlayer.transform.rotation = Quaternion.Euler(0f, 230f, 0f);
            }
            else if (XanaConstants.xanaConstants.EnviornmentName.Contains("DJ Event") || XanaConstants.xanaConstants.EnviornmentName.Contains("XANA Festival Stage"))
            {
                mainPlayer.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            if (XanaConstants.xanaConstants.EnviornmentName.Contains("Koto") || XanaConstants.xanaConstants.EnviornmentName.Contains("Tottori"))
            {
                mainPlayer.transform.rotation = Quaternion.Euler(0f, 180f, 0);
                Invoke(nameof(SetKotoAngle), 0.5f);
            }

        }
        mainPlayer.transform.position = new Vector3(0, 0, 0);
        mainController.transform.position = spawnPoint.position + new Vector3(0, 0.1f, 0);
        player = PhotonNetwork.Instantiate("34", spawnPoint.position, Quaternion.identity, 0);
        ReferrencesForDynamicMuseum.instance.m_34player = player;
        SetAxis();
        mainPlayer.SetActive(true);
        Metaverse.AvatarManager.Instance.InitCharacter();

        LoadingHandler.Instance.UpdateLoadingSlider(0.98f, true);

        //change youtube player instantiation code because while env is in loading and youtube started playing video
        InstantiateYoutubePlayer();

        SetAddressableSceneActive();
        LoadingHandler.Instance.HideLoading();
        LoadingHandler.Instance.UpdateLoadingSlider(0, true);
        LoadingHandler.Instance.UpdateLoadingStatusText("");
        updatedSpawnpoint.transform.localPosition = spawnPoint.localPosition;
        StartCoroutine(VoidCalculation());

        yield return new WaitForSeconds(.5f);
        LoadingHandler.Instance.HideLoading();
        //TurnOnPostCam();
        try
        {
            LoadingHandler.Instance.Loading_WhiteScreen.SetActive(false);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Exception here..............");
        }
    }



    public IEnumerator SpawnPlayerForBuilderScene()
    {
        LoadingHandler.Instance.UpdateLoadingStatusText("Joining World...");
        float n = UnityEngine.Random.Range(1f, 10f);
        if (PlayerPrefs.GetFloat("Count") != n)
        {
            PlayerPrefs.SetFloat("Count", n);
            spawnPoint.position = new Vector3(spawnPoint.position.x + n, spawnPoint.position.y + 2, spawnPoint.position.z);
        }
        else {
            spawnPoint.position = new Vector3(spawnPoint.position.x +.10f, spawnPoint.position.y + 2, spawnPoint.position.z);
        }

        RaycastHit hit;

        CheckAgain:
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(spawnPoint.position, spawnPoint.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.Log("Colliders====" + hit.collider.gameObject.tag);
            if (hit.collider.gameObject.tag == "PhotonLocalPlayer")
            {
                spawnPoint.gameObject.transform.position = new Vector3(spawnPoint.position.x + UnityEngine.Random.Range(-1f, 1f), spawnPoint.position.y, spawnPoint.position.z + UnityEngine.Random.Range(-1f, 1f));
                goto CheckAgain;
            } //else if()

            else if (hit.collider.gameObject.GetComponent<NPCRandomMovement>())
            {
                spawnPoint.gameObject.transform.position = new Vector3(spawnPoint.position.x + UnityEngine.Random.Range(-2, 2), spawnPoint.position.y, spawnPoint.position.z + UnityEngine.Random.Range(-2, 2));
                goto CheckAgain;
            }

            spawnPoint.gameObject.transform.position = new Vector3(spawnPoint.position.x, hit.point.y, spawnPoint.position.z);
        }

        mainPlayer.transform.position = new Vector3(0, 0, 0);
        mainController.transform.position = spawnPoint.position + new Vector3(0, 0.1f, 0);

        player = PhotonNetwork.Instantiate("34", spawnPoint.position, Quaternion.identity, 0);
        ReferrencesForDynamicMuseum.instance.m_34player = player;
        SetAxis();
        mainPlayer.SetActive(true);
        Metaverse.AvatarManager.Instance.InitCharacter();
        End:
        LoadingHandler.Instance.UpdateLoadingSlider(0.98f, true);
        yield return new WaitForSeconds(1);

        try
        {
            LoadingHandler.Instance.Loading_WhiteScreen.SetActive(false);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Exception here..............");
        }

        SetAddressableSceneActive();
        updatedSpawnpoint.localPosition = spawnPoint.localPosition;
        StartCoroutine(VoidCalculation());
        LightCullingScene();

        BuilderEventManager.AfterPlayerInstantiated?.Invoke();

        LoadingHandler.Instance.HideLoading();
        LoadingHandler.Instance.UpdateLoadingSlider(0, true);
        LoadingHandler.Instance.UpdateLoadingStatusText("");
    }

    void SetKotoAngle()
    {
        PlayerCamera.m_XAxis.Value = 0f;
        PlayerCamera.m_YAxis.Value = 0.75f;
    }
    public void SetAxis()
    {
        CinemachineFreeLook cam = PlayerCamera.GetComponent<CinemachineFreeLook>();
        if (cam)
        {
            if (XanaConstants.xanaConstants.EnviornmentName == "XANALIA NFTART AWARD 2021")
            {
                cam.Follow = mainController.transform;
                cam.m_XAxis.Value = 0;
                cam.m_YAxis.Value = 0.5f;
            }
            else
            {

                cam.Follow = mainController.transform;
                cam.m_XAxis.Value = 180;
                cam.m_YAxis.Value = 0.5f;
            }

            if (XanaConstants.xanaConstants.EnviornmentName == "DJ Event" || XanaConstants.xanaConstants.EnviornmentName == "Xana Festival" || XanaConstants.xanaConstants.EnviornmentName == "NFTDuel Tournament")
            {
                cam.Follow = mainController.transform;
                cam.m_XAxis.Value = 0;
                cam.m_YAxis.Value = 0.5f;
            }
            else
            {

                cam.Follow = mainController.transform;
                cam.m_XAxis.Value = 173;
                cam.m_YAxis.Value = 0.5f;
            }


        }

        CinemachineFreeLook cam2 = playerCameraCharacterRender.GetComponent<CinemachineFreeLook>();
        if (cam2)
        {

            if (XanaConstants.xanaConstants.EnviornmentName == "XANALIA NFTART AWARD 2021")
            {
                cam2.Follow = mainController.transform;
                cam2.m_XAxis.Value = 0;
                cam2.m_YAxis.Value = 0.5f;
            }

            else
            {

                cam2.Follow = mainController.transform;
                cam2.m_XAxis.Value = 180;
                cam2.m_YAxis.Value = 0.5f;
            }
            if (XanaConstants.xanaConstants.EnviornmentName == "DJ Event" || XanaConstants.xanaConstants.EnviornmentName == "Xana Festival" || XanaConstants.xanaConstants.EnviornmentName == "NFTDuel Tournament")
            {
                cam2.Follow = mainController.transform;
                cam2.m_XAxis.Value = 0;
                cam2.m_YAxis.Value = 0.5f;
            }
            else
            {

                cam2.Follow = mainController.transform;
                cam2.m_XAxis.Value = 173;
                cam2.m_YAxis.Value = 0.5f;
            }

        }
    }

    void ResetPlayerPosition()
    {
        Debug.Log("Reset Player Position");

        mainController.GetComponent<PlayerControllerNew>().gravityVector.y = 0;
        mainController.transform.localPosition = spawnPoint.localPosition;

        if (IdolVillaRooms.instance != null)
        {
            IdolVillaRooms.instance.ResetVilla();
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    public override void OnLeftRoom()
    {

    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("Instantiating Photon Complete");

        ResetPlayerPosition();
    }
    /*******************************************************************new code */

    string environmentLabel;
    public void LoadEnvironment(string label)
    {
        environmentLabel = label;
        StartCoroutine(DownloadAssets());
    }



    void SetupEnvirnmentForBuidlerScene()
    {
        LoadingHandler.Instance.UpdateLoadingStatusText("Getting World Ready....");
        if (BuilderData.spawnPoint.Count == 1)
        {
            spawnPoint = BuilderData.spawnPoint[0].spawnObject.transform;
        }
        else if (BuilderData.spawnPoint.Count > 1)
        {
            foreach (SpawnPointData g in BuilderData.spawnPoint)
            {
                if (g.IsActive)
                {
                    spawnPoint = g.spawnObject.transform;
                    break;
                }
            }
        }
        if (spawnPoint == null)
        {
            GameObject newobject = new GameObject("SpawningPoint");
            newobject.transform.position = new Vector3(0, 2500, 0);
            RaycastHit hit;
            if (Physics.Raycast(newobject.transform.position, newobject.transform.TransformDirection(Vector3.down), out hit, 3000))
            {
                newobject.transform.position = new Vector3(0, hit.point.y, 0);
            }
            else
            {
                newobject.transform.position = new Vector3(0, 100, 0);
            }
            spawnPoint = newobject.transform;
        }
        if (spawnPoint)
        {
            StartCoroutine(SpawnPlayerForBuilderScene());
        }

        BuilderEventManager.ApplySkyoxSettings?.Invoke();

    }





    IEnumerator DownloadAssets()
    {
        if (!isEnvLoaded)
        {
            if (environmentLabel.Contains(" : "))
            {
                string name = environmentLabel.Replace(" : ", string.Empty);
                environmentLabel = name;
            }
            //yield return StartCoroutine(DownloadEnvoirnmentDependanceies(environmentLabel));
            Debug.Log("Scene name to be loaded :- " + environmentLabel);
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(environmentLabel, LoadSceneMode.Additive, false);
            LoadingHandler.Instance.UpdateLoadingStatusText("Loading World...");
            LoadingHandler.Instance.UpdateLoadingSlider(.6f, true);
            yield return handle;
            Debug.Log("bundles load status :- "+handle.Status);
            addressableSceneName = environmentLabel;
            //...

            //One way to handle manual scene activation.
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                yield return handle.Result.ActivateAsync();
                DownloadCompleted();
            }
            else // error occur 
            {
                AssetBundle.UnloadAllAssetBundles(false);
                Resources.UnloadUnusedAssets();
                
                HomeBtn.onClick.Invoke();
            }
        }
        else
        {
            AssetBundle.UnloadAllAssetBundles(false);
            Resources.UnloadUnusedAssets();
           
            RespawnPlayer();
        }
    }
    private void DownloadCompleted()
    {
        isEnvLoaded = true;
        StartCoroutine(spwanPlayerWithWait());
    }

    IEnumerator spwanPlayerWithWait()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        CheckAgain:
        if (GameObject.FindGameObjectWithTag("SpawnPoint"))
        {
            spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        }
        else
            goto CheckAgain;
        if (spawnPoint)
        {
            StartCoroutine(SpawnPlayer());
        }
        yield return null;
    }

    private void OnDisable()
    {
        Resources.UnloadUnusedAssets();
        GC.SuppressFinalize(this);
        GC.Collect(0);
        //  Caching.ClearCache();
    }


    void RespawnPlayer()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("AddressableScene"));
        StartCoroutine(spwanPlayerWithWait());
    }
    IEnumerator DownloadEnvoirnmentDependanceies(string key)
    {

        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
        LoadingHandler.Instance.UpdateLoadingSlider(.3f);

        yield return getDownloadSize;
        if (getDownloadSize.IsValid())
        {

        }
        //If the download size is greater than 0, download all the dependencies.
        if (getDownloadSize.Result > 0)
        {
            AsyncOperationHandle downloadDependencies = Addressables.DownloadDependenciesAsync(key);
            yield return downloadDependencies;
        }
        LoadingHandler.Instance.UpdateLoadingSlider(.4f);
        yield return new WaitForSeconds(.3f);
    }


    public void SetAddressableSceneActive()
    {
        Debug.Log("builder ==="+ XanaConstants.xanaConstants.isBuilderScene);
        string temp = addressableSceneName;
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("AddressableScene"));
        if (temp.Contains(" Astroboy x Tottori Metaverse Museum"))
        {
            temp = "Astroboy x Tottori Metaverse Museum";
        }
        print("~~~~~~ " + temp);
        if (!string.IsNullOrEmpty(temp))
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(temp));
        else if (XanaConstants.xanaConstants.isBuilderScene)
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
        else
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(XanaConstants.xanaConstants.EnviornmentName));

    }

    void LightCullingScene()
    {
        // Forcfully resetting lights because on 
        if (XanaConstants.xanaConstants.EnviornmentName == "Xana Festival" || XanaConstants.xanaConstants.EnviornmentName == "NFTDuel Tournament" )
        {
            Light[] sceneLight;
            sceneLight = GameObject.FindObjectsOfType<Light>();
            for (int i = 0; i < sceneLight.Length; i++)
            {
                if (sceneLight[i].name.Contains("Character"))
                {
                    // sceneLight[i].cullingMask = LayerMask.GetMask("Nothing");
                    print("if character before " + sceneLight[i].cullingMask);
                    sceneLight[i].cullingMask = LayerMask.GetMask("NoPostProcessing");
                    print("if character After " + sceneLight[i].cullingMask);
                }
                else if (sceneLight[i].name.Contains("Directional Light"))
                {
                    sceneLight[i].cullingMask = LayerMask.GetMask("Default", "TransparentFX", "RenderTexture", "Character", "Head", "Body", "Plane", "Room", "AvaterSelection", "MiniMap", "ZoomUI", "Arrow", "CameraColliderIgnore", "PostProcessing", "PictureInteractable", "Particles", "NFTDisplayPanel", "NoRenderOnFPS", "Hair_Light");
                }

            }
        }
        else
        {
            CharacterLightCulling();
        }
    }


}

