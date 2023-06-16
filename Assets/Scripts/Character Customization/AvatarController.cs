﻿
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

public class AvatarController : MonoBehaviour
{
    //public GameObject MyHead;
    public delegate void CharacterLoaded();
    public static event CharacterLoaded characterLoaded;

    // private AddressableAddressableDownloader.Instance AddressableDownloader.Instance;
    public Stitcher stitcher;
    private ItemDatabase itemDatabase;
    //[HideInInspector]
    public GameObject wornHair, wornPant, wornShirt, wornShose, wornEyewearable, wornGloves, wornChain;
    [HideInInspector]
    public int wornHairId, hairColorPaletteId, wornPantId, wornShirtId, wornShoesId, wornEyewearableId, skinId, faceId, eyeBrowId, eyeBrowColorPaletteId, eyesId, eyesColorId, eyesColorPaletteId, noseId, lipsId, lipsColorId, lipsColorPaletteId, bodyFat, makeupId, eyeLashesId, wornGlovesId, wornChainId;
    [HideInInspector]
    public string presetValue;

    public List<Texture> masks = new List<Texture>();
    CharcterBodyParts bodyParts;
    public bool IsInit = false;

    public string dummyJsonForBoxer;

    //NFT avatar color codes

    private void Awake()
    {
        bodyParts = this.GetComponent<CharcterBodyParts>();
        if (SceneManager.GetActiveScene().name == "ARModuleActionScene" || SceneManager.GetActiveScene().name == "ARModuleRoomScene" || SceneManager.GetActiveScene().name == "ARModuleRealityScene")
        {
            transform.localScale *= 2;
        }
        stitcher = new Stitcher();
    }

    private void OnEnable()
    {
        //BoxerNFTEventManager.OnNFTequip += EquipNFT;
        //BoxerNFTEventManager.OnNFTUnequip += UnequipNFT;

        itemDatabase = ItemDatabase.instance;
        //AddressableDownloader.Instance =AddressableDownloader.Instance;

        if (IsInit)
        {
            SetAvatarClothDefault(this.gameObject);
        }

        string currScene = SceneManager.GetActiveScene().name;//Riken Add Condition for Set Default cloths on AR scene so.......
        if (!currScene.Contains("Main")) // call for worlds only
        {
            //Invoke(nameof(IntializeAvatar), 0.5f);
            Invoke(nameof(Custom_IntializeAvatar), 0.5f);
        }

        //Invoke(nameof(Test_EquiptNFT), 1f);
    }

    private void OnDisable()
    {
        //BoxerNFTEventManager.OnNFTequip -= EquipNFT;
        //BoxerNFTEventManager.OnNFTUnequip -= UnequipNFT;
    }

    
    

    public void SetAvatarClothDefault(GameObject applyOn)
    {
        IsInit = false;
        StartCoroutine(DefaultCloths(applyOn));
        
    }

    IEnumerator DefaultCloths(GameObject obj)
    {
        WearDefaultItem("Legs", obj.gameObject);
        yield return new WaitForSeconds(.5f);
        WearDefaultItem("Chest", obj.gameObject);
        yield return new WaitForSeconds(.1f);
        WearDefaultItem("Feet", obj.gameObject);
        yield return new WaitForSeconds(.1f);
        WearDefaultItem("Hair", obj.gameObject);
        obj.GetComponent<CharcterBodyParts>().DefaultTexture();
       
    }
    private void SetItemIdsFromFile(SavingCharacterDataClass _CharacterData)
    {
        presetValue = _CharacterData.PresetValue;
        hairColorPaletteId = _CharacterData.HairColorPaletteValue;
        skinId = _CharacterData.SkinId;
        faceId = _CharacterData.FaceValue;
        eyeBrowId = _CharacterData.EyeBrowValue;
        eyeBrowColorPaletteId = _CharacterData.EyeBrowColorPaletteValue;
        eyesId = _CharacterData.EyeValue;
        eyesColorId = _CharacterData.EyesColorValue;
        eyesColorPaletteId = _CharacterData.EyesColorPaletteValue;
        noseId = _CharacterData.NoseValue;
        lipsId = _CharacterData.LipsValue;
        lipsColorId = _CharacterData.LipsColorValue;
        lipsColorPaletteId = _CharacterData.LipsColorPaletteValue;
        bodyFat = _CharacterData.BodyFat;
        eyeLashesId = _CharacterData.EyeLashesValue;
        makeupId = _CharacterData.MakeupValue;
    }
    /// <summary>
    /// To Inialize Character.
    ///  - Intilaze Store item 
    ///  - Intilaze Character customization (bones, morphes)
    /// </summary>
    /// 

