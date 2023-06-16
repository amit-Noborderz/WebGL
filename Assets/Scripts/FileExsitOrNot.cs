using Photon.Pun;
using Sign_Up_Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FileExsitOrNot : MonoBehaviour
{
    public GameObject StartPanel;
    private bool firstTimeCall;
    public TMPro.TMP_InputField Username;
    public GameObject errorTextName;
    public ErrorHandler errorHandler;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Value of preset==="+ PlayerPrefs.GetInt("first"));
        if (PlayerPrefs.GetInt("first")==0)
        {
          
            firstTimeCall = true;
            StartPanel.SetActive(true);
        }
        else
        {
            if (!File.Exists(Application.persistentDataPath + "/loginAsGuestClass.json"))
            {
                StartPanel.SetActive(true);
            }
            else
            {

                StartPanel.SetActive(false);
            }
        }
       
    }

    IEnumerator WaitUntilAnimationFinished(Animator MyAnim)
    {

        yield return new WaitForSeconds(3f);
        MyAnim.SetBool("playAnim", false);
    }
    public void EnterUserName()
    {
        if (string.IsNullOrEmpty(Username.text.ToString()))
        {
            errorTextName.GetComponent<Animator>().SetBool("playAnim", true);
            // if (Application.systemLanguage == SystemLanguage.Japanese  )
            // {
            //     errorTextName.GetComponent<Text>().text = "名前を入力してください";
            // }
            // else
            // {
            //     errorTextName.GetComponent<Text>().text = "Name Field should not be empty";
            //  }
            errorHandler.ShowErrorMessage("Name should not be empty", errorTextName.GetComponent<Text>());

            StartCoroutine(WaitUntilAnimationFinished(errorTextName.GetComponent<Animator>()));
            return;
        }
        else
        {

            if (XanaChatSystem.instance.UserName.Length > 12)
            {
                PhotonNetwork.NickName = XanaChatSystem.instance.UserName.Substring(0, 12) + "...";
            }
            else
            {
                PhotonNetwork.NickName = XanaChatSystem.instance.UserName;
            }
            ArrowManager.Instance.PhotonUserName.text = Username.text.ToString();
            StartPanel.SetActive(false);
        }

        //    print(PlayerPrefs.GetInt("shownWelcome"));
        //  print(PlayerPrefs.GetInt("iSignup"));
        //   print(PlayerPrefs.GetInt("IsProcessComplete"));
        //   string Localusername = UsernameTextNew.Text;
        //string Localusername = Username.text;

        //if (Localusername == "")// || Localusername.Contains(" "))
        //{
        //    //  print("Username Field should not be empty");
        //    errorTextName.GetComponent<Animator>().SetBool("playAnim", true);
        //    // if (Application.systemLanguage == SystemLanguage.Japanese  )
        //    // {
        //    //     errorTextName.GetComponent<Text>().text = "名前を入力してください";
        //    // }
        //    // else
        //    // {
        //    //     errorTextName.GetComponent<Text>().text = "Name Field should not be empty";
        //    //  }
        //    errorHandler.ShowErrorMessage(ErrorType.Name_Field__empty.ToString(), errorTextName.GetComponent<Text>());

        //    StartCoroutine(WaitUntilAnimationFinished(errorTextName.GetComponent<Animator>()));
        //    return;
        //}
        //else if (Localusername.StartsWith(" "))
        //{
        //    errorTextName.GetComponent<Animator>().SetBool("playAnim", true);
        //    // if (Application.systemLanguage == SystemLanguage.Japanese  )
        //    // {
        //    //     errorTextName.GetComponent<Text>().text = "名前を入力してください";
        //    // }
        //    // else
        //    // {
        //    //     errorTextName.GetComponent<Text>().text = "Name Field should not be empty";
        //    //  }
        //    errorHandler.ShowErrorMessage(ErrorType.UserName_Has_Space.ToString(), errorTextName.GetComponent<Text>());

        //    StartCoroutine(WaitUntilAnimationFinished(errorTextName.GetComponent<Animator>()));
        //    return;
        //}

        //if (Localusername.EndsWith(" "))
        //{
        //    Localusername = Localusername.TrimEnd(' ');
        //}

        //if (isSetXanaliyaUserName)//rik
        //{
        //    MyClassOfPostingName tempMyObject = new MyClassOfPostingName();
        //    string bodyJsonOfName1 = JsonUtility.ToJson(tempMyObject.GetNamedata(Localusername));
        //    StartCoroutine(HitNameAPIWithXanaliyaUser(ConstantsGod.API_BASEURL + ConstantsGod.NameAPIURL, bodyJsonOfName1, Localusername));
        //    return;
        //}

        //// if(PlayerPrefs.GetInt("IsProcessComplete")==0)
        //if (PlayerPrefs.GetInt("shownWelcome") == 0 && PlayerPrefs.GetInt("IsProcessComplete") == 0 && PlayerPrefs.GetInt("iSignup") == 0)
        //{
        //    print("--- Return using namepanel" + Localusername);

        //    DynamicEventManager.deepLink?.Invoke("come from Guest Registration");
        //    //PlayerPrefs.SetString("GuestName", Localusername);//rik cmt add guste username key
        //    PlayerPrefs.SetString(ConstantsGod.GUSTEUSERNAME, Localusername);
        //    usernamePanal.SetActive(false);
        //    checkbool_preser_start = true;

        //    //  StoreManager.instance.OnSaveBtnClicked();
        //    PlayerPrefs.SetInt("shownWelcome", 1);
        //    if (PlayerPrefs.GetInt("shownWelcome") == 1)
        //    {
        //        StoreManager.instance.OnSaveBtnClicked();
        //    }
        //    PlayerPrefs.SetInt("IsProcessComplete", 1);// user is registered as guest/register.
        //    return;
        //}
        ////   print(PlayerPrefs.GetInt("shownWelcome"));
        ////  print(PlayerPrefs.GetInt("iSignup"));
        ////  print(PlayerPrefs.GetInt("IsProcessComplete"));
        //PlayerPrefs.SetInt("IsProcessComplete", 1);  // 
        ////   print("Test passed");
        //print("calling after user registration");



        //MyClassOfPostingName myObject = new MyClassOfPostingName();
        //string bodyJsonOfName = JsonUtility.ToJson(myObject.GetNamedata(Localusername));
        ////  print(bodyJson);
        //// StartCoroutine(HitNameAPIWithNewTechnique(NameAPIURL, bodyJsonOfName, Localusername));
        //Debug.Log("IsLoggedIn:" + PlayerPrefs.GetInt("IsLoggedIn"));
        //if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        //{
        //    Debug.LogError("User Already loged in set name api call.......");
        //    StartCoroutine(HitNameAPIWithNewTechnique(ConstantsGod.API_BASEURL + ConstantsGod.NameAPIURL, bodyJsonOfName, Localusername));
        //}
        //else
        //{

        //}
    }
}
