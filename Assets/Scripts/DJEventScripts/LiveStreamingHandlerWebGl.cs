using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using RenderHeads.Media.AVProVideo;

public class LiveStreamingHandlerWebGl : MonoBehaviour
{

    public MediaPlayer mPlayer;
    public AudioSource videoPlayerAudioSource;
    private LiveResponse _response;
    public VideoAPIRes Data = null;
    private string PrevURL;
    // Start is called before the first frame update
    void Start()
    {
        if (SoundManager.Instance)
        {
            SoundManager.Instance.videoPlayerSource = videoPlayerAudioSource;
            SoundManager.Instance.livePlayerSource = mPlayer;
            SoundManagerSettings.soundManagerSettings.videoSource = videoPlayerAudioSource;
            SoundManagerSettings.soundManagerSettings.setNewSliderValues();
        }

        mPlayer.GetComponent<ApplyToMesh>().MeshRenderer.sharedMaterial.color = new Color32(57, 57, 57, 255);
        PrevURL = "xyz";
        StartCoroutine(CheckApiContinousForUrlUpdate());
    }

    public IEnumerator CheckApiContinousForUrlUpdate()
    {
        while (true)
        {
            StartCoroutine(SetStream());
            yield return new WaitForSeconds(5.0f);
        }
    }


    private IEnumerator SetStream()
    {
        yield return StartCoroutine(GetStream());

        while (Data == null)
        {
            yield return null;
        }

        if (!PrevURL.Equals(Data.URL) && Data.isPlaying)
        {
            PrevURL = Data.URL;
            SetLiveUrlInAvpro(Data.URL);
        }
        else if (!Data.isPlaying)
        {
            mPlayer.GetComponent<ApplyToMesh>().MeshRenderer.sharedMaterial.color = new Color32(57, 57, 57, 255);
            mPlayer.CloseMedia();
        }
        //else
        //{
        //    mPlayer.GetComponent<ApplyToMesh>().MeshRenderer.sharedMaterial.color = new Color32(255, 255, 255, 255);
        //}
    }



    public void SetLiveUrlInAvpro(string url)
    {
        MediaPathType check = MediaPathType.AbsolutePathOrURL;
        if (mPlayer.OpenMedia(check, url, true))
        {
            mPlayer.GetComponent<ApplyToMesh>().MeshRenderer.sharedMaterial.color = Color.white;
            if (!XanaConstants.xanaConstants.EnviornmentName.Contains("DJ Event"))
            {
                mPlayer.transform.localRotation = Quaternion.identity;
            }
           
        }
    }

    public IEnumerator GetStream()
    {
        yield return null;
        //if (XanaEventDetails.eventDetails.DataIsInitialized)
        //{
        //    if (!XanaEventDetails.eventDetails.youtubeUrl.Equals(null))
        //    {
        //        Data = new VideoAPIRes(XanaEventDetails.eventDetails.youtubeUrl, false, true);
        //    }
        //    else
        //    {
        //        Data = null;
        //    }
        //    //XanaEventDetails.eventDetails.DataIsInitialized = false;
        //}
        //else
        //{
        using (UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.LIVESTREAMINGWEBGL + XanaConstants.xanaConstants.EnviornmentName))
        {
            www.SendWebRequest();

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
                Debug.LogError("---" + www.downloadHandler.text);
                _response = JsonUtility.FromJson<LiveResponse>(www.downloadHandler.text);
                if (_response != null)
                {
                    string incominglink = _response.data.link;
                    if (!string.IsNullOrEmpty(incominglink))
                    {
                        Data = new VideoAPIRes(incominglink, _response.data.isLive, _response.data.isPlaying);
                    }
                    else
                    {
                        Debug.LogError("No Link Found Turning off player");
                        Data = null;
                    }
                }

            }
        }
        //}
    }



}
[System.Serializable]
public class VideoAPIRes
{
    public string URL;
    public bool IsLive;
    public bool isPlaying;

    public VideoAPIRes(string URL, bool isLive, bool isPlaying)
    {
        this.URL = URL;
        this.IsLive = isLive;
        this.isPlaying = isPlaying;
    }

}

[System.Serializable]
public class LiveResponse
{
    public bool success;
    public ApiData data;
    public string msg;


}
[System.Serializable]
public class ApiData
{
    public long id;
    public string link;
    public bool isLive;
    public bool isPlaying;
}
