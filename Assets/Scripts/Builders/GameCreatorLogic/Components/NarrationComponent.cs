using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Photon.Pun;

//[RequireComponent(typeof(Rigidbody))]
public class NarrationComponent : ItemComponent
{
    [SerializeField]
    private NarrationComponentData narrationComponentData;
    public static IEnumerator currentCoroutine;

    public bool isCoroutineRunning = false;
    int i = 0;

    public void Init(NarrationComponentData narrationComponentData)
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        //GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        this.narrationComponentData = narrationComponentData;
        if (narrationComponentData.onStart)
        {
            currentCoroutine = SetNarration();
            //CanvasComponenetsManager._instance.narrationComponent = this;
            isCoroutineRunning = true;
            i = 0;
            StartCoroutine(currentCoroutine);
        }

    }
    IEnumerator SetNarration()
    {
        Debug.Log("SetNaration");
        //if (CanvasComponenetsManager._instance != null)
        //{
        //    CanvasComponenetsManager._instance.narrationParentReference.SetActive(true);
        float timeBwNarrations = this.narrationComponentData.timeBwNarrations;
        while (i < narrationComponentData.narrationsData.Count && isCoroutineRunning)
        {
            //CanvasComponenetsManager._instance.narrationTextInformation.text = narrationComponentData.narrationsData[i].ToString();
            BuilderEventManager.OnNarrationCollisionEnter?.Invoke(narrationComponentData.narrationsData[i]);
            yield return new WaitForSeconds(timeBwNarrations);
            if (isCoroutineRunning) i++;
            else yield break;
        }


        //    CanvasComponenetsManager._instance.narrationParentReference.SetActive(false);
        //}
    }


    //OnCollisionenter to OnTriggerEnter

    /*private void OnCollisionEnter(Collision _other)
    {
        Debug.Log("Naration colliosion enter" + _other.gameObject.name);
        if (_other.gameObject.CompareTag("Player") || (_other.gameObject.tag == "PhotonLocalPlayer" && _other.gameObject.GetComponent<PhotonView>().IsMine))
        {
            if (!narrationComponentData.onStart)
            {
                //CanvasComponenetsManager._instance.narrationParentReference.SetActive(true);

                //if (CanvasComponenetsManager._instance.narrationComponent != this && CanvasComponenetsManager._instance.narrationComponent)
                //    CanvasComponenetsManager._instance.narrationComponent.isCoroutineRunning = false;
                currentCoroutine = SetNarration();
                //CanvasComponenetsManager._instance.narrationComponent = this;
                isCoroutineRunning = true;
                i = 0;
                StartCoroutine(currentCoroutine);
            }
            //StartTriggerEvent?.Invoke();
        }
    }*/
    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("Naration colliosion enter" + _other.gameObject.name);
        if (_other.gameObject.CompareTag("Player") || (_other.gameObject.tag == "PhotonLocalPlayer" && _other.gameObject.GetComponent<PhotonView>().IsMine))
        {
            if (!narrationComponentData.onStart)
            {
                //CanvasComponenetsManager._instance.narrationParentReference.SetActive(true);

                //if (CanvasComponenetsManager._instance.narrationComponent != this && CanvasComponenetsManager._instance.narrationComponent)
                //    CanvasComponenetsManager._instance.narrationComponent.isCoroutineRunning = false;


                currentCoroutine = SetNarration();
                //CanvasComponenetsManager._instance.narrationComponent = this;
                isCoroutineRunning = true;
                i = 0;
                StartCoroutine(currentCoroutine);
            }

            //StartTriggerEvent?.Invoke();
        }
    }

    //OnCollisionExit to OnTriggerExit
    /*private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Naration colliosion exit" + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player") || (collision.gameObject.tag == "PhotonLocalPlayer" && collision.gameObject.GetComponent<PhotonView>().IsMine))
        {
            //CanvasComponenetsManager._instance.narrationParentReference.SetActive(false);
            BuilderEventManager.OnNarrationCollisionExit?.Invoke();
            StopCoroutine(currentCoroutine);
        }
    }*/
    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("Naration colliosion exit" + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player") || (collision.gameObject.tag == "PhotonLocalPlayer" && collision.gameObject.GetComponent<PhotonView>().IsMine))
        {
            //CanvasComponenetsManager._instance.narrationParentReference.SetActive(false);
            BuilderEventManager.OnNarrationCollisionExit?.Invoke();
            StopCoroutine(currentCoroutine);
        }
    }
}
