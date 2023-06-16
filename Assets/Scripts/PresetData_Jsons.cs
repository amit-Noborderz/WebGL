using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;
using Metaverse;

public class PresetData_Jsons : MonoBehaviour
{

    public string JsonDataPreset;
    private string PresetNameinServer = "Presets";
    // Start is called before the first frame update
    public static string clickname;
    public bool IsStartUp_Canvas;   // if preset panel is appeared on start thn allow it to change 
    bool isAddedInUndoRedo = false;
    bool presetAlreadySaved = false;
    //public static GameObject lastSelectedPreset=null;
    //public static string lastSelectedPresetName=null;
    [SerializeField] Texture eyeTex;
    AvatarController avatarController;
    CharcterBodyParts charcterBodyParts;
    private string OtherPlayerId;
    private string json;
    object[] _mydatatosend = new object[2];

    void Start()  
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ChangecharacterOnCLickFromserver);

        //if (!isAddedInUndoRedo && this.transform.GetChild(0).gameObject.activeInHierarchy)
        //{
        //    isAddedInUndoRedo = true;
        //    if (UndoRedo.undoRedo.back)
        //    {
        //        UndoRedo.undoRedo.back = false;
        //    }
        //    else
        //    {
        //        UndoRedo.undoRedo.undoRedoList.ActionWithParametersAdd(this.gameObject, -1, "ChangecharacterOnCLickFromserver", UndoRedo.ActionWithParameters.ActionType.ChangeItem , Color.white);
        //    }
        //}

        avatarController = GameManager.Instance.mainCharacter.GetComponent<AvatarController>();
        charcterBodyParts = CharcterBodyParts.instance;
    }
    public void callit()
    {
        clickname = "";
    }
    void ChangecharacterOnCLickFromserver()
    {

        //if (GameManager.Instance.isStoreAssetDownloading)
        //    return;

        //    if (this.gameObject.name == PlayerPrefs.GetString("PresetValue"))
        //    {
        //        StoreManager.instance.ActivateSaveButton(false);
        //    }
        //    try
        //    {
        //        if (lastSelectedPreset != null)
        //        {
        //            lastSelectedPreset.transform.GetChild(0).gameObject.SetActive(false);
        //            lastSelectedPreset = this.gameObject;
        //            lastSelectedPresetName = lastSelectedPreset.name;
        //            lastSelectedPreset.transform.GetChild(0).gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            lastSelectedPreset = this.gameObject;
        //lastSelectedPresetName = lastSelectedPreset.name;
        //            lastSelectedPreset.transform.GetChild(0).gameObject.SetActive(true);
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        lastSelectedPreset = null;
        //    }

        //if(StoreManager.instance)
        //  PlayerPrefs.SetString("PresetValue", PresetData_Jsons.lastSelectedPreset.name);
        //if (!IsStartUp_Canvas)   //for presets in avatar panel 
        // {
        //    if (clickname != gameObject.name)
        //        clickname = gameObject.name;
        //    else
        //        return;
        //}
        //charcterBodyParts.DefaultTexture(false);

        ////    print("Calling cloths");
        //if (!IsStartUp_Canvas && !PremiumUsersDetails.Instance.CheckSpecificItem(PresetNameinServer))
        //{
        //    //PremiumUsersDetails.Instance.PremiumUserUI.SetActive(true);
        //    print("Please Upgrade to Premium account");
        //    return;
        //}
        //else
        //{
        //    print("Horayyy you have Access");
        //    XanaConstants.xanaConstants.avatarStoreSelection[XanaConstants.xanaConstants.currentButtonIndex] = this.gameObject;
        //    XanaConstants.xanaConstants._curretClickedBtn = this.gameObject;

        //if (XanaConstants.xanaConstants._lastClickedBtn && XanaConstants.xanaConstants._curretClickedBtn == XanaConstants.xanaConstants._lastClickedBtn 
        //    && !IsStartUp_Canvas)
        //    return;

        //GameManager.Instance.isStoreAssetDownloading = true;
        //StoreManager.instance.UndoSelection();
        //if(!IsStartUp_Canvas)
        //    XanaConstants.xanaConstants._curretClickedBtn.transform.GetChild(0).gameObject.SetActive(true);
        //if (XanaConstants.xanaConstants._lastClickedBtn)
        //{
        //    if (XanaConstants.xanaConstants._lastClickedBtn.GetComponent<PresetData_Jsons>())
        //        XanaConstants.xanaConstants._lastClickedBtn.transform.GetChild(0).gameObject.SetActive(false);
        //}
        //XanaConstants.xanaConstants._lastClickedBtn = this.gameObject;
        //XanaConstants.xanaConstants.PresetValueString = gameObject.name;
        PlayerPrefs.SetInt("presetPanel", 1);

            // Hack for latest update // keep all preset body fat to 0
            //change lipsto default
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = JsonUtility.FromJson<SavingCharacterDataClass>(JsonDataPreset);  //(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            //print(_CharacterData.BodyFat);
            _CharacterData.BodyFat = 0;
            _CharacterData.PresetValue = gameObject.name;
        //AddressableDownloader.Instance.presetItemCount = _CharacterData.myItemObj.Count;

        //Lips Default
        //if (_CharacterData.myItemObj.Count >= 6)
        //{
        //    _CharacterData.myItemObj[5].ItemLinkAndroid = "";
        //    _CharacterData.myItemObj[5].ItemName = "";
        //    _CharacterData.myItemObj[5].ItemID = 0;

        //    _CharacterData.myItemObj[4].ItemLinkAndroid = "";
        //    _CharacterData.myItemObj[4].ItemLinkIOS = "";
        //    _CharacterData.myItemObj[4].ItemName = "";
        //    _CharacterData.myItemObj[4].ItemID = 0;

        //}

        //_CharacterData.SavedBones.Clear();
        //_CharacterData.FaceBlendsShapes = null;

        // Implementing Save Skin Color
        //if (_CharacterData.Skin != null && _CharacterData.SssIntensity != null)
        //{
        //    charcterBodyParts.ImplementSavedSkinColor(_CharacterData.Skin, _CharacterData.SssIntensity);
        //}
        //else
        //{
        //    charcterBodyParts.ImplementSavedSkinColor(_CharacterData.Skin);
        //}

        //if (_CharacterData.SkinGerdientColor != null)
        //{
        //    charcterBodyParts.ApplyGredientColor(_CharacterData.SkinGerdientColor, GameManager.Instance.mainCharacter);
        //}
        //else
        //{
        //    charcterBodyParts.ApplyGredientDefault(GameManager.Instance.mainCharacter);
        //}
        //   XanaConstants.xanaConstants.bodyNumber = 0;
        if (SetConstant.isGuest)
        {
            PlayerPrefs.SetInt("first", 1);
            File.WriteAllText((Application.persistentDataPath + "/loginAsGuestClass.json"), JsonUtility.ToJson(_CharacterData));
            StartCoroutine(ItemDatabase.instance.WaitAndDownloadFromRevert(0));
            Debug.Log("Json call first 123==="+ ReferrencesForDynamicMuseum.instance.m_34player.GetComponent<PhotonView>().IsMine);
            if (ReferrencesForDynamicMuseum.instance.m_34player.GetComponent<PhotonView>().IsMine)
            {
                _mydatatosend[0] = ReferrencesForDynamicMuseum.instance.m_34player.GetComponent<PhotonView>().ViewID as object;
                _mydatatosend[1] = GetJsonFolderData() as object;
                Invoke(nameof(CallRpcInvoke), 1.2f);
                //CallRpcInvoke();
            }
            //if (!this.GetComponent<PhotonView>().IsMine && !this.gameObject.GetComponent<Speaker>())
            //{
            //    this.gameObject.AddComponent<Speaker>();
            //}
        }
        //else
        //{
        //    File.WriteAllText((Application.persistentDataPath + "/login.json"), JsonUtility.ToJson(_CharacterData));
        //}


        //   File.WriteAllText((Application.persistentDataPath + "/SavingReoPreset.json"), JsonDataPreset);

        // DefaultEnteriesforManican.instance.DefaultReset();

        //WaqasAhmad
        //GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().DefaultTexture();
        //GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
        //SavaCharacterProperties.instance.LoadMorphsfromFile();
        //StoreManager.instance.UndoSelection();
        //Enable save button

        //    if (StoreManager.instance.StartPanel_PresetParentPanel.activeSelf)
        //    {

        //        if (PlayerPrefs.GetInt("iSignup") == 1)
        //        {
        //            //for register
        //            //   PlayerPrefs.SetInt("iSignup", 1);
        //            //UserRegisterationManager.instance.OpenUIPanal(8);
        //            //    ItemDatabase.instance.GetComponent<SavaCharacterProperties>().SavePlayerProperties();
        //            Invoke("abcd", 5f);

        //            //  PlayerPrefs.SetInt("presetPanel", 0);
        //            //  if (PlayerPrefs.GetInt("presetPanel") == 1)   // preset panel is enable so saving preset to account 
        //            //      PlayerPrefs.SetInt("presetPanel", 0);

        //            ///  ItemDatabase.instance.GetComponent<SavaCharacterProperties>().SavePlayerProperties();
        //            StoreManager.instance.StartPanel_PresetParentPanel.SetActive(false);

        //            //   

        //        }
        //        else                // as a guest
        //        {
        //            Invoke("abcd", 5f);

        //            //StoreManager.instance.StartPanel_PresetParentPanel.SetActive(false);
        //            //UserRegisterationManager.instance.UsernameFieldAdvance.Clear();
        //            //UserRegisterationManager.instance.usernamePanal.SetActive(true);
        //            //// enable check so that it will know that index is comming from start of the game
        //            //UserRegisterationManager.instance.checkbool_preser_start = false;
        //        }
        //    }
        //    else
        //    {
        //        if (this.gameObject.name != PlayerPrefs.GetString("PresetValue"))
        //        {
        //            //StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
        //            //StoreManager.instance.GreyRibbonImage.SetActive(false);
        //            //StoreManager.instance.WhiteRibbonImage.SetActive(true);
        //        }
        //    //    if (UndoRedo.undoRedo.back)
        //    //    {
        //    //        UndoRedo.undoRedo.back = false;
        //    //    }
        //    //    else
        //    //    {
        //    //        UndoRedo.undoRedo.undoRedoList.ActionWithParametersAdd(this.gameObject, -1, "ChangecharacterOnCLickFromserver", UndoRedo.ActionWithParameters.ActionType.ChangeItem, Color.white);
        //    //    }
        //    //    XanaConstants.xanaConstants._lastClickedBtn = this.gameObject;
        //    //}
        //    //if (avatarController.wornEyewearable != null)
        //    //{
        //    //    avatarController.UnStichItem("EyeWearable");
        //    //}

        //    //avatarController.ApplyPreset(_CharacterData);

        //    GetSavedPreset();
        //    //if (!presetAlreadySaved)
        //    //{
        //    //    StoreManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = true;
        //    //    SavedButtonClickedBlue();
        //    //}

        //    //else
        //    //{
        //    //    StoreManager.instance.SaveStoreBtn.SetActive(true);
        //    //    StoreManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = false;
        //    //    StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
        //    //    StoreManager.instance.GreyRibbonImage.SetActive(true);
        //    //    StoreManager.instance.WhiteRibbonImage.SetActive(false);
        //    //}
        //    //GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().Head.gameObject.GetComponent<SkinnedMeshRenderer>().materials[0].SetTexture("_BaseMap", eyeTex);
        //    //  PlayerPrefs.SetInt("IsLoggedIn", 0);
        //    // PlayerPrefs.SetInt("FristPresetSet", 1);
        //}
    }
    [PunRPC]
    void CheckRpc(object[] Datasend)
    {
        AvatarController otherPlayer;
        Debug.Log("Datasend ------------- " + Datasend.ToString());
        Debug.Log("Datasend1 ------------- " + Datasend[0].ToString());
        Debug.Log("Datasend2 ------------- " + Datasend[1].ToString());


        string SendingPlayerID = Datasend[0].ToString();
        OtherPlayerId = Datasend[0].ToString();
        SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
        _CharacterData = JsonUtility.FromJson<SavingCharacterDataClass>(Datasend[1].ToString());

        //for (int j = 0; j < Launcher.instance.playerobjects.Count; j++)
        //{
        if (ReferrencesForDynamicMuseum.instance.m_34player.GetComponent<PhotonView>().ViewID.ToString() == OtherPlayerId)
        {
            otherPlayer = GetComponent<AvatarController>();
            CharcterBodyParts bodyparts = otherPlayer.GetComponent<CharcterBodyParts>();

            if (_CharacterData.myItemObj.Count != 0)
            {
                for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
                {
                    //if (!GetComponent<PhotonView>().IsMine)
                    //{
                    // Update Body fate
                    if (_CharacterData.myItemObj[i].ItemName != "")
                    {
                        string type = _CharacterData.myItemObj[i].ItemType;
                        if (type.Contains("Legs") || type.Contains("Chest") || type.Contains("Feet") || type.Contains("Hair") || type.Contains("EyeWearable"))
                        {
                            WearAddreesable(_CharacterData.myItemObj[i].ItemType, _CharacterData.myItemObj[i].ItemName, otherPlayer.gameObject);
                        }
                    }
                    else
                    {
                        if (GetComponent<AvatarController>())
                        {
                            switch (_CharacterData.myItemObj[i].ItemType)
                            {
                                case "Legs":
                                    otherPlayer.WearDefaultItem("Legs", otherPlayer.gameObject);
                                    break;
                                case "Chest":
                                    otherPlayer.WearDefaultItem("Chest", otherPlayer.gameObject);
                                    break;
                                case "Feet":
                                    otherPlayer.WearDefaultItem("Feet", otherPlayer.gameObject);
                                    break;
                                case "Hair":
                                    otherPlayer.WearDefaultItem("Hair", otherPlayer.gameObject);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    //}
                }
            }
            else // if player is all default cloths
            {
                otherPlayer.WearDefaultItem("Legs", otherPlayer.gameObject);
                otherPlayer.WearDefaultItem("Chest", otherPlayer.gameObject);
                otherPlayer.WearDefaultItem("Feet", otherPlayer.gameObject);
                otherPlayer.WearDefaultItem("Hair", otherPlayer.gameObject);
            }
            if (_CharacterData.eyeTextureName != "" && _CharacterData.eyeTextureName != null)
            {
                StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeTextureName, otherPlayer.gameObject));
            }
            if (_CharacterData.eyebrrowTexture != "" && _CharacterData.eyebrrowTexture != null)
            {
                StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyebrrowTexture, otherPlayer.gameObject));
            }
            if (_CharacterData.makeupName != "" && _CharacterData.makeupName != null)
            {
                StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.makeupName, otherPlayer.gameObject));
            }
            if (_CharacterData.eyeLashesName != "" && _CharacterData.eyeLashesName != null)
            {
                StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeLashesName, otherPlayer.gameObject));
            }

            //if (_CharacterData.SkinGerdientColor != null && _CharacterData.SssIntensity != null)
            //{
            //    bodyparts.StartCoroutine(bodyparts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, _CharacterData.SkinGerdientColor, this.gameObject));
            //}
            //else
            //{
            //    bodyparts.StartCoroutine(bodyparts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, this.gameObject));
            //}

            //if (_CharacterData.Skin != null && _CharacterData.LipColor != null && _CharacterData.HairColor != null && _CharacterData.EyebrowColor != null && _CharacterData.EyeColor != null)
            //{
            //    //bodyparts.StartCoroutine(bodyparts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, otherPlayer.gameObject));
            //    bodyparts.StartCoroutine(bodyparts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, _CharacterData.HairColor, _CharacterData.EyebrowColor, _CharacterData.EyeColor, otherPlayer.gameObject));
            //}

            // Seperate 
            if (_CharacterData.Skin != null)
            {
                bodyparts.StartCoroutine(bodyparts.ImplementColors(_CharacterData.Skin, SliderType.Skin, this.gameObject));
            }
            if (_CharacterData.EyeColor != null)
            {
                bodyparts.StartCoroutine(bodyparts.ImplementColors(_CharacterData.EyeColor, SliderType.EyesColor, this.gameObject));
            }
            if (_CharacterData.LipColor != null)
            {
                bodyparts.StartCoroutine(bodyparts.ImplementColors(_CharacterData.LipColor, SliderType.LipsColor, this.gameObject));
            }
            if (_CharacterData.HairColor != null)
            {
                bodyparts.StartCoroutine(bodyparts.ImplementColors(_CharacterData.HairColor, SliderType.HairColor, this.gameObject));
            }
            if (_CharacterData.EyebrowColor != null)
            {
                bodyparts.StartCoroutine(bodyparts.ImplementColors(_CharacterData.EyebrowColor, SliderType.EyeBrowColor, this.gameObject));
            }

            if (_CharacterData.SkinGerdientColor != null)
            {
                bodyparts.ApplyGredientColor(_CharacterData.SkinGerdientColor, otherPlayer.gameObject);
            }
            else
            {
                bodyparts.ApplyGredientDefault(otherPlayer.gameObject);
            }
            bodyparts.SetSssIntensity(0, otherPlayer.gameObject);
            bodyparts.LoadBlendShapes(_CharacterData, bodyparts.gameObject);
            otherPlayer.LoadBonesData(_CharacterData, otherPlayer.gameObject);
            //StartCoroutine(otherPlayer.RPCMaskApply(otherPlayer.gameObject));
        }
        //}
    }
    public void WearAddreesable(string itemtype, string itemName, GameObject applyOn)
    {
        if (!itemName.Contains("md", StringComparison.CurrentCultureIgnoreCase))
        {
            StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(-1, itemName, itemtype, applyOn.GetComponent<AvatarController>()));
        }
        else
        {
            applyOn.GetComponent<AvatarController>().WearDefaultItem(itemtype, applyOn);
        }
    }

    void CallRpcInvoke()
    {
        ReferrencesForDynamicMuseum.instance.m_34player.GetComponent<PhotonView>().RPC(nameof(CheckRpc), RpcTarget.AllBuffered, _mydatatosend as object);

    }
    //void SavedButtonClickedBlue()
    //{
    //    StoreManager.instance.SaveStoreBtn.SetActive(true);
    //    StoreManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
    //    StoreManager.instance.GreyRibbonImage.SetActive(false);
    //    StoreManager.instance.WhiteRibbonImage.SetActive(true);
    //}

    public string GetJsonFolderData()
    {
        //if (PlayerPrefs.GetInt("IsLoggedIn") == 1)  // loged from account)
        //{
        //    return File.ReadAllText(Application.persistentDataPath + "/logIn.json");
        //}
        //else
        //{
        //    return File.ReadAllText(Application.persistentDataPath + "/loginAsGuestClass.json");
        //}
        string path = "";
        bool IsGuestBool = SetConstant.isGuest;
        // bool IsGuestBool = true;
        //   Debug.Log("Guest user cloth 2===" + Jammer.PlayerPrefs.GetString("IsGuest"));
        if (IsGuestBool)
        {
            Debug.Log("call Geste====");
            //clothDataString = 
            path = Application.persistentDataPath + "/loginAsGuestClass.json";
            //if (File.Exists(path))
            //{
            json = File.ReadAllText(path);
            // Process the JSON data here
            Debug.Log(json);

            return json;
            //return (Application.persistentDataPath + "/loginAsGuestClass.json");
        }
        else
        {
            path = Application.persistentDataPath + "/logIn.json";
            //if (File.Exists(path))
            //{
            json = File.ReadAllText(path);
            // Process the JSON data here
            Debug.Log(json);

            return json;
            //   return (Application.persistentDataPath + "/logIn.json");
        }
    }
    void GetSavedPreset()
    {
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)  // logged in from account
        {
            if (File.Exists(Application.persistentDataPath + "/logIn.json") && File.ReadAllText(Application.persistentDataPath + "/logIn.json") != "")
            {
                SavingCharacterDataClass _CharacterData1 = new SavingCharacterDataClass();

                _CharacterData1 = _CharacterData1.CreateFromJSON(File.ReadAllText(Application.persistentDataPath + "/logIn.json"));
                if (this.gameObject.name == _CharacterData1.PresetValue)
                    presetAlreadySaved = true;
            }
        }
        else // Guest User
        {
            if (File.Exists(Application.persistentDataPath + "/loginAsGuestClass.json") && File.ReadAllText(Application.persistentDataPath + "/loginAsGuestClass.json") != "")
            {
                SavingCharacterDataClass _CharacterData1 = new SavingCharacterDataClass();

                _CharacterData1 = _CharacterData1.CreateFromJSON(File.ReadAllText(Application.persistentDataPath + "/loginAsGuestClass.json"));
                if (this.gameObject.name == _CharacterData1.PresetValue)
                    presetAlreadySaved = true;
            }
        }
    }
    void abcd()
    {
        print("Coroutin Called " + PlayerPrefs.GetInt("presetPanel"));
        if (PlayerPrefs.GetInt("presetPanel") == 1)   // preset panel is enable so saving preset to account 
            PlayerPrefs.SetInt("presetPanel", 0);
        ItemDatabase.instance.GetComponent<SavaCharacterProperties>().SavePlayerProperties();
        avatarController.IntializeAvatar();
    }
}
