
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Voice.Unity;
using System.Linq;
using System.IO;
using Photon.Pun.Demo.PunBasics;
public class RPCCallforBufferPlayers : MonoBehaviour, IPunInstantiateMagicCallback
{
    [HideInInspector]
    public AssetBundle bundle;
    public AssetBundleRequest newRequest;
    private string OtherPlayerId;
    public static List<string> bundle_Name = new List<string>();
    private bool ItemAlreadyExists = false;
    bool NeedtoDownload = true;
    bool NotNeedtoDownload = true;
    string ClothSlugName = "";
    [SerializeField]
    public static Dictionary<object, object> allPlayerIdData = new Dictionary<object, object>();
    object[] _mydatatosend = new object[2];
    public string clothDataString;
    private string json = "";
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

        Debug.Log("SetConstant.isGuest ===" + SetConstant.isGuest);
        if (IsGuestBool)
        {
            Debug.Log("SetConstant IsGuestBool ===" + SetConstant.isGuest);

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
            Debug.Log("!SetConstant.isGuest ===" + SetConstant.isGuest);

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
    private void Start()
    {
        Debug.Log("Json call first===");
        if (this.GetComponent<PhotonView>().IsMine)
        {
            _mydatatosend[0] = GetComponent<PhotonView>().ViewID as object;
            _mydatatosend[1] = GetJsonFolderData() as object;
            Invoke(nameof(CallRpcInvoke), 1.2f);
            //CallRpcInvoke();
        }
        if (!this.GetComponent<PhotonView>().IsMine && !this.gameObject.GetComponent<Speaker>())
        {
            this.gameObject.AddComponent<Speaker>();
        }
    }

    void CallRpcInvoke()
    {
        GetComponent<PhotonView>().RPC(nameof(CheckRpc), RpcTarget.AllBuffered, _mydatatosend as object);

    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Launcher.instance.playerobjects.Add(info.photonView.gameObject);
    }

    //Equipment otherEquip;

    [PunRPC]
    void CheckRpc(object[] Datasend)
    {
        AvatarController otherPlayer;
        Debug.Log("Datasend ------------- "+Datasend.ToString());
        Debug.Log("Datasend1 ------------- " + Datasend[0].ToString());
        Debug.Log("Datasend2 ------------- " + Datasend[1].ToString());


        string SendingPlayerID = Datasend[0].ToString();
        OtherPlayerId = Datasend[0].ToString();
        SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
        _CharacterData = JsonUtility.FromJson<SavingCharacterDataClass>(Datasend[1].ToString());

        //for (int j = 0; j < Launcher.instance.playerobjects.Count; j++)
        //{
            if (GetComponent<PhotonView>().ViewID.ToString() == OtherPlayerId)
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

}
