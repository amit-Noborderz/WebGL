using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Photon.Pun;

//[RequireComponent(typeof(Rigidbody))]
public class DisplayMessagesComponent : MonoBehaviour
{
    [SerializeField]
    private DisplayMessageComponentData displayMessageComponentData;
    public static IEnumerator currentCoroutine;
    public bool isCoroutineRunning = false;

    public void Init(DisplayMessageComponentData displayMessageComponentData)
    {
        //CanvasComponenetsManager._instance.displayMessagesComponent = this;
        this.displayMessageComponentData = displayMessageComponentData;
        //GetComponent<Rigidbody>().isKinematic = true;
        //GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    //oncollisionEnter to OnTriggerEnter
    /*private void OnCollisionEnter(Collision _other)
    {
        Debug.Log("Display Message Collision Enter " + _other.gameObject.name);
        if (_other.gameObject.CompareTag("Player") || (_other.gameObject.tag == "PhotonLocalPlayer" && _other.gameObject.GetComponent<PhotonView>().IsMine))
        {
            //if (CanvasComponenetsManager._instance.displayMessagesComponent != this && CanvasComponenetsManager._instance.displayMessagesComponent)
            //    CanvasComponenetsManager._instance.displayMessagesComponent.isCoroutineRunning = false;

            //CanvasComponenetsManager._instance.displayMessagesComponent = this;
            isCoroutineRunning = true;
            BuilderEventManager.OnDisplayMessageCollisionEnter?.Invoke(displayMessageComponentData.titleDisplayMessage, displayMessageComponentData.messageDisplayTimeData + 1);
            //StartCoroutine(SetDisplayMessageNarration());
        }
    }*/
    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("Display Message Collision Enter " + _other.gameObject.name);
        if (_other.gameObject.CompareTag("Player") || (_other.gameObject.tag == "PhotonLocalPlayer" && _other.gameObject.GetComponent<PhotonView>().IsMine))
        {
            //if (CanvasComponenetsManager._instance.displayMessagesComponent != this && CanvasComponenetsManager._instance.displayMessagesComponent)
            //    CanvasComponenetsManager._instance.displayMessagesComponent.isCoroutineRunning = false;

            //CanvasComponenetsManager._instance.displayMessagesComponent = this;
            isCoroutineRunning = true;
            BuilderEventManager.OnDisplayMessageCollisionEnter?.Invoke(displayMessageComponentData.titleDisplayMessage, displayMessageComponentData.messageDisplayTimeData + 1);
            //StartCoroutine(SetDisplayMessageNarration());
        }
    }

    IEnumerator SetDisplayMessageNarration()
    {
        //if (CanvasComponenetsManager._instance != null)
        //{
        //    CanvasComponenetsManager._instance.displayMessageParentReference.SetActive(true);
        //    CanvasComponenetsManager._instance.displayMessageTitle.text = displayMessageComponentData.titleDisplayMessage;
        //}

        currentCoroutine = ShowCanvas();
        yield return currentCoroutine;
        //CanvasComponenetsManager._instance.displayMessageParentReference.SetActive(false);

    }

    public IEnumerator ShowCanvas()
    {
        float time = displayMessageComponentData.messageDisplayTimeData + 1;
        while (time > 0 && isCoroutineRunning)
        {
            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);
            //CanvasComponenetsManager._instance.timeLeft.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            time -= Time.deltaTime;
            yield return null;
        }
        if (isCoroutineRunning)
        {
            //CanvasComponenetsManager._instance.OnDisplayMessagesCanvasClosed();
        }
    }

}
