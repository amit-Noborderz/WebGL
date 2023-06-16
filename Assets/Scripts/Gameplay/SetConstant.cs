using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.IO;
using System;
//using static ServerSIdeCharacterHandling;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
//using SimpleJSON;

public class SetConstant : MonoBehaviour
{
    public string defaultJson;
    public bool XanaEnvironment;
    public static bool isGuest;
    //public string token;
    //public string envName;
    //public string defaultJson;
    public GameObject mainPlayer34;
    public static string isLogin;

    private IEnumerator Start()
    {
        // XanaConstants.xanaConstants.isBuilderScene = true;
        Application.runInBackground = true;

        //PassTextParam(string text);
        //   WebglPluginjs.PassTextParam("SetConstants Start Function ----------------");

        // XanaConstants.xanaConstants.isBuilderScene = true;
        yield return new WaitForSeconds(.05f);
         GetToken();
    }

    public IEnumerator JavaScriptToken(string objects)
    {
        yield return new WaitForSeconds(.05f);
        try
        {
            //Debug.Log("Objects are==="+objects);
            //JObject jobect = JObject.Parse(objects);
            ////JavaScriptJson bean = JsonUtility.FromJson<JavaScriptJson>(objects);
            //Debug.Log("Objects are 1===" + jobect.GetValue("usertoken").ToString());
            //SetConstant.isLogin = jobect.GetValue("usertoken").ToString() ;
            //Debug.Log("Objects are 2===" + jobect.GetValue("env").ToString());
            //XanaConstants.xanaConstants.EnviornmentName = jobect.GetValue("env").ToString();
            //Debug.Log("Objects are 3===" + bool.Parse(jobect.GetValue("BuilderCheck").ToString()));
            //XanaConstants.xanaConstants.isBuilderScene = bool.Parse(jobect.GetValue("BuilderCheck").ToString());
            //Debug.Log("Objects are 4===" + int.Parse(jobect.GetValue("mapcode").ToString());
            //XanaConstants.xanaConstants.builderMapID = int.Parse(jobect.GetValue("mapcode").ToString());
            //Debug.Log("Objects are 5===" + bool.Parse(jobect.GetValue("IsGuest").ToString()));
            //XanaConstants.xanaConstants.isGuest = bool.Parse(jobect.GetValue("IsGuest").ToString());
            JObject objectnew = JObject.Parse(objects);

            Debug.Log("Objects are===" + objectnew);
            //     JavaScriptJson bean = LitJson.JsonMapper.ToObject<JavaScriptJson>(objects);
            Debug.Log("Objects are 1===" + objectnew.GetValue("UserToken"));
            isLogin = objectnew.GetValue("UserToken").ToString();
            Debug.Log("Objects are 2===" + objectnew.GetValue("EnvName"));
            XanaConstants.xanaConstants.EnviornmentName = objectnew.GetValue("EnvName").ToString();
            Debug.Log("Objects are 3===" + objectnew.GetValue("BuilderCheck"));
            XanaConstants.xanaConstants.isBuilderScene = (bool)objectnew.GetValue("BuilderCheck");
            Debug.Log("Objects are 4===" + objectnew.GetValue("IsBuilderMapCode"));
            XanaConstants.xanaConstants.builderMapID = (int)objectnew.GetValue("IsBuilderMapCode");
            Debug.Log("Objects are 5===" + (bool)objectnew.GetValue("IsGuest"));
            isGuest = (bool)objectnew.GetValue("IsGuest");

        }
        catch (Exception e)
        {

        }


        if (!string.IsNullOrEmpty(isLogin) && !string.IsNullOrEmpty(XanaConstants.xanaConstants.EnviornmentName))
        {
            SetParameter();
        }
        else
            StartCoroutine(LoginGuest(ConstantsGod.API_BASEURL + ConstantsGod.guestAPI, (isSucess) =>
            {
                //SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
                //SubCatString.FaceBlendsShapes = new float[46];
                //string jbody = JsonUtility.ToJson(SubCatString);

                //Debug.Log("Default user" + defaultJson);


                //Debug.Log("jbody local====" + defaultJson);
                //File.WriteAllText(Application.persistentDataPath + "/loginAsGuestClass.json", defaultJson);
                //StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));

                if (!File.Exists(Application.persistentDataPath + "/loginAsGuestClass.json"))
                {
                    File.WriteAllText(Application.persistentDataPath + "/loginAsGuestClass.json", defaultJson);
                    StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));

                }

                if (!XanaConstants.xanaConstants.isBuilderScene)
                {
                    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                }
                else
                {
                    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
                }

                //while (!asyncOperation.isDone)
                //{
                LoadingHandler.Instance.UpdateLoadingSlider(1 * .2f);
                LoadingHandler.Instance.UpdateLoadingStatusText("Loading World");

                //   Debug.LogError("start 2");
            }));

    }


    public void GetToken()
    {
        //  GL.Clear(true,true,Color.clear);

        isLogin = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6NjI0LCJpYXQiOjE2ODY2NTYyNDEsImV4cCI6MTY4NjgyOTA0MX0.IhwLYAmExcqFYkrtzZSyvAsKUi8XI0KyYc-jiw1LsqM";
        // isLogin = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6NjI0OSwiaWF0IjoxNjg1MjA4NjUyLCJleHAiOjE2ODUzODE0NTJ9.tnURhs4IHgJxjLzGwSmgdK6_DfIKaFAdgeeFXuNJ4ko";
        // SetConstant.isLogin = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6NjI0OSwiaWF0IjoxNjg0NDgyOTA3LCJleHAiOjE2ODQ2NTU3MDd9";
        //XanaConstants.xanaConstants.builderMapID = 1302;
        //XanaConstants.xanaConstants.isBuilderScene = true;


        XanaConstants.xanaConstants.EnviornmentName = "Xana Festival";


        // WebglPluginjs.PassTextParam("COME TO GET TOKEN FUNCTION----------------");



        //// WebglPluginjs.PassTextParam("UserToken KEY ----------------" + Jammer.PlayerPrefs.HasKey("UserToken"));
        //if (Jammer.PlayerPrefs.HasKey("UserToken"))
        //{
        //    SetConstant.isLogin = Jammer.PlayerPrefs.GetString("UserToken");
        //    //    WebglPluginjs.PassTextParam("SetConstants GetToken Function ----------------" + SetConstant.isLogin);
        //}
        ////  WebglPluginjs.PassTextParam("EnvName KEY ----------------" + Jammer.PlayerPrefs.HasKey("EnvName"));

        //if (Jammer.PlayerPrefs.HasKey("EnvName"))
        //{
        //    XanaConstants.xanaConstants.EnviornmentName = Jammer.PlayerPrefs.GetString("EnvName");
        //    //   WebglPluginjs.PassTextParam("SetConstants GetToken Function ----------------" + XanaConstants.xanaConstants.EnviornmentName);
        //}


        //bool BuilderValue = bool.Parse(Jammer.PlayerPrefs.GetString("BuilderCheck"));
        //if (BuilderValue)
        //{
        //    // XanaEnvironment = false;
        //    XanaConstants.xanaConstants.isBuilderScene = true;
        //}
        //else
        //{
        //    //  XanaEnvironment = true;
        //    XanaConstants.xanaConstants.isBuilderScene = false;
        //}

        //XanaConstants.xanaConstants.isGuest = bool.Parse(Jammer.PlayerPrefs.GetString("IsGuest"));
        //XanaConstants.xanaConstants.builderMapID = int.Parse(Jammer.PlayerPrefs.GetString("IsBuilderMapCode"));
        Debug.Log("here -0");
        if (!string.IsNullOrEmpty(isLogin) && !string.IsNullOrEmpty(XanaConstants.xanaConstants.EnviornmentName))
        {
            Debug.Log("here -1");
            SetParameter();
            Debug.Log("here -2");
        }
        else
        {
            Debug.Log("here -3");
            StartCoroutine(LoginGuest(ConstantsGod.API_BASEURL + ConstantsGod.guestAPI, (isSucess) =>
            {
                Debug.Log("here -4");
                SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
                SubCatString.FaceBlendsShapes = new float[46];
                //string jbody = JsonUtility.ToJson(SubCatString);
                if (!File.Exists(Application.persistentDataPath + "/loginAsGuestClass.json"))
                {
                    File.WriteAllText(Application.persistentDataPath + "/loginAsGuestClass.json", defaultJson);
                    StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));
                }
                //StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));

                if (!XanaConstants.xanaConstants.isBuilderScene)
                {
                    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                }
                else
                {
                    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
                }

                //while (!asyncOperation.isDone)
                //{
                LoadingHandler.Instance.UpdateLoadingSlider(1 * .2f);
                LoadingHandler.Instance.UpdateLoadingStatusText("Loading World");

                //   Debug.LogError("start 2");
            }));
        }
            

        //SetConstant.isLogin = token;
        //XanaConstants.xanaConstants.EnviornmentName = envName;
        //SetParameter();
    }


    IEnumerator LoginGuest(string url, Action<bool> CallBack)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, "POST"))
        {
            var operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                yield return null;
            }
            ClassWithToken myObject1 = new ClassWithToken();
            myObject1 = ClassWithToken.CreateFromJSON(www.downloadHandler.text);
            if (!www.isHttpError && !www.isNetworkError)
            {
                if (www.error == null)
                {
                    if (myObject1.success)
                    {
                        // SetConstant.isLogin = myObject1.data.token;
                        //XanaConstants.xanaConstants.EnviornmentName = "Beach";
                        CallBack(false);
                    }
                }
            }
            www.Dispose();
        }
    }
    public void SetParameter()
    {
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

        //LoadingHandler.Instance.UpdateLoadingSlider(1 * .2f);
        //LoadingHandler.Instance.UpdateLoadingStatusText("Loading World");
        try
        {
            StartCoroutine(GetUserData(isLogin, (isSucess) =>
            {
                if (isSucess)
                {
                    Debug.LogError("first----------");

                    if (!XanaConstants.xanaConstants.isBuilderScene)
                    {
                        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                    }
                    else
                    {
                        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
                    }
                    //while (!asyncOperation.isDone)
                    //{
                    LoadingHandler.Instance.UpdateLoadingSlider(1 * .2f);
                    LoadingHandler.Instance.UpdateLoadingStatusText("Loading World");
                    //}
                }
                else
                {
                    Debug.LogError("second else----------" + XanaConstants.xanaConstants.isBuilderScene);
                    // SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
                    isGuest = true;

                    if (!File.Exists(Application.persistentDataPath + "/loginAsGuestClass.json"))
                    {
                        File.WriteAllText(Application.persistentDataPath + "/loginAsGuestClass.json", defaultJson);
                        StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));
                    }
                    // SubCatString.FaceBlendsShapes = new float[46];
                    //string jbody= JsonUtility.ToJson(SubCatString);

                    // Debug.Log("Default user" + defaultJson);


                    // Debug.Log("jbody local====" + defaultJson);
                    //File.WriteAllText(Application.persistentDataPath + "/loginAsGuestClass.json", defaultJson);
                    //StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));

                    // Debug.Log("call hua builder 20====" + XanaConstants.xanaConstants.isBuilderScene);
                    if (!XanaConstants.xanaConstants.isBuilderScene)
                    {
                        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                    }
                    else
                    {
                        Debug.Log("call hua builder 21====" + XanaConstants.xanaConstants.isBuilderScene);
                        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
                    }
                    //while (!asyncOperation.isDone)
                    //{
                    LoadingHandler.Instance.UpdateLoadingSlider(1 * .2f);
                    LoadingHandler.Instance.UpdateLoadingStatusText("Loading World");
                }
            }));
        }
        catch (Exception e)
        {
            SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
            SubCatString.FaceBlendsShapes = new float[46];
            //string jbody = JsonUtility.ToJson(SubCatString);

             string jbody = JsonUtility.ToJson(SubCatString);

            File.WriteAllText(Application.persistentDataPath + "/logIn.json", jbody);
            // StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));
            mainPlayer34.GetComponent<AvatarController>().IntializeAvatar();
            //  yield return new WaitForSeconds(0.1f);

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            //while (!asyncOperation.isDone)
            //{
            LoadingHandler.Instance.UpdateLoadingSlider(1 * .2f);
            LoadingHandler.Instance.UpdateLoadingStatusText("Loading World");
            //}
        }
    }


    IEnumerator GetUserData(string token, Action<bool> action)   // check if  data Exist
    {

        UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.OCCUPIDEASSETS + "1/1");
        www.SetRequestHeader("Authorization", token);
        www.SendWebRequest();
        while (!www.isDone)
        {
            yield return null;
        }
        Debug.Log(www.downloadHandler.text);
        string str = www.downloadHandler.text;
        //     WebglPluginjs.PassTextParam("Get User Data User Name ----------------"+ str);
        Debug.Log("Get User Data User Name ----------------" + str);

        Root getdata1 = new Root();
        getdata1 = JsonUtility.FromJson<Root>(str);
        // DefaultEnteriesforManican.instance.DefaultReset();
        //  print(getdata.success);
        if (!www.isHttpError && !www.isNetworkError)
        {
            Debug.Log("debug0...............");

            if (getdata1.success)
            {
                Debug.Log("debug1...............");
                // its a new user so create file 
                if (getdata1.data.count == 0)
                {


                    //StartCoroutine(CreateUserData(() =>
                    //{

                    //    Debug.Log("debug3...............");
                    //    SetParameter();
                    //    return;
                    //}));
                    Debug.Log("debug4...............");
                    SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
                    SubCatString.FaceBlendsShapes = new float[46];
                    string jbody = JsonUtility.ToJson(SubCatString);
                    // string jbody = JsonUtility.ToJson(defaultJson);
                    File.WriteAllText(Application.persistentDataPath + "/logIn.json", jbody);
                    // File.WriteAllText(GameManager.Instance.GetStringFolderPath(), jbody);
                    mainPlayer34.GetComponent<AvatarController>().IntializeAvatar();
                    yield return new WaitForSeconds(0.1f);
                    print("!!GetUserData IF");
                    action(false);
                }
                else
                {
                    //StartCoroutine(CreateUserData(() => {

                    //    Debug.Log("debug3...............");
                    //    SetParameter();
                    //    return;
                    //}));
                    Debug.Log("debug5...............");

                    //string jsonbody = JsonUtility.ToJson(getdata.data.rows[0].json);
                    //Debug.Log("Data json==="+ jsonbody);
                    //File.WriteAllText(Application.persistentDataPath + "/logIn.json", getdata.data.rows[0].json.ToString());
                    //SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
                    //SubCatString.FaceBlendsShapes = new float[46];
                    //  string jbody = getdata.data.rows[0].json.ToString();
                    Debug.Log("Jbody login data-------" + getdata1.data.rows[0].json.ToString());
                    string jbody = JsonUtility.ToJson(getdata1.data.rows[0].json);

                 // string jbody = JSONObject.Ser (getdata.data.rows[0].json);
                    Debug.Log("Jbody login data"+jbody);

                    File.WriteAllText(Application.persistentDataPath + "/logIn.json", jbody);
                    // StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));
                    yield return new WaitForSeconds(2f);
                    mainPlayer34.GetComponent<AvatarController>().IntializeAvatar();
                    //}

                    //StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));
                    //GameManager.Instance.mainCharacter.GetComponent<AvatarController>().IntializeAvatar();
                    action(true);

                }

            }
            else
            {
                Debug.Log("this is else");
                action(false);
            }
        }
        else
        {
            Debug.LogError("NetWorkissue Debug...");
            action(false);
        }
        www.Dispose();

    }

    //public IEnumerator CreateUserData(Action callBack)   // send json data with user id but first check if user already exist or not   user ID
    //{
    //    Debug.Log("user data created....");
    //    string url = ConstantsGod.API_BASEURL + ConstantsGod.CREATEOCCUPIDEUSER;
    //    //  string urlwithId = url + UserIDfromServer; //Adding user ID
    //    //Get data from file
    //    // need to add check if file exists
    //    Json json = new Json();
    //    json = json.CreateFromJSON(defaultJson);
    //    Debug.Log("user data created....1");
    //    //StartCoroutine(AddingEnteries(UserIDfromServer));
    //    SendUpdateData senddata = new SendUpdateData();
    //    senddata.name = "Avatar";
    //    senddata.json = JsonUtility.ToJson(json);
    //    senddata.thumbnail = "";
    //    senddata.description = "None";
    //    string bodyJson = JsonUtility.ToJson(senddata);
    //    //  WebglPluginjs.PassTextParam("string bodyJson ----------------"+ bodyJson);

    //    //print(bodyJson);
    //    UnityWebRequest www = new UnityWebRequest(url, "Post");
    //    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJson);
    //    www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
    //    www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    //    www.SetRequestHeader("Authorization", isLogin);
    //    www.SetRequestHeader("Content-Type", "application/json");
    //    Debug.Log("user data created....2");
    //    yield return www.SendWebRequest();
    //    Debug.Log("user data created....3");
    //    //Debug.LogError(www.downloadHandler.text);
    //    string str = www.downloadHandler.text;

    //    callBack();
    //    Debug.Log("user data created.... sucess");
    //    //Root getdata = new Root();
    //    //getdata = JsonUtility.FromJson<Root>(str);
    //    ////  print(getdata.success);
    //    //if (!www.isHttpError && !www.isNetworkError)
    //    //{
    //    //    if (getdata.success)
    //    //    {
    //    //        print("DataUpdated");
    //    //        GetResponseupdatedata _getdata = new GetResponseupdatedata();
    //    //        _getdata = JsonUtility.FromJson<GetResponseupdatedata>(www.downloadHandler.text);
    //    //        print(_getdata.msg);
    //    //        print("Data Sent : " + _getdata.success);
    //    //        print(JsonUtility.ToJson(_getdata.data.ToString()));
    //    //    }
    //    //}
    //    //callBack();
    //    //if (loadAllAvatar != null)
    //    //    loadAllAvatar?.Invoke(1, 1);
    //    ////LoadPlayerAvatar.instance_loadplayer.LoadPlayerAvatar_onAvatarSaved(1, 1);
    //}
    [System.Serializable]
    public class ClassWithToken
    {
        public bool success;
        public JustToken data;
        public string msg;
        public static ClassWithToken CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ClassWithToken>(jsonString);
        }
    }

    [System.Serializable]
    public class JustToken
    {
        public string token;
        public string encryptedId;
        public string xanaliaToken;
        //public UserData user;
        public bool isAdmin;
        public static JustToken CreateFromJSON(string jsonString)
        {
            print("Person " + jsonString);
            return JsonUtility.FromJson<JustToken>(jsonString);
        }
    }



    [Serializable]
    public class Row
    {
        public int id;
        public string name;
        public string thumbnail;
        public Json json;
        public string description;
        public bool isDeleted;
        public int createdBy;
        public DateTime createdAt;
        public DateTime updatedAt;
        public User user;
    }
    [Serializable]
    public class Root
    {
        public bool success;
        public Data data;
        public string msg;
    }

    [Serializable]
    public class Data
    {
        public int count;
        public List<Row> rows;
    }

    [Serializable]
    public class Json
    {
        public string id;
        public string name;
        public string thumbnail;
        public List<Item> myItemObj;

        public List<BoneDataContainer> SavedBones;
        public int SkinId;
        public Color Skin;
        public Color LipColor;
        public float SssIntensity;
        public Color SkinGerdientColor;
        public bool isSkinColorChanged;
        public bool isLipColorChanged;
        public int BodyFat;
        public int FaceValue;
        public int NoseValue;
        public int EyeValue;
        public int EyesColorValue;
        public int EyeBrowValue;
        public int EyeLashesValue;
        public int MakeupValue;
        public int LipsValue;
        public int LipsColorValue;
        public bool faceMorphed;
        public bool eyeBrowMorphed;
        public bool eyeMorphed;
        public bool noseMorphed;
        public bool lipMorphed;
        public string PresetValue;
        public string eyeTextureName;
        public string eyeLashesName;
        public string makeupName;
        public float[] FaceBlendsShapes;
        public Json CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Json>(jsonString);
        }
    }

    [Serializable]
    public class User
    {
        public int id;
        public string name;
        public string email;
        public string avatar;
    }
    [Serializable]
    public class JavaScriptJson
    {
        public string usertoken { get; set; }
        public string env { get; set; }
        public int mapcode { get; set; }
        public bool BuilderCheck { get; set; }
        public bool IsGuest { get; set; }
    }
}
