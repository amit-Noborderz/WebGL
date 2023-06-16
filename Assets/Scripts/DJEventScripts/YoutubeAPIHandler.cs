using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal;
using WebSocketSharp;

public class YoutubeAPIHandler : MonoBehaviour
{

    private StreamResponse _response;

    private int DataIndex = 4;
    public StreamData Data;
    bool _urlDataInitialized = false;

    public IEnumerator GetStream()
    {
           yield return null;

        if (GameObject.FindGameObjectWithTag("MainCamera") != null)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;

        }

        //if (XanaEventDetails.eventDetails.DataIsInitialized)
        //{
        //    if (!XanaEventDetails.eventDetails.youtubeUrl.Equals(null))
        //    {
        //        Data = new StreamData(XanaEventDetails.eventDetails.youtubeUrl, false, true);
        //        _urlDataInitialized = true;
        //    }
        //    else
        //    {
        //        Data = null;
        //    }
        //    //XanaEventDetails.eventDetails.DataIsInitialized = false;
        //}
        //else
        //{
        //    using (UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.YOUTUBEVIDEOBYSCENE + XanaConstants.xanaConstants.EnviornmentName))
        //    {
        //        www.timeout = 10;

        //        www.SendWebRequest();

        //        while (!www.isDone)
        //        {
        //            yield return null;
        //        }
        //        if (www.isHttpError || www.isNetworkError)
        //        {
        //            _response = null;
        //            Debug.LogError("Youtube API returned no result");
        //        }
        //        else
        //        {
        //            _response = Gods.DeserializeJSON<StreamResponse>(www.downloadHandler.text.Trim());
        //            if (_response != null)
        //            {
        //                string incominglink = _response.data.link;
        //                if (!string.IsNullOrEmpty(incominglink))
        //                {
        //                    Data = new StreamData(incominglink, _response.data.isLive, _response.data.isPlaying);
        //                    _urlDataInitialized = true;
        //                    // print("Stage 3 video link:" + Data);
        //                }
        //                else
        //                {
        //                    Debug.Log("No Link Found Turning off player");
        //                    Data = null;
        //                }
        //            }

        //        }
        //    }
        //}







        print("===================Get Stream" + _urlDataInitialized);
        WWWForm form = new WWWForm();

