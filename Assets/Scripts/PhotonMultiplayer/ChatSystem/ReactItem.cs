﻿using Metaverse;
//using SuperStar.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;

public class ReactItem : MonoBehaviour
{
    #region PUBLIC_VAR
    public Image icon;
    public string iconUrl;
    public string _mainImage;
    public string ReactName;
    public GameObject highlighter;
    public GameObject ContentPanel;
    public Image StarImg;
    public TextMeshProUGUI reactionText;
    public int index;
    public Button button;
    #endregion

    #region PRIVATE_VAR
    #endregion

    #region UNITY_METHOD
    //IEnumerator Start()
    //{

    //}
    public void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            string data = PlayerPrefsUtility.GetEncryptedString(ConstantsGod.ANIMATION_DATA + i);
            if (!data.IsNullOrEmpty())
            {
                AnimationData d = JsonUtility.FromJson<AnimationData>(data);
                if (d.isEmote)
                {
                    if (d.animationURL == _mainImage)
                    {
                        StarImg.sprite = AvatarManager.Instance.FavouriteAnimationSprite;
                        return;
                    }
                    else
                    {
                        StarImg.sprite = AvatarManager.Instance.NormalAnimationSprite;
                    }
                }
            }
            else
                StarImg.sprite = AvatarManager.Instance.NormalAnimationSprite;
        }
    }

    void OnEnable()
    {
        Debug.Log("onenable call hua" + iconUrl);
        ContentPanel = transform.parent.gameObject;
        if (iconUrl != "" && icon.sprite.name == "buttonLoading")
        {
            StartCoroutine(GetTexture());
            //AssetCache.Instance.EnqueueOneResAndWait(iconUrl, iconUrl, (success) =>
            //{
            //    if (success)
            //    {
            //        AssetCache.Instance.LoadSpriteIntoImage(icon, iconUrl, changeAspectRatio: true);
            //        // CheckAndSetResolutionOfImage(imgFeed.sprite);
            //        //  isImageSuccessDownloadAndSave = true;
            //    }
            //    else
            //    {
            //        Debug.LogError("Download Failed");
            //    }
            //});
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ButtonClick);
        for (int i = 0; i < 10; i++)
        {
            string data = PlayerPrefsUtility.GetEncryptedString(ConstantsGod.ANIMATION_DATA + i);
            Debug.Log("URL:" + ConstantsGod.ANIMATION_DATA + i);
            if (!data.IsNullOrEmpty())
            {
                AnimationData d = JsonUtility.FromJson<AnimationData>(data);
                if (d.isEmote)
                {
                    if (d.animationURL == _mainImage)
                    {
                        StarImg.sprite = AvatarManager.Instance.FavouriteAnimationSprite;
                        return;
                    }
                    else
                    {
                        StarImg.sprite = AvatarManager.Instance.NormalAnimationSprite;
                    }
                }
            }
            else
                StarImg.sprite = AvatarManager.Instance.NormalAnimationSprite;
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void SetData(string url, string mainImage, int _index,string imageName)
    {
        index = _index;
        iconUrl = url;
        ReactName = imageName;
        _mainImage = mainImage;
        //reactionText.text = ReactName;    Riken
        if (this.isActiveAndEnabled)
        {
            //AssetCache.Instance.EnqueueOneResAndWait(iconUrl, iconUrl, (success) =>
            //{
            //    if (success)
            //    {
            //        AssetCache.Instance.LoadSpriteIntoImage(icon, iconUrl, changeAspectRatio: true);
            //        // CheckAndSetResolutionOfImage(imgFeed.sprite);
            //        //  isImageSuccessDownloadAndSave = true;
            //    }
            //    else
            //    {
            //        Debug.LogError("Download Failed");
            //    }
            //});

              StartCoroutine(GetTexture());
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private void ButtonClick()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        Resources.UnloadUnusedAssets();
        Debug.Log("Reaction URL : " + _mainImage);

        for (int i = 0; i < ContentPanel.transform.childCount; i++)
        {
            ContentPanel.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        highlighter.SetActive(true);
        if (GamePlayButtonEvents.inst != null && GamePlayButtonEvents.inst.selectionPanelOpen)
        {
            OnSaveDataOnButton();
            return;
        }
        PlayerPrefs.SetString(ConstantsGod.ReactionThumb, _mainImage);
        ArrowManager.OnInvokeReactionButtonClickEvent(PlayerPrefs.GetString(ConstantsGod.ReactionThumb));
    }

    private void OnSaveDataOnButton()
    {
        AnimationData animData = new AnimationData();
        animData.animationName = _mainImage;
        animData.animationURL = _mainImage;
        animData.thumbURL = _mainImage;
        animData.bgColor = GetComponent<Image>().color;
        animData.isEmote = true;
        GamePlayButtonEvents.inst.OnAnimationSelect(animData);
    }
    #endregion

    #region COROUTINE

    IEnumerator GetTexture()
    {
        Debug.Log("Enter");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(iconUrl);
        www.SendWebRequest();
        while (!www.isDone)
        {
            yield return null;
        }
        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));
            icon.sprite = sprite;
        }
    }

    #endregion

    #region DATA
    #endregion

}
