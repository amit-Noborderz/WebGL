using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;
using System;

public class GetServerResponse : MonoBehaviour
{
   // public TMPro.TextMeshProUGUI t1;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //using (UnityWebRequest www = UnityWebRequest.Get("https://api-test.xana.net/item/environment/1/10"))
        //{
        //    www.SetRequestHeader("Authorization", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MTYwLCJpYXQiOjE2NzMyNTg2NTMsImV4cCI6MTY3MzQzMTQ1M30.X5W-vUWBaR9AfZ-f4YklmVrsruO_br_ypRoRnYUUuK0");
        //    www.SendWebRequest();
        //    while (!www.isDone)
        //        yield return null;
        //    Debug.LogError(www.downloadHandler.text);
        //    t1.text = www.downloadHandler.text;
        //    if ((www.result == UnityWebRequest.Result.ConnectionError) || (www.result == UnityWebRequest.Result.ProtocolError))
        //    {
        //        //callback(false);
        //    }
        //    else
        //    {
        //        //_WorldInfo = JsonUtility.FromJson<WorldsInfo>(www.downloadHandler.text);
        //        //callback(true);
        //    }
        //}

        //LoadAddressable();
       
        yield return null;

    }

    void LoadAddressable()
    {
        AsyncOperationHandle<GameObject> asyncOperationHandle= Addressables.LoadAssetAsync<GameObject>("girlc9shirt");
        asyncOperationHandle.Completed += AsyncOperationHandle_Completed;
    }

    private void AsyncOperationHandle_Completed(AsyncOperationHandle<GameObject> obj)
    {
        GameObject t1 = Instantiate(obj.Result);

        this.gameObject.SetActive(false);
    }



    
}
