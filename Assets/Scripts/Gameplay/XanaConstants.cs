﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class XanaConstants : MonoBehaviour
{
    public static XanaConstants xanaConstants;
    public int mic;
    public int minimap;
    public int userName;
    public string CurrentSceneName;
    public string EnviornmentName;
    public string userLimit;
    public AssetBundle museumAssetLoaded;
   // public string museumDownloadLink;// = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Aurora_Art_Museum/auroramuseum.android";
    public GameObject buttonClicked;
    public GameObject _lastClickedBtn;
    public GameObject _curretClickedBtn;
    public bool IsMuseum = false;
    public string hair = "";
    public int faceIndex = 0;
    public bool isFaceMorphed = false;
    public int eyeBrowIndex = 0;
    public int eyeLashesIndex = 0;
    public int makeupIndex = 0;
    public bool isEyebrowMorphed = false;
    public int eyeIndex = 0;
    public string eyeColor = "";
    public bool isEyeMorphed = false;
    public int noseIndex = 0;
    public bool isNoseMorphed = false;
    public int lipIndex = 0;
    public string lipColor = "";
    public bool isLipMorphed = false;
    public int bodyNumber = -1;
    public string skinColor = "";
    public string shirt = "";
    public string shoes = "";
    public string pants = "";
    public string eyeWearable = "";
    public int currentButtonIndex = -1;
    public string PresetValueString;
    public GameObject[] avatarStoreSelection;
    public GameObject[] wearableStoreSelection;
    public GameObject[] colorSelection;
    public bool setIdolVillaPosition = true;
    public GameObject lastSelectedButton;
    
    public bool orientationchanged = false;
    public bool SelfiMovement = true;
    public GameObject ConnectionPopUpPanel;
    public int presetItemsApplied = 0;
    public bool isSkinApplied = false;
    //for Create Room Scene Avatar
    [Header("SNS Variables")]
    public bool r_isSNSComingSoonActive = true;
    public GameObject r_MainSceneAvatar;
   // public string loginToken;
    public enum ScreenType { MobileScreen, TabScreen }
    public ScreenType screenType;
    /// <summary>
    /// variables for builder scene integration 
    /// </summary>
    /// 
    public bool isBuilderScene;
    public bool isGuest;
    public int builderMapID;
    public string r_EmoteStoragePersistentPath
    {
        get
        {
            return Application.persistentDataPath + "/EmoteAnimationBundle";
        }
    }

    public string[] labels = { "boyc11hair", "staffshirt", "girlc24pants", "shoesc43" }; // labels of each group item for preload
    public UnityEvent<float> ProgressEvent;
    public UnityEvent<bool> CompletionEvent;
    private AsyncOperationHandle downloadHandle;

    
    public string r_EmoteReactionPersistentPath
    {
        get
        {
            return Application.persistentDataPath + "/EmoteReaction";
        }
    }


    public void Awake()
    {
        if (xanaConstants)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            xanaConstants = this;
            if (PlayerPrefs.HasKey("micSound"))
            {
                mic = PlayerPrefs.GetInt("micSound");
            }
            else
            {
                PlayerPrefs.SetInt("micSound", 0);
                mic = PlayerPrefs.GetInt("micSound");
            }
            if (!XanaConstants.xanaConstants.isBuilderScene)
            {
                if (PlayerPrefs.HasKey("minimap"))
                {
                    minimap = PlayerPrefs.GetInt("minimap");
                }
                else
                {
                    PlayerPrefs.SetInt("minimap", 0);
                    minimap = PlayerPrefs.GetInt("minimap");
                }
            }
            else
            {
                PlayerPrefs.SetInt("minimap", 0);
                minimap = PlayerPrefs.GetInt("minimap");
            }


            //if (PlayerPrefs.HasKey("userName"))
            //{
            //    userName = PlayerPrefs.GetInt("userName");
            //}
            //else
            //{
            //    PlayerPrefs.SetInt("userName", 1);
            //    userName = PlayerPrefs.GetInt("userName");
            //}



            DontDestroyOnLoad(this.gameObject);
        }

        avatarStoreSelection = new GameObject[11];
        wearableStoreSelection = new GameObject[8];
        colorSelection = new GameObject[2];
#if !UNITY_EDITOR
        var aspectRatio = Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
        if (DeviceDiagonalSizeInInches() > 6.5f && aspectRatio < 2f)
        {
            screenType = ScreenType.TabScreen;
        }
        else
        {
            screenType = ScreenType.MobileScreen;
        }
#endif
    }
    public float DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
        Debug.Log("Getting device inches: " + diagonalInches);

        return diagonalInches;
    }
    private void Start()
    {
      //  StartCoroutine(LoadAddressableDependenceies());
    }

    public void StopMic()
    {
        PlayerPrefs.SetInt("micSound", 0);
        mic = PlayerPrefs.GetInt("micSound");
    }

    public void PlayMic()
    {
        PlayerPrefs.SetInt("micSound", 1);
        mic = PlayerPrefs.GetInt("micSound");
    }


    /// <summary>
    /// To preload addressable dependenceies
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadAddressableDependenceies()
    {
        //yield return Addressables.DownloadDependenciesAsync("boyc11hair", true);

        // Check the download size
        for (int i = 0; i < labels.Length; i++)
        {
            downloadHandle = Addressables.DownloadDependenciesAsync(labels[i], false);
            float progress = 0;

            while (downloadHandle.Status == AsyncOperationStatus.None)
            {
                float percentageComplete = downloadHandle.GetDownloadStatus().Percent;
                if (percentageComplete > progress * 1.1) // Report at most every 10% or so
                {
                    progress = percentageComplete; // More accurate %
                    ProgressEvent.Invoke(progress);
                }
                yield return null;
            }

            CompletionEvent.Invoke(downloadHandle.Status == AsyncOperationStatus.Succeeded);
            Addressables.Release(downloadHandle); //Release the operation handle
        }
    }
}