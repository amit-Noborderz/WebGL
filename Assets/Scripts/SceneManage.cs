using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun;
using Metaverse;
using System.Collections;
using System;
using System.IO;
using UnityEngine.Networking;

public class SceneManage : MonoBehaviourPunCallbacks
{ 
    public static bool callRemove;
    public GameObject AnimHighlight;
    public GameObject popupPenal;
    public GameObject spawnCharacterObject;
    public GameObject spawnCharacterObjectRemote;
    public GameObject EventEndedPanel;

    public string mainScene;

    private AsyncOperation asyncLoading;

    bool exitOnce = true;


    private void OnEnable()
    {
       
        if (SceneManager.GetActiveScene().name == "Main")
        {
            AvatarManager.sendDataValue = false;
        }
        if (LoadFromFile.instance)
        {
            LoadFromFile.instance._uiReferences = this;
        }
    }

    private void OnDisable()
    {
       // AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
       // Caching.ClearCache();
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
      //  Caching.ClearCache();
    }



    public void OpenARScene()
    {
        SceneManager.LoadScene("ARHeadWebCamTextureExample");
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
            PlayerPrefs.SetInt("RequestSend", 1);
        }
    }

    public void LoadMain()
    {
        if (exitOnce)
        {
            exitOnce = false;

            Screen.orientation = ScreenOrientation.LandscapeLeft;

            //if (GameManager.currentLanguage == "ja")
            //{
            //    LoadingHandler.Instance.UpdateLoadingStatusText("ホームに戻っています");
            //}
            //else if (GameManager.currentLanguage == "en")
            //{
            LoadingHandler.Instance.UpdateLoadingStatusText("Going Back to Home");
            //}
            Debug.Log("~~~~~~ LoadMain call");

            LoadingHandler.Instance.ShowLoading();
            AssetBundle.UnloadAllAssetBundles(false);
            Resources.UnloadUnusedAssets();
         //   Caching.ClearCache();
            StartCoroutine(LoadMainEnumerator());
        }

    }

    

    IEnumerator LoadMainEnumerator()
    {
        
        LeaveRoom();
        yield return new WaitForSeconds(.5f);
        if (XanaConstants.xanaConstants.museumAssetLoaded != null)
            XanaConstants.xanaConstants.museumAssetLoaded.Unload(true);
    }


    public void LoadWorld()
    {
        //webgl comment - 08-06-2023

        //if (PlayerPrefs.GetInt("IsLoggedIn") == 0)
        //{
        //    UIManager.Instance.LoginRegisterScreen.transform.SetAsLastSibling();
        //    UIManager.Instance.LoginRegisterScreen.SetActive(true);
        //}
        //else
        //{
        //    UIManager.Instance.IsWorldClicked();
        //}

    }
    public void LeaveRoom()
    {
        callRemove = true;
        Launcher.instance.working = ScenesList.MainMenu;
        PhotonNetwork.LeaveRoom(false);
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.DestroyAll(true);
        StartSceneLoading();
    }

    public void StartSceneLoading()
    {
        print("Hello Scene Manager");
        // string unit = "Going Back to Home";
        //string a= TextLocalization.GetLocaliseTextByKey();
        //LoadingHandler.Instance.UpdateLoadingStatusText("Going Back to Home");
        //asyncLoading = SceneManager.LoadSceneAsync(mainScene);
        //InvokeRepeating("AsyncProgress", 0.1f, 0.1f);
        StartCoroutine(LoadMianScene());
    }

    /// <summary>
    /// To load main scene 
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadMianScene() {
        yield return new WaitForSeconds(.2f);
        LoadingHandler.Instance.UpdateLoadingSlider(0.3f);
        yield return new WaitForSeconds(.4f);
        LoadingHandler.Instance.UpdateLoadingSlider(0.6f);
        print("loading mainmenu");
      
        Resources.UnloadUnusedAssets();
        if (!APIBaseUrlChange.instance.IsXanaLive)
        {
            if (XanaConstants.xanaConstants.isGuest)
            {
                Application.ExternalEval("window.location.href='" + "https://event-test.xana.net" + "'");
            }
            else
            {
                Application.ExternalEval("window.location.href='" + "https://event-test.xana.net" + "'");
            }
          
            // Application.Quit();
            //UnityWebRequest.ClearCookieCache();
            //Application.ExternalEval("window.location.href='" + "https://xanaapp-web.xana.net/home" + "'");
            //Application.Quit();
            //Process.Start("https://xanaapp-web.xana.net/home");
            // Application.("https://xanaapp-web.xana.net/home");
            //Application.OpenURL("https://be-dev.xana.net/");
            //  Debug.Log("testnetttttttttttttttttttttttttttttttttttttttttttttttttt");
        }
        else
        {
            // Application.Quit();
            if (XanaConstants.xanaConstants.isGuest)
            {
                Application.ExternalEval("window.location.href='" + "https://xana.net" + "'");
            }
            else
            {
                Application.ExternalEval("window.location.href='" + "https://xana.net" + "'");
            }
            //Application.Quit();
            //Process.Start("https://xanaapp-web.xana.net/home");
            ////   Application.OpenURL("https://web.xana.net/home");
            // Debug.Log("mainnettttttttttttttttttttttttttttttttttttttttttttttttt");

        }
        //  Caching.ClearCache();
        // GC.Collect();
        //SceneManager.LoadScene(mainScene);
    }
    void AsyncProgress()
    {
        LoadingHandler.Instance.UpdateLoadingSlider(asyncLoading.progress * 1.1f);
    }

    //public void Dispose()
    //{
    //    // Dispose of unmanaged resources.
    //    Dispose(true);
    //    // Suppress finalization.
    //    GC.SuppressFinalize(this);
    //}
    //protected virtual void Dispose(bool disposing)
    //{
    //    if (!_disposedValue)
    //    {
    //        if (disposing)
    //        {
    //            // TODO: dispose managed state (managed objects)
    //        }

    //        // TODO: free unmanaged resources (unmanaged objects) and override finalizer
    //        // TODO: set large fields to null
    //        _disposedValue = true;
    //    }
    //}
}
