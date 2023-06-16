using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Models;
using Photon.Pun;

//[RequireComponent(typeof(Rigidbody))]
public class SituationChangerComponent : ItemComponent
{
    [SerializeField]
    private SituationChangerComponentData situationChangerComponentData;

    private bool isActivated = false;

    public Light[] _light;

    public float[] _lightsIntensity;

    public float timeCheck = 0f;

    bool running = false;

    GameObject textComponent;

    private TextMeshProUGUI m_TimeCounterText;

    const string m_TimerUIName = "SituationChangerText";

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        //if (transform.childCount > 0)
        //{
        //    if (transform.GetComponentInChildren<BoxCollider>())
        //    {
        //        Collider[] _childCollider = transform.GetComponentsInChildren<BoxCollider>();
        //        for (int i = 0; i < _childCollider.Length; i++)
        //        {
        //            _childCollider[i].isTrigger = true;
        //        }
        //    }
        //    if (transform.GetComponentInChildren<MeshCollider>())
        //    {
        //        MeshCollider[] _childCollider = transform.GetComponentsInChildren<MeshCollider>();
        //        for (int i = 0; i < _childCollider.Length; i++)
        //        {
        //            _childCollider[i].convex = true;
        //            _childCollider[i].isTrigger = true;
        //        }
        //    }
        //}
        _light = FindObjectsOfType<Light>();
        _lightsIntensity = new float[_light.Length];
        for (int i = 0; i < _light.Length; i++)
        {
            _lightsIntensity[i] = _light[i].intensity;
        }

        timeCheck = situationChangerComponentData.Timer;
    }
    public void Init(SituationChangerComponentData situationChangerComponentData)
    {
        this.situationChangerComponentData = situationChangerComponentData;

        isActivated = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Situation changer collision" + collision.gameObject.name);
        if (collision.gameObject.tag == "Player" || (collision.gameObject.tag == "PhotonLocalPlayer" && collision.gameObject.GetComponent<PhotonView>().IsMine))
        {
            timeCheck = situationChangerComponentData.Timer;
            //TimeStats._intensityChangerStop?.Invoke();
            TimeStats._intensityChanger?.Invoke(_light, _lightsIntensity, timeCheck);
        }
    }
}