        form.AddField("token", "piyush55");
        //if (!_urlDataInitialized)
        //{
            if (XanaConstants.xanaConstants.EnviornmentName.Contains("DJ Event"))
            {
                //if (XanaEventDetails.eventDetails.DataIsInitialized)
                //{
                //    if (!XanaEventDetails.eventDetails.youtubeUrl.Equals(null))
                //    {
                //        print("============Setting Youtube Link Data" + XanaEventDetails.eventDetails.youtubeUrl);
                //        Data = new StreamData(XanaEventDetails.eventDetails.youtubeUrl, false, true);
                //        _urlDataInitialized = true;
                //    }
                //    else
                //    {
                //        print("================No youtube link found");
                //        Data = null;
                //    }
                //    //XanaEventDetails.eventDetails.DataIsInitialized = false;
                //}
                //else
                //{
                    using (UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.SHARELINKS))
                    {

                        www.timeout = 10;

                        yield return www.SendWebRequest();

                        while (!www.isDone)
                        {
                            yield return null;
                        }
                        if (www.isHttpError || www.isNetworkError)
                        {
                            _response = null;
                            Data = null;
                            Debug.LogError("Youtube API returned no result");
                        }
                        else
                        {
                            _response = JsonUtility.FromJson<StreamResponse>(www.downloadHandler.text);
                            if (_response != null)
                            {
                                string incominglink = _response.data.link;
                                if (!string.IsNullOrEmpty(incominglink))
                                {
                                    Data = new StreamData(incominglink, _response.data.isLive, _response.data.isPlaying);
                                    _urlDataInitialized = true;
                                }
                                else
                                {
                                    Debug.Log("No Link Found Turning off player");
                                    Data = null;
                                }
                            }

                        }
                    }
                //}
            }
            else if (XanaConstants.xanaConstants.EnviornmentName.Contains("XANA Festival Stage"))
            {
                //if (XanaEventDetails.eventDetails.DataIsInitialized)
                //{
                //    if (!XanaEventDetails.eventDetails.youtubeUrl.Equals(null))
                //    {
                //        print("============Setting Youtube Link Data" + XanaEventDetails.eventDetails.youtubeUrl);
                //        Data = new StreamData(XanaEventDetails.eventDetails.youtubeUrl, false, true);
                //        _urlDataInitialized = true;
                //    }
                //    else
                //    {
                //        print("================No youtube link found");
                //        Data = null;
                //    }
                //    //XanaEventDetails.eventDetails.DataIsInitialized = false;
                //}
                //else
                //{
                    print("============Setting WWW data");
                    using (UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.SHAREDEMOS))
                    {
                        www.timeout = 10;

                        yield return www.SendWebRequest();

                        while (!www.isDone)
                        {
                            yield return null;
                        }
                        if (www.isHttpError || www.isNetworkError)
                        {
                            _response = null;
                            Data = null;
                            Debug.LogError("Youtube API returned no result");
                        }
                        else
                        {
                            _response = JsonUtility.FromJson<StreamResponse>(www.downloadHandler.text);
                            if (_response != null)
                            {
                                string incominglink = _response.data.link;
                                if (!string.IsNullOrEmpty(incominglink))
                                {
                                    Data = new StreamData(incominglink, _response.data.isLive, _response.data.isPlaying);
                                    _urlDataInitialized = true;
                                    // print("Stage 3 video link:" + Data);
                                }
                                else
                                {
                                    Debug.Log("No Link Found Turning off player");
                                    Data = null;
                                }
                            }

                        }
                    }
                //}
            }
            else if (XanaConstants.xanaConstants.EnviornmentName.Contains("Xana Festival") || XanaConstants.xanaConstants.EnviornmentName.Contains("NFTDuel Tournament"))
            {
                if (GameObject.FindGameObjectWithTag("MainCamera") != null)
                {
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;

                }
                Debug.Log("call hua kya he");
                //if (XanaEventDetails.eventDetails.DataIsInitialized)
                //{
                //    if (!XanaEventDetails.eventDetails.youtubeUrl.Equals(null))
                //    {
                //        print("============Setting Youtube Link Data" + XanaEventDetails.eventDetails.youtubeUrl);
                //        Data = new StreamData(XanaEventDetails.eventDetails.youtubeUrl, false, true);
                //        _urlDataInitialized = true;
                //    }
                //    else
                //    {
                //        print("================No youtube link found");
                //        Data = null;
                //    }
                //    //XanaEventDetails.eventDetails.DataIsInitialized = false;
                //}
                //else
                //{
                    print("============Setting WWW data");
                    using (UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.YOUTUBEVIDEOBYSCENE + XanaConstants.xanaConstants.EnviornmentName))
                    {
                        www.timeout = 10;

                        yield return www.SendWebRequest();

                        while (!www.isDone)
                        {
                            yield return null;
                        }
                        if (www.isHttpError || www.isNetworkError)
                        {
                            _response = null;
                            Debug.LogError("Youtube API returned no result");
                        }
                        else
                        {
                            _response = JsonUtility.FromJson<StreamResponse>(www.downloadHandler.text);
                            if (_response != null)
                            {
                                string incominglink = _response.data.link;
                                if (!string.IsNullOrEmpty(incominglink))
                                {
                                    Data = new StreamData(incominglink, _response.data.isLive, _response.data.isPlaying);
                                    _urlDataInitialized = true;
                                    // print("Stage 3 video link:" + Data);
                                }
                                else
                                {
                                    Debug.Log("No Link Found Turning off player");
                                    Data = null;
                                }
                            }

                        }
                    }
                //}
            }
            else if (XanaConstants.xanaConstants.EnviornmentName.Contains("BreakingDown Arena"))
            {
                if (GameObject.FindGameObjectWithTag("MainCamera") != null)
                {
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;

                }
                Debug.Log("call hua kya he");
                //if (XanaEventDetails.eventDetails.DataIsInitialized)
                //{
                //    if (!XanaEventDetails.eventDetails.youtubeUrl.Equals(null))
                //    {
                //        print("============Setting Youtube Link Data" + XanaEventDetails.eventDetails.youtubeUrl);
                //        Data = new StreamData(XanaEventDetails.eventDetails.youtubeUrl, false, true);
                //        _urlDataInitialized = true;
                //    }
                //    else
                //    {
                //        print("================No youtube link found");
                //        Data = null;
                //    }
                //    //XanaEventDetails.eventDetails.DataIsInitialized = false;
                //}
                //else
                //{
                    print("============Setting WWW data");
                    using (UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.YOUTUBEVIDEOBYSCENE + XanaConstants.xanaConstants.EnviornmentName))
                    {
                        www.timeout = 10;

                        yield return www.SendWebRequest();

                        while (!www.isDone)
                        {
                            yield return null;
                        }
                        if (www.isHttpError || www.isNetworkError)
                        {
                            _response = null;
                            Debug.LogError("Youtube API returned no result");
                        }
                        else
                        {
                            Debug.LogError("You tube respns===" + www.downloadHandler.text);
                            _response =JsonUtility.FromJson<StreamResponse>(www.downloadHandler.text);
                            if (_response != null)
                            {
                                string incominglink = _response.data.link;
                                if (!string.IsNullOrEmpty(incominglink))
                                {
                                    Data = new StreamData(incominglink, _response.data.isLive, _response.data.isPlaying);
                                    _urlDataInitialized = true;
                                    // print("Stage 3 video link:" + Data);
                                }
                                else
                                {
                                    Debug.Log("No Link Found Turning off player");
                                    Data = null;
                                }
                            }

                        }
                    }
                //}
            }

        //}

    }
}

[System.Serializable]
public class StreamData
{
    public string URL;
    public bool IsLive;
    public bool isPlaying;

    public StreamData(string URL, bool isLive, bool isPlaying)
    {
        this.URL = URL;
        this.IsLive = isLive;
        this.isPlaying = isPlaying;
    }

}

[System.Serializable]
public class StreamResponse
{
    public bool success;
    public string msg;
    public IncomingData data;


}

[System.Serializable]
public class IncomingData
{
    public long id;
    public string link;
    public bool isLive;
    public object createdAt;
    public object updatedAt;
    public bool isPlaying;
}
                                                                                                                                                                                                                                                                                