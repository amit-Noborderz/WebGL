﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class PremiumUsersDetails : MonoBehaviour
{
    public static PremiumUsersDetails Instance;
    public string getGroupDetailsAPI = ConstantsGod.GETSETS;
    //   public string getGroupDetailsAPITest = "https://api-test.xana.net" + ConstantsGod.GETSETS;
    public MainClass SetMainObj;
    public MainClass comingSoonObj;
    public Dictionary<string, bool> combinedUserFeatures = new Dictionary<string, bool>();
    public Dictionary<string, bool> comingSoonFeatures = new Dictionary<string, bool>();
    public GameObject PremiumUserUI;
    //public GameObject comingSoonPanel;
    public GameObject PremiumUserUIDJEvent;
    public GameObject vipPassUI;
    private string AuthToken;
    public string PremiumUserType;
    public bool testing;



    [Header("Top Notification Reference")]
    public RectTransform notificationPanelOnb;
    public TextMeshProUGUI messageText;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            SetMainObj = new MainClass();
            comingSoonObj = new MainClass();
            DontDestroyOnLoad(gameObject);
            PremiumUserType = "";
        }
        else
        if (Instance != this)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        /*if(SNSComingSoonManager.Instance.snsComingSoonScreen!=null)
        {
            comingSoonPanel = SNSComingSoonManager.Instance.snsComingSoonScreen;
        }*/
    }

    //public void OpenComingSoonPopUp()
    //{
    //    if (SNSNotificationManager.Instance != null)
    //        SNSNotificationManager.Instance.ShowNotificationMsg("This features is coming soon");
    //}
    public bool CheckSpecificItem(string getName, bool enablePopupHere = true)
    {

        if (testing)
            return true;

        getName = getName.Trim();
        getName = getName.ToLower();
        bool EnabledFeaturebool = false;
        if (combinedUserFeatures.ContainsKey(getName))
        {
            EnabledFeaturebool = combinedUserFeatures[getName];
            if (EnabledFeaturebool)
            {
                if (comingSoonFeatures.ContainsKey(getName))
                {
                    EnabledFeaturebool = comingSoonFeatures[getName];
                    if (EnabledFeaturebool)
                    {
                        //if (SNSNotificationManager.Instance != null && enablePopupHere)
                        //    SNSNotificationManager.Instance.ShowNotificationMsg("This features is coming soon");    //this method is used to show Coming Soon notification.......  
                        return false;
                    }
                }

                return true;
            }
            else if (comingSoonFeatures.ContainsKey(getName))
            {
                EnabledFeaturebool = comingSoonFeatures[getName];
                if (EnabledFeaturebool)
                {
                    if (enablePopupHere)
                        ShowNotificationMsg("This features is coming soon");    //this method is used to show Coming Soon notification.......  
                }
                else
                {
                    //not showing alpha pass pop to any user as it is removed or completed.
                    //if (SNSNotificationManager.Instance != null && enablePopupHere)
                    //    SNSNotificationManager.Instance.ShowNotificationMsg("This features is coming soon");    //this method is used to show Coming Soon notification.......  
                    //if (enablePopupHere)
                    //    ShowNotAvailablePanel(PremiumUserUI);
                }
                return false;

            }
            //else if (ConstantsGod.UserPriorityRole == "alpha-pass")
            //{
            //    if (SNSNotificationManager.Instance != null && enablePopupHere)
            //    {
            //        SNSNotificationManager.Instance.ShowNotificationMsg("This features is coming soon");//this method is used to show Coming Soon notification.......                                            
            //    }
            //    return false;
            //}
            //else
            //{
            //    if (enablePopupHere)
            //        ShowNotAvailablePanel(PremiumUserUI);
            //    return false;
            //}


        }
        else {

            Debug.LogError("String not exist");
        }
        return false;
    }



    void ShowNotAvailablePanel(GameObject panel)
    {
        panel.SetActive(true);
    }




    [System.Serializable]
    public class MyClassOfPostingName
    {
        public string name;
        public MyClassOfPostingName GetEmaildata(string _name)
        {
            MyClassOfPostingName myObj = new MyClassOfPostingName();
            myObj.name = _name;
            return myObj;
        }
    }

    public void GetGroupDetails(string _groupName = "")
    {
        PlayerPrefs.SetString("PremiumUserType", _groupName);
        PlayerPrefs.Save();
        switch (_groupName)
        {
            case "astroboy":
                {
                    print("Astroboy");
                    _groupName = "Astroboy";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "djevent":
                {
                    print("djevent");
                    _groupName = "DJ Event";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "Access Pass":
                {
                    print("Access Path");
                    _groupName = "Set3";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "Extra NFT":
                {
                    print("Extra NFT");
                    _groupName = "Set2";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "guest":
                {
                    print("Guest");
                    _groupName = "Set1";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "freeuser":
                {
                    print("Freeuser");
                    _groupName = "Set1";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "vip-pass":
                {
                    print("Freeuser");
                    _groupName = "vip-pass";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
           
        }
        PremiumUserType = PlayerPrefs.GetString("PremiumUserType");

        MyClassOfPostingName myobjectOfEmail = new MyClassOfPostingName();
        string bodyJson = JsonUtility.ToJson(myobjectOfEmail.GetEmaildata(_groupName));

        StartCoroutine(HitGetGroupDetails(ConstantsGod.API_BASEURL + ConstantsGod.GetGroupDetailsAPI, bodyJson, (SetMainObj) =>
        {
            this.SetMainObj = SetMainObj;
            for (int i = 0; i < SetMainObj.data.relationList.Count; i++)
            {
                SetMainObj.data.relationList[i].feature = SetMainObj.data.relationList[i].feature.Trim();
                SetMainObj.data.relationList[i].feature = SetMainObj.data.relationList[i].feature.ToLower();
                if (!combinedUserFeatures.ContainsKey(SetMainObj.data.relationList[i].feature))
                    combinedUserFeatures.Add(SetMainObj.data.relationList[i].feature, SetMainObj.data.relationList[i].isEnabled);
                else if (combinedUserFeatures[SetMainObj.data.relationList[i].feature] == false)
                    combinedUserFeatures[SetMainObj.data.relationList[i].feature] = SetMainObj.data.relationList[i].isEnabled;

            }
        }));
    }


    public void GetGroupDetailsForComingSoon(string _groupName = "ComingSoon")
    {
        switch (_groupName)
        {
            case "astroboy":
                {
                    _groupName = "Astroboy";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "djevent":
                {
                    print("djevent");
                    _groupName = "DJ Event";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "Access Pass":
                {
                    print("Access Path");
                    _groupName = "Set3";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "Extra NFT":
                {
                    print("Extra NFT");
                    _groupName = "Set2";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "guest":
                {
                    print("Guest");
                    _groupName = "Set1";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "freeuser":
                {
                    print("Freeuser");
                    _groupName = "Set1";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "vip-pass":
                {
                    print("Freeuser");
                    _groupName = "vip-pass";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
            case "ComingSoon":
                {
                    _groupName = "ComingSoon";
                    AuthToken = ConstantsGod.AUTH_TOKEN;
                    break;
                }
           
        }

        MyClassOfPostingName myobjectOfEmail = new MyClassOfPostingName();
        string bodyJson = JsonUtility.ToJson(myobjectOfEmail.GetEmaildata(_groupName));

        StartCoroutine(HitGetGroupDetails(ConstantsGod.API_BASEURL + ConstantsGod.GetGroupDetailsAPI, bodyJson, (comingSoonObj) =>
        {
            this.comingSoonObj = comingSoonObj;
            for (int i = 0; i < comingSoonObj.data.relationList.Count; i++)
            {
                comingSoonObj.data.relationList[i].feature = comingSoonObj.data.relationList[i].feature.Trim();
                comingSoonObj.data.relationList[i].feature = comingSoonObj.data.relationList[i].feature.ToLower();
                if (!comingSoonFeatures.ContainsKey(comingSoonObj.data.relationList[i].feature))
                    comingSoonFeatures.Add(comingSoonObj.data.relationList[i].feature, comingSoonObj.data.relationList[i].isEnabled);

            }
        }));
    }

    public IEnumerator HitGetGroupDetails(string url, string Jsondata, Action<MainClass> callback)
    {
        //    print(Jsondata);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", AuthToken);
        yield return request.SendWebRequest();
        // Debug.LogError(request.downloadHandler.text);
        MainClass mainClassObj = new MainClass();
        mainClassObj = JsonUtility.FromJson<MainClass>(request.downloadHandler.text);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                //if (myObject1.success == "true")
                if (mainClassObj.success)
                {
                    print("Success in getting Group Data ");
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                print("Error in getting Group Details");
            }
            else
            {
                if (request.error != null)
                {
                    if (!mainClassObj.success)
                    {
                        Debug.LogError("Hey success false in Getting Group data " + mainClassObj.msg);
                    }
                }
            }
        }

        callback(mainClassObj);
        request.Dispose();
    }


    public void ShowNotificationMsg(string msg)
    {
        notificationPanelOnb.gameObject.SetActive(true);
        messageText.text = TextLocalization.GetLocaliseTextByKey(msg);
        notificationPanelOnb.DOAnchorPosY(-40, 0.3f).SetEase(Ease.Linear);
        if (waitToCloseNotificationCo != null)
        {
            StopCoroutine(waitToCloseNotificationCo);
        }
        waitToCloseNotificationCo = StartCoroutine(NotificationPanelClose());
    }

    Coroutine waitToCloseNotificationCo;
    IEnumerator NotificationPanelClose()
    {
        yield return new WaitForSeconds(1.5f);
        notificationPanelOnb.DOAnchorPosY(250, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        notificationPanelOnb.gameObject.SetActive(false)
        );
    }




    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    [System.Serializable]
    public class RelationListClass
    {
        public bool isEnabled;
        public string feature;
    }

    [System.Serializable]
    public class DataClass
    {
        public string name;
        public List<RelationListClass> relationList;
    }

    [System.Serializable]
    public class MainClass
    {
        public bool success;
        public DataClass data;
        public string msg;
    }

}
