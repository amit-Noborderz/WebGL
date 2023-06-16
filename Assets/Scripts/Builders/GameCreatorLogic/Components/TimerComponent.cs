using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Models;
using Photon.Pun;

/// <summary>
/// this class public members is accecebile from ervery where in the project just 
/// by Timer._instance.pulic members
/// </summary>
/// 

//[RequireComponent(typeof(Rigidbody))]
public class TimerComponent : ItemComponent
{

    /// <summary>
    ///  this event will call all its subsciber methods on timer completion
    /// </summary>
    public static Action TimerCompleted;


    [Tooltip("Set Total Time")]
    [SerializeField]
    private float m_TotalTime;

    /// <summary>
    /// TimerUi object
    /// </summary>
   // private TimerComponentUI object_TimerUI;

    /// <summary>
    /// decide where timer is started or stoped
    /// </summary>
    private bool isTimerStarted = false;

    /// <summary>
    ///  timer counter Text
    /// </summary>
    private Text m_TimeCounterText;

    /// <summary>
    /// Prefab Name of the Timer ui which is in resources folder
    /// </summary>
    private const string m_TimerUIName = "TimerText";
    /// <summary>
    /// end time for the timer
    /// </summary>
    private const int m_EndTime = 0;

    /// <summary>
    /// convert seconds in to time formate 
    /// </summary>
    private TimeSpan m_SecondsInToTimeFormate;
    private const string m_TimeFormate = @"mm\:ss";

    public string ElapseTimeSTR;

    private bool isActivated = false;

    private TimerComponentData timerComponentData;

    public static Action StartTriggerEvent;
    public static Action EndTriggerEvent;

    BoxCollider boxCollider;
    //  Collider _childCollider;
    static bool timerUploaded = false;

    bool coroutineRunning = false;
    private void OnEnable()
    {

        StartTriggerEvent += StartTimer;
        EndTriggerEvent += StopTimer;

    }
    private void OnDisable()
    {
        StartTriggerEvent -= StartTimer;
        EndTriggerEvent -= StopTimer;
    }



    private void Start()
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        timerUploaded = false;
        m_TotalTime = timerComponentData.Timer;
        //boxCollider = GetComponent<BoxCollider>();
        //boxCollider.isTrigger = true;
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
    }
    public void StartTimer()
    {
        GameObject textComponent = null;
        if (!timerUploaded)
        {
            Transform canvas = GameObject.FindObjectOfType<Canvas>().transform;
            textComponent = (GameObject)Instantiate(Resources.Load(m_TimerUIName, typeof(GameObject)), canvas);
            textComponent.gameObject.name = m_TimerUIName;
            m_TimeCounterText = textComponent.GetComponent<Text>();
            timerUploaded = true;
        }
        if (m_TimeCounterText == null)
        {
            m_TimeCounterText = GameObject.Find("TimerText").GetComponent<Text>();
        }
        m_TimeCounterText.transform.SetSiblingIndex(1);
        isTimerStarted = true;

        StartCoroutine(Timer());
    }


    /// <summary>
    /// Stop Timer
    /// </summary>
    public void StopTimer()
    {
        StopCoroutine(Timer());
        isTimerStarted = false;
        coroutineRunning = false;

    }



    IEnumerator Timer()
    {
        if (!timerComponentData.IsStart || coroutineRunning) { yield break; }
        if (isActivated)
        {
            if (!isTimerStarted) yield break;
            coroutineRunning = true;
            m_TimeCounterText.gameObject.SetActive(true);
            while (!m_TotalTime.Equals(m_EndTime) && isTimerStarted)
            {
                m_TotalTime -= Time.deltaTime;
                m_TotalTime = (Mathf.Clamp(m_TotalTime, 0, Mathf.Infinity));
                m_SecondsInToTimeFormate = TimeSpan.FromSeconds(m_TotalTime);
                ElapseTimeSTR = m_TimeCounterText.text = m_SecondsInToTimeFormate.ToString(m_TimeFormate);
                yield return null;
            }
            if (m_TotalTime.Equals(m_EndTime)) { m_TimeCounterText.gameObject.SetActive(false); }
            //  m_TimeCounterText.gameObject.SetActive(false);
            //StopTimer();
            //      TimerCompleted?.Invoke();
        }
    }

    public void Init(TimerComponentData timerComponentData)
    {
        this.timerComponentData = timerComponentData;

        isActivated = true;
    }


    private void OnTriggerEnter(Collider _other)
    {
        if (isActivated && timerComponentData.IsStart)
        {
            if (_other.CompareTag("Player") || (_other.gameObject.tag == "PhotonLocalPlayer" && _other.gameObject.GetComponent<PhotonView>().IsMine))
            {
                StartTriggerEvent?.Invoke();
            }
        }
        if (isActivated && timerComponentData.IsEnd)
        {
            if (_other.CompareTag("Player") || (_other.gameObject.tag == "PhotonLocalPlayer" && _other.gameObject.GetComponent<PhotonView>().IsMine))
            {
                EndTriggerEvent?.Invoke();
            }
        }
    }

}// End of class



