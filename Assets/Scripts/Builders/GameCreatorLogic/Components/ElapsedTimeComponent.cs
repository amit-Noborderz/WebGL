using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Threading.Tasks;
using Models;
using Photon.Pun;

//[RequireComponent(typeof(Rigidbody))]
public class ElapsedTimeComponent : ItemComponent
{
    [Tooltip("Set Total Time")]
    [SerializeField]
    private float m_TotalTime;
    private bool isActivated = false;
    [SerializeField]
    private ElapsedTimeComponentData elapsedTimeComponentData;
    //private void Start()
    //{
    //    GetComponent<Rigidbody>().isKinematic = true;
    //    GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
    //    if (transform.childCount > 0)
    //    {
    //        Collider[] _childCollider = transform.GetComponentsInChildren<Collider>();
    //        for (int i = 0; i < _childCollider.Length; i++)
    //        {
    //            _childCollider[i].isTrigger = true;
    //        }
    //    }
    //}


    public void Init(ElapsedTimeComponentData elapsedTimeComponentData)
    {
        Debug.Log("Elapsed Time INit");
        this.elapsedTimeComponentData = elapsedTimeComponentData;

        isActivated = true;
    }


    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("Elapsed Time Trigger " + _other.gameObject.name + "Activated: " + isActivated + "  " + elapsedTimeComponentData.IsStart + " IsEnd " + elapsedTimeComponentData.IsEnd);
        if (isActivated && elapsedTimeComponentData.IsStart)
        {
            if (_other.CompareTag("Player") || (_other.gameObject.tag == "PhotonLocalPlayer" && _other.gameObject.GetComponent<PhotonView>().IsMine))
            {
                TimeStats._timeStop?.Invoke();
                TimeStats._timeStart?.Invoke();
                //    StartTimer();
                gameObject.SetActive(false);
            }
        }
        if (isActivated && elapsedTimeComponentData.IsEnd)
        {
            if (_other.CompareTag("Player") || (_other.gameObject.tag == "PhotonLocalPlayer" && _other.gameObject.GetComponent<PhotonView>().IsMine))
            {
                Debug.Log("Elapse Time ");
                TimeStats._timeStop?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