    public SavingCharacterDataClass _CharData = new SavingCharacterDataClass();
    bool getHairColorFormFile = false;
    public void IntializeAvatar(bool canWriteFile = false)
    {
        // Other Requirements 
        // Set "isNFTAquiped" Variable according to Equipt & Unequipt of the NFT


        //{ // Testing Purpose
        //    XanaConstants.xanaConstants.isHoldCharacterNFT = true;
        //    XanaConstants.xanaConstants.isNFTEquiped = true;
        //}

        //if (canWriteFile && /*XanaConstants.xanaConstants.isHoldCharacterNFT &&*/ XanaConstants.xanaConstants.isNFTEquiped)
        //{
        //    BoxerNFTDataClass nftAttributes = new BoxerNFTDataClass();
        //    string _Path = Application.persistentDataPath + XanaConstants.xanaConstants.NFTBoxerJson;
        //    nftAttributes = nftAttributes.CreateFromJSON(File.ReadAllText(_Path));
        //    CreateOrUpdateBoxerFile(nftAttributes);
        //}
        Debug.Log("here -6");
        Custom_IntializeAvatar();
        Debug.Log("here -6");

        //if (PlayerPrefs.GetInt("IsLoggedIn") == 1 && File.Exists(Application.persistentDataPath + "/BoxerNFTData.json") && File.ReadAllText(Application.persistentDataPath + "/BoxerNFTData.json") != "")
        //{
        //    BoxerNFTDataClass _NFTData = new BoxerNFTDataClass();
        //    _NFTData = JsonUtility.FromJson<BoxerNFTDataClass>(Application.persistentDataPath + "/BoxerNFTData.json");

        //    if (_NFTData.isNFTAquiped)
        //    {
        //        IntializeAvatar_BoxerNFT(_NFTData);
        //    }
        //    else
        //    {
        //        // Default Working
        //        Custom_IntializeAvatar();
        //    }
        //}
        //else
        //{
        //    // Default Working
        //Custom_IntializeAvatar();
        //}
    }
    void Custom_IntializeAvatar()
    {
        Debug.Log("here -60");
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "") //Check if data exist
        {
            Debug.Log("here -61");
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
            _CharData = _CharacterData;
            if (SceneManager.GetActiveScene().name.Contains("Main")) // for store/ main menu
            {
                if (_CharacterData.myItemObj.Count > 0)
                {
                    for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(_CharacterData.myItemObj[i].ItemName))
                        {
                            string type = _CharacterData.myItemObj[i].ItemType;
                            if (type.Contains("Legs") || type.Contains("Chest") || type.Contains("Feet") || type.Contains("Hair") || type.Contains("EyeWearable") || type.Contains("Glove") || type.Contains("Chain"))
                            {
                                getHairColorFormFile = true;
                                if (!_CharacterData.myItemObj[i].ItemName.Contains("md", System.StringComparison.CurrentCultureIgnoreCase))
                                {
                                    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(_CharacterData.myItemObj[i].ItemID, _CharacterData.myItemObj[i].ItemName, type, this.gameObject.GetComponent<AvatarController>()));
                                }
                                else
                                {
                                    //if (XanaConstants.xanaConstants.isNFTEquiped)
                                    //{
                                    //    if (_CharacterData.myItemObj[i].ItemType.Contains("Chest"))
                                    //    {
                                    //        if (wornShirt)
                                    //        {
                                    //            UnStichItem("Chest");
                                    //            bodyParts.TextureForShirt(null);
                                    //        }
                                    //    }
                                    //    else if (_CharacterData.myItemObj[i].ItemType.Contains("Hair"))
                                    //    {
                                    //        if (wornHair)
                                    //            UnStichItem("Hair");
                                    //    }
                                    //    else if (_CharacterData.myItemObj[i].ItemType.Contains("Legs"))
                                    //    {
                                    //        if (wornPant)
                                    //        {
                                    //            UnStichItem("Legs");
                                    //            bodyParts.TextureForPant(null);
                                    //        }
                                    //    }
                                    //    else if (_CharacterData.myItemObj[i].ItemType.Contains("Feet"))
                                    //    {
                                    //        if (wornShose)
                                    //        {
                                    //            UnStichItem("Feet");
                                    //            bodyParts.TextureForShoes(null);
                                    //        }

                                    //    }
                                    //    else if (_CharacterData.myItemObj[i].ItemType.Contains("EyeWearable"))
                                    //    {
                                    //        if (wornEyewearable)
                                    //            UnStichItem("EyeWearable");
                                    //    }
                                    //    else if (_CharacterData.myItemObj[i].ItemType.Contains("Glove"))
                                    //    {
                                    //        if (wornGloves)
                                    //        {
                                    //            UnStichItem("Glove");
                                    //            bodyParts.TextureForGlove(null);
                                    //        }

                                    //    }
                                    //    else if (_CharacterData.myItemObj[i].ItemType.Contains("Chain"))
                                    //    {
                                    //        if (wornChain)
                                    //            UnStichItem("Chain");
                                    //    }

                                    //}
                                    //else
                                    //{
                                        WearDefaultItem(type, this.gameObject);
                                    //}
                                }
                            }
                            else
                            {
                                WearDefaultItem(_CharacterData.myItemObj[i].ItemType, this.gameObject);
                            }
                        }
                        else // wear the default item of that specific part.
                        {
                            //if (XanaConstants.xanaConstants.isNFTEquiped && _CharacterData.myItemObj[i].ItemType.Contains("Chest"))
                            //{
                            //    if (wornShirt)
                            //        UnStichItem("Chest");
                            //    bodyParts.TextureForShirt(null);
                            //}
                            //else
                            //{
                                WearDefaultItem(_CharacterData.myItemObj[i].ItemType, this.gameObject);
                            //}
                        }
                    }
                }
                // Added By WaqasAhmad
                // When User Reset From Store 
                // _CharacterData file clear & no Data is available
                // Implemented Default Cloths
                else
                {
                    WearDefaultItem("Legs", this.gameObject);
                    WearDefaultItem("Chest", this.gameObject);
                    WearDefaultItem("Feet", this.gameObject);
                    WearDefaultItem("Hair", this.gameObject);
                }
                //else
                //{
                //    SetAvatarDefault();
                //}
                //if (_CharacterData.eyeTextureName != "" && _CharacterData.eyeTextureName != null)
                //{
                //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeTextureName, this.gameObject, CurrentTextureType.EyeLense));
                //}

                //if (_CharacterData.eyeLashesName != "" && _CharacterData.eyeLashesName != null)
                //{
                //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeLashesName, this.gameObject, CurrentTextureType.EyeLashes));
                //}
                //if (_CharacterData.eyebrrowTexture != "" && _CharacterData.eyebrrowTexture != null)
                //{
                //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyebrrowTexture, this.gameObject, CurrentTextureType.EyeBrows));
                //}
                ////if (_CharacterData.eyeBrowName != "" && _CharacterData.eyeBrowName != null)
                ////{
                ////    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeBrowName, this.gameObject));
                ////}

                //if (_CharacterData.makeupName != "" && _CharacterData.makeupName != null)
                //{
                //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.makeupName, this.gameObject, CurrentTextureType.Makeup));
                //}

                //New texture are downloading for Boxer NFT 
                //if (!string.IsNullOrEmpty(_CharacterData.faceTattooTextureName) && _CharacterData.faceTattooTextureName != null)
                //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.faceTattooTextureName, this.gameObject, CurrentTextureType.FaceTattoo));
                //else
                //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.FaceTattoo);

                //if (!string.IsNullOrEmpty(_CharacterData.chestTattooTextureName) && _CharacterData.chestTattooTextureName != null)
                //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.chestTattooTextureName, this.gameObject, CurrentTextureType.ChestTattoo));
                //else
                //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.ChestTattoo);

                //if (!string.IsNullOrEmpty(_CharacterData.legsTattooTextureName) && _CharacterData.legsTattooTextureName != null)
                //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.legsTattooTextureName, this.gameObject, CurrentTextureType.LegsTattoo));
                //else
                //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.LegsTattoo);

                //if (!string.IsNullOrEmpty(_CharacterData.armTattooTextureName) && _CharacterData.armTattooTextureName != null)
                //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.armTattooTextureName, this.gameObject, CurrentTextureType.ArmTattoo));
                //else
                //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.ArmTattoo);

                //if (!string.IsNullOrEmpty(_CharacterData.mustacheTextureName) && _CharacterData.mustacheTextureName != null)
                //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.mustacheTextureName, this.gameObject, CurrentTextureType.Mustache));
                //else
                //    this.GetComponent<CharcterBodyParts>().RemoveMustacheTexture(null, this.gameObject);

                //if (!string.IsNullOrEmpty(_CharacterData.eyeLidTextureName) && _CharacterData.eyeLidTextureName != null)
                //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeLidTextureName, this.gameObject, CurrentTextureType.EyeLid));
                //else
                //    this.GetComponent<CharcterBodyParts>().RemoveEyeLidTexture(null, this.gameObject);



                LoadBonesData(_CharacterData, this.gameObject);


                //if (_CharacterData.Skin != null && _CharacterData.LipColor!=null && _CharacterData.HairColor != null &&  _CharacterData.EyebrowColor != null && _CharacterData.EyeColor != null)
                //{
                //    //bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, this.gameObject));
                //    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, _CharacterData.HairColor, _CharacterData.EyebrowColor, _CharacterData.EyeColor, this.gameObject));
                //}

                // Seperate 
                if (_CharacterData.Skin != null)
                {
                    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, SliderType.Skin, this.gameObject));
                }
                if (_CharacterData.EyeColor != null)
                {
                    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.EyeColor, SliderType.EyesColor, this.gameObject));
                }
                if (_CharacterData.LipColor != null)
                {
                    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.LipColor, SliderType.LipsColor, this.gameObject));
                }
                // Commented By WaqasAhmad
                // Reason Changing Hair color when asset download completely in Sticth method
                //if (_CharacterData.HairColor != null)
                //{
                //    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.HairColor, SliderType.HairColor, this.gameObject));
                //}
                if (_CharacterData.EyebrowColor != null)
                {
                    Color tempColor = _CharacterData.EyebrowColor;
                    tempColor.a = 1;
                    _CharacterData.EyebrowColor = tempColor;
                    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.EyebrowColor, SliderType.EyeBrowColor, this.gameObject));
                }


                if (_CharacterData.SkinGerdientColor != null)
                {
                    bodyParts.ApplyGredientColor(_CharacterData.SkinGerdientColor, this.gameObject);
                }
                else
                {
                    bodyParts.ApplyGredientDefault(this.gameObject);
                }

                if (_CharacterData.SssIntensity != null)
                {
                    bodyParts.SetSssIntensity(_CharacterData.SssIntensity, this.gameObject);
                }
                else
                {
                    bodyParts.SetSssIntensity(bodyParts.defaultSssValue, this.gameObject);
                }
                SetItemIdsFromFile(_CharacterData);
                bodyParts.LoadBlendShapes(_CharacterData, this.gameObject);
            }
            else // wolrd scence 
            {
                if (GetComponent<PhotonView>() && GetComponent<PhotonView>().IsMine) // self
                {
                    if (_CharacterData.myItemObj.Count > 0)
                    {
                        for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(_CharacterData.myItemObj[i].ItemName))
                            {
                                string type = _CharacterData.myItemObj[i].ItemType;
                                if (type.Contains("Legs") || type.Contains("Chest") || type.Contains("Feet") || type.Contains("Hair") || type.Contains("EyeWearable") || type.Contains("Glove") || type.Contains("Chain"))
                                {
                                    getHairColorFormFile = true;
                                    if (!_CharacterData.myItemObj[i].ItemName.Contains("md", System.StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(_CharacterData.myItemObj[i].ItemID, _CharacterData.myItemObj[i].ItemName, type, this.gameObject.GetComponent<AvatarController>()));
                                    }
                                    else
                                    {
                                        //if (XanaConstants.xanaConstants.isNFTEquiped)
                                        //{
                                        //    if (_CharacterData.myItemObj[i].ItemType.Contains("Chest"))
                                        //    {
                                        //        if (wornShirt)
                                        //        {
                                        //            UnStichItem("Chest");
                                        //            bodyParts.TextureForShirt(null);
                                        //        }
                                        //    }
                                        //    else if (_CharacterData.myItemObj[i].ItemType.Contains("Hair"))
                                        //    {
                                        //        if (wornHair)
                                        //            UnStichItem("Hair");
                                        //    }
                                        //    else if (_CharacterData.myItemObj[i].ItemType.Contains("Legs"))
                                        //    {
                                        //        if (wornPant)
                                        //        {
                                        //            UnStichItem("Legs");
                                        //            bodyParts.TextureForPant(null);
                                        //        }
                                        //    }
                                        //    else if (_CharacterData.myItemObj[i].ItemType.Contains("Feet"))
                                        //    {
                                        //        if (wornShose)
                                        //        {
                                        //            UnStichItem("Feet");
                                        //            bodyParts.TextureForShoes(null);
                                        //        }

                                        //    }
                                        //    else if (_CharacterData.myItemObj[i].ItemType.Contains("EyeWearable"))
                                        //    {
                                        //        if (wornEyewearable)
                                        //            UnStichItem("EyeWearable");
                                        //    }
                                        //    else if (_CharacterData.myItemObj[i].ItemType.Contains("Glove"))
                                        //    {
                                        //        if (wornGloves)
                                        //        {
                                        //            UnStichItem("Glove");
                                        //            bodyParts.TextureForGlove(null);
                                        //        }

                                        //    }
                                        //    else if (_CharacterData.myItemObj[i].ItemType.Contains("Chain"))
                                        //    {
                                        //        if (wornChain)
                                        //            UnStichItem("Chain");
                                        //    }

                                        //}
                                        //else
                                        //{
                                            WearDefaultItem(type, this.gameObject);
                                       // }
                                    }
                                }
                                else
                                {
                                    WearDefaultItem(_CharacterData.myItemObj[i].ItemType, this.gameObject);
                                }
                            }
                            else // wear the default item of that specific part.
                            {
                                //if (XanaConstants.xanaConstants.isNFTEquiped && _CharacterData.myItemObj[i].ItemType.Contains("Chest"))
                                //{
                                //    if (wornShirt)
                                //        UnStichItem("Chest");
                                //    bodyParts.TextureForShirt(null);
                                //}
                                //else
                                //{
                                    WearDefaultItem(_CharacterData.myItemObj[i].ItemType, this.gameObject);
                             //   }

                            }
                        }
                    }
                    else
                    {
                        WearDefaultItem("Legs", this.gameObject);
                        WearDefaultItem("Chest", this.gameObject);
                        WearDefaultItem("Feet", this.gameObject);
                        WearDefaultItem("Hair", this.gameObject);
                    }
                    //if (_CharacterData.eyeTextureName != "" && _CharacterData.eyeTextureName != null)
                    //{
                    //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeTextureName, this.gameObject, CurrentTextureType.EyeLense));
                    //}
                    //if (_CharacterData.eyebrrowTexture != "" && _CharacterData.eyebrrowTexture != null)
                    //{
                    //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyebrrowTexture, this.gameObject, CurrentTextureType.EyeBrows));
                    //}
                    //if (_CharacterData.eyeLashesName != "" && _CharacterData.eyeLashesName != null)
                    //{
                    //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeLashesName, this.gameObject, CurrentTextureType.EyeLashes));
                    //}
                    ////if (_CharacterData.eyeBrowName != "" && _CharacterData.eyeBrowName != null)
                    ////{
                    ////    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeBrowName, this.gameObject));
                    ////}
                    //if (_CharacterData.makeupName != "" && _CharacterData.makeupName != null)
                    //{
                    //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.makeupName, this.gameObject, CurrentTextureType.Makeup));
                    //}


                    ////New texture are downloading for Boxer NFT 
                    //if (!string.IsNullOrEmpty(_CharacterData.faceTattooTextureName) && _CharacterData.faceTattooTextureName != null)
                    //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.faceTattooTextureName, this.gameObject, CurrentTextureType.FaceTattoo));
                    //else
                    //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.FaceTattoo);

                    //if (!string.IsNullOrEmpty(_CharacterData.chestTattooTextureName) && _CharacterData.chestTattooTextureName != null)
                    //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.chestTattooTextureName, this.gameObject, CurrentTextureType.ChestTattoo));
                    //else
                    //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.ChestTattoo);

                    //if (!string.IsNullOrEmpty(_CharacterData.legsTattooTextureName) && _CharacterData.legsTattooTextureName != null)
                    //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.legsTattooTextureName, this.gameObject, CurrentTextureType.LegsTattoo));
                    //else
                    //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.LegsTattoo);

                    //if (!string.IsNullOrEmpty(_CharacterData.armTattooTextureName) && _CharacterData.armTattooTextureName != null)
                    //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.armTattooTextureName, this.gameObject, CurrentTextureType.ArmTattoo));
                    //else
                    //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.ArmTattoo);

                    //if (!string.IsNullOrEmpty(_CharacterData.mustacheTextureName) && _CharacterData.mustacheTextureName != null)
                    //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.mustacheTextureName, this.gameObject, CurrentTextureType.Mustache));
                    //else
                    //    this.GetComponent<CharcterBodyParts>().RemoveMustacheTexture(null, this.gameObject);

                    //if (!string.IsNullOrEmpty(_CharacterData.eyeLidTextureName) && _CharacterData.eyeLidTextureName != null)
                    //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeLidTextureName, this.gameObject, CurrentTextureType.EyeLid));
                    //else
                    //    this.GetComponent<CharcterBodyParts>().RemoveEyeLidTexture(null, this.gameObject);


                    //if (_CharacterData.SkinGerdientColor != null && _CharacterData.SssIntensity != null)
                    //{
                    //    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, _CharacterData.SkinGerdientColor, this.gameObject));
                    //}
                    //else
                    //{
                    //    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, this.gameObject));
                    //}

                    //if (_CharacterData.Skin != null && _CharacterData.LipColor != null && _CharacterData.HairColor != null && _CharacterData.EyebrowColor != null && _CharacterData.EyeColor != null)
                    //{
                    //    //bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, this.gameObject));
                    //    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, _CharacterData.HairColor, _CharacterData.EyebrowColor, _CharacterData.EyeColor, this.gameObject));
                    //}

                    // Seperate 
                    if (_CharacterData.Skin != null)
                    {
                        bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, SliderType.Skin, this.gameObject));
                    }
                    if (_CharacterData.EyeColor != null)
                    {
                        bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.EyeColor, SliderType.EyesColor, this.gameObject));
                    }
                    if (_CharacterData.LipColor != null)
                    {
                        bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.LipColor, SliderType.LipsColor, this.gameObject));
                    }
                    //if (_CharacterData.HairColor != null)
                    //{
                    //    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.HairColor, SliderType.HairColor, this.gameObject));
                    //}
                    if (_CharacterData.EyebrowColor != null)
                    {
                        bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.EyebrowColor, SliderType.EyeBrowColor, this.gameObject));
                    }

                    if (_CharacterData.SkinGerdientColor != null)
                    {
                        bodyParts.ApplyGredientColor(_CharacterData.SkinGerdientColor, this.gameObject);
                    }
                    else
                    {
                        bodyParts.ApplyGredientDefault(this.gameObject);
                    }

                    //if (_CharacterData.SssIntensity != null)
                    //{
                    //    bodyParts.SetSssIntensity(_CharacterData.SssIntensity, this.gameObject);
                    //}
                    //else
                    //{
                    //    bodyParts.SetSssIntensity(bodyParts.defaultSssValue, this.gameObject);
                    //}

                    bodyParts.SetSssIntensity(0, this.gameObject);
                    bodyParts.LoadBlendShapes(_CharacterData, this.gameObject);
                    LoadBonesData(_CharacterData, this.gameObject);

                }
            }


            //BoxerNFTDataClass _NFTData = new BoxerNFTDataClass();
            //_NFTData.isNFTAquiped = true;

            //string updatedBoxerData = JsonUtility.ToJson(_NFTData);
            //File.WriteAllText((Application.persistentDataPath + XanaConstants.xanaConstants.NFTBoxerJson), updatedBoxerData);
        }
        //if (XanaConstants.xanaConstants.isNFTEquiped)
        //    LoadingHandler.Instance.nftLoadingScreen.SetActive(false);
    }

    /// <summary>
    /// Activate Boxer Avater. 
    /// Changes/Disable/Enable Items Releated to Boxer Data
    /// </summary>
    //public void IntializeAvatar_BoxerNFT(BoxerNFTDataClass _NFTData)
    //{
    //    // Already LoggedIn & Have Boxer NFT Equipt
    //    // NFT Json => (Application.persistentDataPath + "/BoxerNFTData.json");
    //    {
    //        if (this.wornEyewearable != null) // to unwear galasses if already equipped.
    //        {
    //            this.UnStichItem("EyeWearable");
    //        }

    //        if (SceneManager.GetActiveScene().name.Contains("Main")) // for store/ main menu
    //        {
    //            // Set All Textures to Default
    //            bodyParts.DefaultTexture();

    //            // Reset BlendShape & Bone to Default
    //            CharcterBodyParts parts = GetComponent<CharcterBodyParts>();
    //            ResetBonesDefault(parts);

    //            SkinnedMeshRenderer effectedHead = parts.Head.GetComponent<SkinnedMeshRenderer>();
    //            for (int i = 0; i < effectedHead.sharedMesh.blendShapeCount; i++)
    //            {
    //                effectedHead.SetBlendShapeWeight(i, 0);
    //            }

    //            // Change Textures According to Requirements
    //            AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_NFTData.eyeLens, this.gameObject));
    //            AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_NFTData.eyebrow, this.gameObject));
    //            AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_NFTData.skin, this.gameObject));


    //            if (string.IsNullOrEmpty(_NFTData.fullCostumes))
    //            {
    //                // NFT Not Wearing Shirt
    //                // Unwear Shirt if already equipped.
    //                // Remove Shirt Mask Aswell
    //                if (this.wornShirt != null)
    //                {
    //                    this.UnStichItem("Chest");
    //                    bodyParts.Body.materials[0].SetTexture("_Shirt_Mask", null);
    //                }
    //            }
    //            else
    //                AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.pant, "Chest", this.gameObject.GetComponent<AvatarController>()));

    //            AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.hair, "Hair", this.gameObject.GetComponent<AvatarController>()));
    //            AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.pant, "Legs", this.gameObject.GetComponent<AvatarController>()));
    //            AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.shoes, "Feet", this.gameObject.GetComponent<AvatarController>()));
    //            AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.sunglasses, "EyeWearable", this.gameObject.GetComponent<AvatarController>()));
    //            AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.glove, "Glove", this.gameObject.GetComponent<AvatarController>()));

    //           // Remining Items
    //           // Make Eye Shape with Blend & Bones

    //            CreateOrUpdateBoxerFile();
    //        }
    //        else // wolrd scence 
    //        {
    //            if (GetComponent<PhotonView>() && GetComponent<PhotonView>().IsMine) // self
    //            {

    //                // Set All Textures to Default
    //                bodyParts.DefaultTexture();

    //                // Change Textures According to Requirements
    //                AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_NFTData.eyeLens, this.gameObject));
    //                AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_NFTData.eyebrow, this.gameObject));
    //                AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_NFTData.skin, this.gameObject));


    //                if (string.IsNullOrEmpty(_NFTData.fullCostumes))
    //                {
    //                    // NFT Not Wearing Shirt
    //                    // Unwear Shirt if already equipped.
    //                    // Remove Shirt Mask Aswell
    //                    if (this.wornShirt != null)
    //                    {
    //                        this.UnStichItem("Chest");
    //                        bodyParts.Body.materials[0].SetTexture("_Shirt_Mask", null);
    //                    }
    //                }
    //                else
    //                    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.pant, "Chest", this.gameObject.GetComponent<AvatarController>()));

    //                AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.hair, "Hair", this.gameObject.GetComponent<AvatarController>()));
    //                AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.pant, "Legs", this.gameObject.GetComponent<AvatarController>()));
    //                AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.shoes, "Feet", this.gameObject.GetComponent<AvatarController>()));
    //                AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.sunglasses, "EyeWearable", this.gameObject.GetComponent<AvatarController>()));
    //                AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(1, _NFTData.glove, "Glove", this.gameObject.GetComponent<AvatarController>()));

    //                //bodyParts.LoadBlendShapes(_CharacterData, this.gameObject);
    //            }
    //        }
    //    }
    //}
    /// <summary>
    /// For Boxer NFT there is not modification in data
    /// We only need to save file each time
    /// Not Detection Available for NFT changed so Each time update & Save
    /// </summary>
    //void CreateOrUpdateBoxerFile(BoxerNFTDataClass _NFTData)
    //{
    //    CharcterBodyParts charcterBodyParts = CharcterBodyParts.instance;
    //    SavingCharacterDataClass _CharacterData1 = new SavingCharacterDataClass();

    //    // Create Class New Object 
    //    // Setting Default Values into this Object
    //    {
    //        _CharacterData1.eyeTextureName = "";
    //        _CharacterData1.Skin = bodyParts.DefaultSkinColor;
    //        _CharacterData1.LipColor = bodyParts.DefaultLipColor;
    //        _CharacterData1.EyebrowColor = bodyParts.DefaultEyebrowColor;
    //        //_CharacterData1.EyebrowColor = Color.white;
    //        _CharacterData1.HairColor = bodyParts.DefaultHairColor;

    //        _CharacterData1.makeupName = bodyParts.defaultMakeup.name;
    //        _CharacterData1.eyeLashesName = bodyParts.defaultEyelashes.name;
    //        _CharacterData1.eyebrrowTexture = bodyParts.defaultEyebrow.name;

    //        _CharacterData1.id = LoadPlayerAvatar.avatarId;
    //        _CharacterData1.name = LoadPlayerAvatar.avatarName;
    //        _CharacterData1.thumbnail = LoadPlayerAvatar.avatarThumbnailUrl;
    //        _CharacterData1.SkinId = this.skinId;
    //        _CharacterData1.PresetValue = this.presetValue;
    //        _CharacterData1.FaceValue = this.faceId;
    //        _CharacterData1.EyeBrowValue = this.eyeBrowId;
    //        _CharacterData1.EyeLashesValue = this.eyeLashesId;
    //        _CharacterData1.EyeValue = this.eyesId;
    //        _CharacterData1.EyesColorValue = this.eyesColorId;
    //        _CharacterData1.NoseValue = this.noseId;
    //        _CharacterData1.LipsValue = this.lipsId;
    //        _CharacterData1.LipsColorValue = this.lipsColorId;
    //        _CharacterData1.BodyFat = this.bodyFat;
    //        _CharacterData1.MakeupValue = this.makeupId;
    //        _CharacterData1.faceMorphed = XanaConstants.xanaConstants.isFaceMorphed;
    //        _CharacterData1.eyeBrowMorphed = XanaConstants.xanaConstants.isEyebrowMorphed;
    //        _CharacterData1.eyeMorphed = XanaConstants.xanaConstants.isEyeMorphed;
    //        _CharacterData1.noseMorphed = XanaConstants.xanaConstants.isNoseMorphed;
    //        _CharacterData1.lipMorphed = XanaConstants.xanaConstants.isLipMorphed;

    //        _CharacterData1.SavedBones = new List<BoneDataContainer>();
    //        for (int i = 0; i < charcterBodyParts.BonesData.Count; i++)
    //        {
    //            Transform bone = charcterBodyParts.BonesData[i].Obj.transform;
    //            _CharacterData1.SavedBones.Add(new BoneDataContainer(charcterBodyParts.BonesData[i].Name, bone.localPosition, bone.localEulerAngles, bone.localScale));
    //        }

    //        int totaBlendShapes = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount;
    //        _CharacterData1.FaceBlendsShapes = new float[totaBlendShapes];

    //        _CharacterData1.faceTattooTextureName = string.Empty;
    //        _CharacterData1.chestTattooTextureName = string.Empty;
    //        _CharacterData1.legsTattooTextureName = string.Empty;
    //        _CharacterData1.armTattooTextureName = string.Empty;
    //        _CharacterData1.mustacheTextureName = string.Empty;
    //        _CharacterData1.eyeLidTextureName = string.Empty;
    //    }

    //    int listCurrentIndex = 0;
    //    _CharacterData1.myItemObj = new List<Item>();

    //    // Cloths , Gloves , Chains , glasses
    //    {
    //        if (!string.IsNullOrEmpty(_NFTData.Pants))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Pants, "Pants_");

    //            _CharacterData1.myItemObj.Add(new Item(-1, tempKey, "Legs"));
    //            listCurrentIndex++;
    //        }
    //        else
    //        {
    //            _CharacterData1.myItemObj.Add(new Item(-1, "md", "Legs"));
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Full_Costumes))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Full_Costumes, "Full_Costume_");

    //            _CharacterData1.myItemObj.Add(new Item(-1, tempKey, "Chest"));
    //            listCurrentIndex++;
    //        }
    //        else
    //        {
    //            _CharacterData1.myItemObj.Add(new Item(-1, "md", "Chest"));
    //            if (wornShirt)
    //            {
    //                UnStichItem("Chest");
    //                bodyParts.TextureForShirt(null);
    //            }
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Hairs))
    //        {
    //            string tempKey = _NFTData.Hairs.Split('-')[0];
    //            tempKey = GetUpdatedKey(tempKey, "Hairs_");
    //            // need to split // its also has color code

    //            _CharacterData1.myItemObj.Add(new Item(-1, tempKey, "Hair"));
    //            listCurrentIndex++;
    //        }
    //        else
    //        {
    //            _CharacterData1.myItemObj.Add(new Item(-1, "md", "Hair"));
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Shoes))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Shoes, "Shoes_");

    //            _CharacterData1.myItemObj.Add(new Item(-1, tempKey, "Feet"));
    //            listCurrentIndex++;
    //        }
    //        else
    //        {
    //            _CharacterData1.myItemObj.Add(new Item(-1, "md", "Feet"));
    //            if (wornShose)
    //            {
    //                UnStichItem("Feet");
    //                bodyParts.TextureForShoes(null);
    //            }
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Glasses))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Glasses, "Glasses_");

    //            _CharacterData1.myItemObj.Add(new Item(-1, tempKey, "EyeWearable"));
    //            listCurrentIndex++;
    //        }
    //        else
    //        {
    //            _CharacterData1.myItemObj.Add(new Item(-1, "md", "EyeWearable"));
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Gloves))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Gloves, "Gloves_");

    //            _CharacterData1.myItemObj.Add(new Item(-1, tempKey, "Glove"));
    //            listCurrentIndex++;
    //        }
    //        else
    //        {
    //            _CharacterData1.myItemObj.Add(new Item(-1, "md", "Glove"));
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Chains))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Chains, "Chains_");

    //            _CharacterData1.myItemObj.Add(new Item(-1, tempKey, "Chain"));
    //            listCurrentIndex++;
    //        }
    //        else
    //        {
    //            _CharacterData1.myItemObj.Add(new Item(-1, "md", "Chain"));
    //        }
    //    }

    //    // Tattoo , Mustache , EyeLid
    //    {
    //        if (!string.IsNullOrEmpty(_NFTData.Face_Tattoo))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Face_Tattoo, "Face_Tattoo_");
    //            bodyParts.faceTattooColor = bodyParts.faceTattooColorDefault;
    //            _CharacterData1.faceTattooTextureName = tempKey;
    //        }
    //        else if (!string.IsNullOrEmpty(_NFTData.Forehead_Tattoo))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Forehead_Tattoo, "Forehead_Tattoo_");
    //            bodyParts.faceTattooColor = bodyParts.foreheafTattooColor;
    //            _CharacterData1.faceTattooTextureName = tempKey;
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Chest_Tattoo))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Chest_Tattoo, "Chest_Tattoo_");
    //            _CharacterData1.chestTattooTextureName = tempKey;
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Legs_Tattoo))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Legs_Tattoo, "Legs_Tattoo_");
    //            _CharacterData1.legsTattooTextureName = tempKey;
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Arm_Tattoo))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Arm_Tattoo, "Arm_Tattoo_");
    //            _CharacterData1.armTattooTextureName = tempKey;
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Mustache))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Mustache, "Mustache_");
    //            _CharacterData1.mustacheTextureName = tempKey;
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Eyelid))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Eyelid, "Eyelid_");
    //            _CharacterData1.eyeLidTextureName = tempKey;
    //        }
    //    }

    //    // EyeShape, Lense, Skin, lips , Hairs , Eyebrows
    //    {
    //        if (!string.IsNullOrEmpty(_NFTData.Eye_Shapes))
    //        {
    //            string tempKey = _NFTData.Eye_Shapes.Replace(" - ", "_");
    //            tempKey = tempKey.Split('_')[0];
    //            NFTBoxerEyeData.instance.SetNFTData(tempKey);
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Eye_Lense))
    //        {
    //            string tempKey = GetUpdatedKey(_NFTData.Eye_Lense, "Eye_Lense_");
    //            _CharacterData1.eyeTextureName = tempKey;
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Eyebrows))
    //        {
    //            string temp = _NFTData.Eyebrows.Replace(" - ", "_").Split('_')[0];

    //            string tempKey = GetUpdatedKey(temp, "Eyebrows_");
    //            Debug.LogError(tempKey);
    //            _CharacterData1.eyebrrowTexture = tempKey;
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Skin))
    //        {
    //            string tempKey = GetColorCodeFromNFTKey(_NFTData.Skin);
    //            _CharacterData1.Skin = GetColorCode(tempKey);
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Lips))
    //        {
    //            string tempKey = GetColorCodeFromNFTKey(_NFTData.Lips);
    //            _CharacterData1.LipColor = GetColorCode(tempKey);
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Eyebrows))
    //        {
    //            //string temp = _NFTData.Eyebrows.Replace(" - ", " _").Split('_')[1];

    //            string tempKey = GetColorCodeFromNFTKey(_NFTData.Eyebrows);
    //            _CharacterData1.EyebrowColor = GetColorCode(tempKey);
    //        }

    //        if (!string.IsNullOrEmpty(_NFTData.Hairs))
    //        {
    //            string tempKey = GetColorCodeFromNFTKey(_NFTData.Hairs);
    //            Debug.LogError(tempKey);
    //            _CharacterData1.HairColor = GetColorCode(tempKey);
    //        }
    //    }


    //    string updatedBoxerData = JsonUtility.ToJson(_CharacterData1);
    //    File.WriteAllText((Application.persistentDataPath + XanaConstants.xanaConstants.NFTBoxerJson), updatedBoxerData);
    //}

    public void WearDefaultItem(string type, GameObject applyOn)
    {
        switch (type)
        {
            case "Legs":
                //AddOrRemoveClothes((ItemDatabase.instance.itemList[8].ItemPrefab), "naked_legs", "Legs", "MDpant", 0);
                StichItem(-1, ItemDatabase.instance.DefaultPent, type, applyOn);
                break;
            case "Chest":
                StichItem(-1, ItemDatabase.instance.DefaultShirt, type, applyOn);
                //AddOrRemoveClothes((ItemDatabase.instance.itemList[10].ItemPrefab), "naked_chest", "Chest", "MDshirt", 1);
                break;
            case "Feet":
                StichItem(-1, ItemDatabase.instance.DefaultShoes, type, applyOn);
                //AddOrRemoveClothes((ItemDatabase.instance.itemList[9].ItemPrefab), "naked_slug", "Feet", "MDshoes", 7);
                break;
            case "Hair":
                StichItem(-1, ItemDatabase.instance.DefaultHair, type, applyOn);
                //AddOrRemoveClothes((ItemDatabase.instance.itemList[11].ItemPrefab), "bald_head", "Hair", "MDhairs", 2);
                break;
            default:
                break;
        }
    }

    //public void ResetForLastSaved()
    //{

    //    //body fats
    //    SavaCharacterProperties.instance.SaveItemList.BodyFat = 0;
    //    //body blends
    //    CharacterCustomizationManager.Instance.UpdateChBodyShape(0);
    //    //lips
    //    // GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[1].mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultLips;
    //    //eyesdefault    

    //    //Change Eye Color Here For Default
    //    // GameManager.Instance.EyeballTexture1.material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultEyes;
    //    //GameManager.Instance.EyeballTexture2.material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultEyes;
    //    //"Skin"
    //    //GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[0].mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultSkin;
    //    //for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
    //    //{
    //    //    if (GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
    //    //        GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.mainTexture = SavaCharacterProperties.instance.GetComponent<ItemDatabase>().DefaultSkin;
    //    //}
    //    ItemDatabase.instance.RevertSavedCloths();
    //}

    public void LastSaved_Reset()
    {
        ItemDatabase.instance.RevertSavedCloths();
    }

    /// <summary>
    /// 1 - Update body according to fat
    /// 2 -Fit cloth according to the selected body type
    /// </summary>
    public void ResizeClothToBodyFat(GameObject applyOn, int bodyFat)
    {
        //Equipment equipment = applyOn.GetComponent<Equipment>();
        CharcterBodyParts bodyparts = applyOn.GetComponent<CharcterBodyParts>();

        float _size = 1f + ((float)bodyFat / 100f);
        Debug.LogError("Resizing Body Parts & Cloths : " + bodyFat + "  :  " + _size);

        if (bodyparts._scaleBodyParts.Count > 0)
        {
            for (int i = 0; i < bodyparts._scaleBodyParts.Count; i++)
            {
                bodyparts._scaleBodyParts[i].transform.localScale = new Vector3(_size, 1, _size);
            }
        }
    }


    /// <summary>
    /// To Load data from file
    /// </summary>
    /// <param name="_CharacterData"> player data save in file</param>
    /// <param name="applyOn">Object on which data is going to apply</param>
    public void LoadBonesData(SavingCharacterDataClass _CharacterData, GameObject applyOn)
    {
        CharcterBodyParts parts = applyOn.GetComponent<CharcterBodyParts>();
        if (applyOn != null)
        {
            if (_CharacterData != null)
            {
                List<BoneDataContainer> boneData = _CharacterData.SavedBones;
                if (boneData.Count > 0)
                {
                    for (int i = 0; i < boneData.Count; i++)
                    {
                        if (parts.BonesData.Count >= i && boneData[i] != null)
                        {
                            parts.BonesData[i].Obj.transform.localPosition = boneData[i].Pos;
                            parts.BonesData[i].Obj.transform.localScale = boneData[i].Scale;
                        }
                    }
                }
                else
                {
                    ResetBonesDefault(parts);
                }
            }
            else
            {
                if (parts.BonesData.Count > 0)
                {
                    ResetBonesDefault(parts);
                }
            }
        }
    }


    /// <summary>
    /// To reset bones to default pos and scale
    /// </summary>
    /// <param name="parts"> CharcterBodyParts </param>
    public void ResetBonesDefault(CharcterBodyParts parts)
    {
        if (parts.BonesData.Count > 0)
        {
            for (int i = 0; i < parts.BonesData.Count; i++)
            {
                if (parts.BonesData[i].Obj != null)
                {
                    parts.BonesData[i].Obj.transform.localPosition = parts.BonesData[i].Pos;
                    parts.BonesData[i].Obj.transform.localScale = parts.BonesData[i].Scale;
                    parts.BonesData[i].Obj.transform.localEulerAngles = parts.BonesData[i].Rotation;
                }
            }
        }
    }

    /// <summary>
    /// To stich item on player rig
    /// </summary>
    /// <param name="item">Cloth to wear</param>
    /// <param name="applyOn">Player that are going to wear the dress</param>
    public void StichItem(int itemId, GameObject item, string type, GameObject applyOn, bool applyHairColor = true)
    {
        UnStichItem(type);
        if (item.GetComponent<EffectedParts>() && item.GetComponent<EffectedParts>().texture != null)
        {
            Texture tempTex = item.GetComponent<EffectedParts>().texture;
            masks.Add(tempTex);
          //  applyOn.gameObject.GetComponent<CharcterBodyParts>().ApplyMaskTexture(type, tempTex, this.gameObject);
        }

        //if (item.GetComponent<EffectedParts>() && item.GetComponent<EffectedParts>().variation_Texture != null)
        //{
        //    item.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.SetTexture("_BaseMap", item.GetComponent<EffectedParts>().variation_Texture);
        //}

        item = this.stitcher.Stitch(item, applyOn);
        if (applyHairColor && _CharData.HairColor != null/* && getHairColorFormFile */&& type == "Hair")
        {
            getHairColorFormFile = false;
            bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharData.HairColor, SliderType.HairColor, applyOn));
        }
        if (SceneManager.GetActiveScene().name != "Main")
            item.layer = 22;
        else
        {
            // Addind Hair Layer Seperate
            if (type == "Hair")
            {
                item.layer = 25;
            }
            else
                item.layer = 11;

        }
        Debug.Log("Avatar type" + type);
        switch (type)
        {
          
            case "Chest":
                wornShirt = item;
                wornShirtId = itemId;
                wornShirt.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
                break;
            case "Legs":
                wornPant = item;
                wornPantId = itemId;
                wornPant.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
                break;
            case "Hair":
                wornHair = item;
                wornHairId = itemId;
                break;
            case "Feet":
                wornShose = item;
                wornShoesId = itemId;
                wornShose.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
                break;
            case "EyeWearable":
                wornEyewearable = item;
                wornEyewearableId = itemId;
                break;
            case "Chain":
                wornChain = item;
                wornChainId = itemId;
                break;
            case "Glove":
                wornGloves = item;
                wornGlovesId = itemId;
                break;
        }
        if (item.name.Contains("arabic"))
        {
            // Disable Pant
            if (wornPant)
                wornPant.SetActive(false);

            // Disable Hair
            if (wornHair)
                wornHair.SetActive(false);
        }
        else if (wornShirt && (wornShirt.name.Contains("arabic") || wornShirt.name.Contains("Arabic")))
        {
            // Yes Arabic Wear , new pant or hair disable
            if (wornPant)
                wornPant.SetActive(false);

            if (wornHair)
                wornHair.SetActive(false);
        }
        else if (wornShirt && (item.name.Contains("Full_Costume", System.StringComparison.CurrentCultureIgnoreCase) || wornShirt.name.Contains("Full_Costume", System.StringComparison.CurrentCultureIgnoreCase)))
        {
            if (wornPant)
                wornPant.SetActive(false);
            if (wornChain)
                wornChain.SetActive(false);
        }
        else
        {
            if (wornPant)
                wornPant.SetActive(true);
            if (wornHair)
                wornHair.SetActive(true);
            if (wornChain)
                wornChain.SetActive(true);
        }
        //if (PlayerPrefs.GetInt("presetPanel") != 1)
        //{
        //    if (StoreManager.instance.loaderForItems && StoreManager.instance != null)
        //        StoreManager.instance.loaderForItems.SetActive(false);
        //}
        //else
        //{
        //    if (AddressableDownloader.Instance.appliedItemsInPresets >= 2)
        //        StartCoroutine(CloseLoader());
        //}

    }
    //IEnumerator CloseLoader()
    //{
    //    yield return new WaitUntil(() => AddressableDownloader.Instance.isTextureDownloaded);
    //    if (StoreManager.instance.loaderForItems)
    //        StoreManager.instance.loaderForItems.SetActive(false);
    //    AddressableDownloader.Instance.isTextureDownloaded = false;
    //    AddressableDownloader.Instance.appliedItemsInPresets = 0;
    //}
    public void UnStichItem(string type)
    {
        switch (type)
        {
            case "Chest":
                Destroy(wornShirt);
                break;
            case "Legs":
                Destroy(wornPant);
                break;
            case "Hair":
                Destroy(wornHair);
                break;
            case "Feet":
                Destroy(wornShose);
                break;
            case "EyeWearable":
                Destroy(wornEyewearable);
                break;
            case "Chain":
                Destroy(wornChain);
                break;
            case "Glove":
                Destroy(wornGloves);
                break;

        }
    }

    //public IEnumerator RPCMaskApply(GameObject applyOn)
    //{
    //    yield return new WaitForSeconds(1);
    //    if (masks.Count > 0)
    //    {
    //        foreach (var mask in masks)
    //        {
    //            applyOn.gameObject.GetComponent<CharcterBodyParts>().ApplyMaskTexture(mask.name, mask, this.gameObject);
    //        }
    //    }
    //}

    public void ApplyPreset(SavingCharacterDataClass _CharacterData)
    {
        if (_CharacterData.myItemObj.Count > 0)
        {
            for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
            {
                if (!string.IsNullOrEmpty(_CharacterData.myItemObj[i].ItemName))
                {
                    string type = _CharacterData.myItemObj[i].ItemType;
                    if (type.Contains("Legs") || type.Contains("Chest") || type.Contains("Feet") || type.Contains("Hair") || type.Contains("EyeWearable"))
                    {
                        getHairColorFormFile = false;
                        if (!_CharacterData.myItemObj[i].ItemName.Contains("md", System.StringComparison.CurrentCultureIgnoreCase))
                        {
                            AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(_CharacterData.myItemObj[i].ItemID, _CharacterData.myItemObj[i].ItemName, type, this.gameObject.GetComponent<AvatarController>()));
                        }
                        else
                        {
                            WearDefaultItem(type, this.gameObject);
                        }
                    }
                    else
                    {
                        WearDefaultItem(_CharacterData.myItemObj[i].ItemType, this.gameObject);
                    }
                }
                else // wear the default item of that specific part.
                {
                    WearDefaultItem(_CharacterData.myItemObj[i].ItemType, this.gameObject);
                }
            }
        }
        //else
        //{
        //    SetAvatarDefault();
        //}
        //if (_CharacterData.eyeTextureName != "" && _CharacterData.eyeTextureName != null)
        //{
        //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeTextureName, this.gameObject, CurrentTextureType.EyeLense));
        //}
        //if (_CharacterData.eyebrrowTexture != "" && _CharacterData.eyebrrowTexture != null)
        //{
        //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyebrrowTexture, this.gameObject, CurrentTextureType.EyeBrows));
        //}
        //if (_CharacterData.makeupName != "" && _CharacterData.makeupName != null)
        //{
        //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.makeupName, this.gameObject, CurrentTextureType.Makeup));
        //}
        //else
        //{
        //    AddressableDownloader.Instance.StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture("nomakeup", this.gameObject, CurrentTextureType.Makeup));
        //}
        LoadBonesData(_CharacterData, this.gameObject);

        //if (_CharacterData.SkinGerdientColor != null && _CharacterData.SssIntensity != null)
        //{
        //    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, _CharacterData.SkinGerdientColor, this.gameObject));
        //}
        //else
        //{
        //    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, this.gameObject));
        //}
        if (_CharacterData.Skin != null && _CharacterData.LipColor != null && _CharacterData.HairColor != null && _CharacterData.EyebrowColor != null && _CharacterData.EyeColor != null)
        {
            //bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, this.gameObject));
            //bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, _CharacterData.LipColor, _CharacterData.HairColor, _CharacterData.EyebrowColor, _CharacterData.EyeColor, this.gameObject));

            // Seperate 
            if (_CharacterData.Skin != null)
            {
                bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, SliderType.Skin, this.gameObject));
            }
            if (_CharacterData.EyeColor != null)
            {
                bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.EyeColor, SliderType.EyesColor, this.gameObject));
            }
            if (_CharacterData.LipColor != null)
            {
                bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.LipColor, SliderType.LipsColor, this.gameObject));
            }

            // Commented By WaqasAhmad
            //if (_CharacterData.HairColor != null)
            //{
            //    bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.HairColor, SliderType.HairColor, this.gameObject));
            //}
            if (_CharacterData.EyebrowColor != null)
            {
                bodyParts.StartCoroutine(bodyParts.ImplementColors(_CharacterData.EyebrowColor, SliderType.EyeBrowColor, this.gameObject));
            }
        }

        if (_CharacterData.SkinGerdientColor != null)
        {
            bodyParts.ApplyGredientColor(_CharacterData.SkinGerdientColor, this.gameObject);
        }
        else
        {
            bodyParts.ApplyGredientDefault(this.gameObject);
        }

        if (_CharacterData.SssIntensity != null)
        {
            bodyParts.SetSssIntensity(_CharacterData.SssIntensity, this.gameObject);
        }
        else
        {
            bodyParts.SetSssIntensity(bodyParts.defaultSssValue, this.gameObject);
        }
        SetItemIdsFromFile(_CharacterData);
        bodyParts.LoadBlendShapes(_CharacterData, this.gameObject);
        bodyParts.ApplyPresiteGredient();
    }


    string GetUpdatedKey(string Key, string prefixAdded)
    {
        string tempKey;
        if (Key.Contains(" - "))
            tempKey = Key.Replace(" - ", "_");
        else
            tempKey = Key;
        tempKey = tempKey.Replace(" ", "");
        tempKey = prefixAdded + tempKey;
        return tempKey;
    }

    string GetColorCodeFromNFTKey(string key)
    {
        string tempKey;
        if (key.Contains(" - "))
        {
            tempKey = key.Replace(" - ", "_");
            tempKey = tempKey.Split('_').Last();
        }
        else
            tempKey = key;


        tempKey = tempKey.Replace(" ", "");
        return tempKey;
    }


    //public Color GetColorCode(string colorCode)
    //{
    //    for (int i = 0; i < _nftAvatarColorCodes.colorCodes.Count; i++)
    //    {
    //        if (colorCode.ToLower() == _nftAvatarColorCodes.colorCodes[i].colorName.ToLower())
    //        {
    //            //Color tempcolor = _nftAvatarColorCodes.colorCodes[i].colorCode;
    //            //tempcolor.a = 1;
    //            //return tempcolor;
    //            return _nftAvatarColorCodes.colorCodes[i].colorCode;
    //        }
    //    }
    //    return Color.black;
    //}
}
