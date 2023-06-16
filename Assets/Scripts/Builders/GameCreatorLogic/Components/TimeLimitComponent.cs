using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Models;
using Photon.Pun;

//[RequireComponent(typeof(Rigidbody))]
public class TimeLimitComponent : ItemComponent
{

    // public static Action TimerCompleted;


    [Tooltip("Set Total Time")]
    [SerializeField]
    private float m_TotalTime;

    [SerializeField]
    private bool isTimerStarted = false;

    public bool IsTimeLimitActive;

    private Text m_TimeCounterText;

    public string TimeLimitSTR;

    private const string m_TimerUIName = "TimeLimitText";

    private const int m_EndTime = 0;

    private TimeSpan m_SecondsInToTimeFormate;

    private const string m_TimeFormate = @"mm\:ss";

    [SerializeField]
    private bool isActivated = false;
    [SerializeField]
    private TimeLimitComponentData timeLimitComponentData;

    public Action StartTimerEvent;
    public Action EndTimerEvent;

    BoxCollider boxCollider;

    bool timerUploaded = false;
    private bool coroutineIsRunning = false;

    private void OnEnable()
    {
        StartTimerEvent += StartTimer;
        EndTimerEvent += StopTimer;
    }
    private void OnDisable()
    {
        StartTimerEvent -= StartTimer;
        EndTimerEvent -= StopTimer;
    }



    private void Start()
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        //GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        timerUploaded = false;
        m_TotalTime = timeLimitComponentData.TimerLimit;
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
        /*GameObject textComponent = null;
        if (!timerUploaded)
        {
            Transform canvas = GameObject.Find("DifferentTextParent").transform;
            textComponent = (GameObject)Instantiate(Resources.Load(m_TimerUIName, typeof(GameObject)), canvas);
            textComponent.gameObject.name = m_TimerUIName;
            m_TimeCounterText = textComponent.GetComponent<Text>();
            timerUploaded = true;
        }
        if (m_TimeCounterText == null)
        {
            m_TimeCounterText = GameObject.Find(m_TimerUIName).GetComponent<Text>();
        m_TimeCounterText.gameObject.SetActive(true);
        m_TimeCounterText.transform.SetSiblingIndex(1);
        }*/
        Debug.Log("Start Timer Time Limit");
        isTimerStarted = true;
        StartCoroutine(Timer());
        if (!IsTimeLimitActive)
        {
            IsTimeLimitActive = true;
            BuilderEventManager.OnTimerLimitTriggerEnter?.Invoke(timeLimitComponentData.Purpose, m_TotalTime);
        }
    }


    public void StopTimer()
    {
        isTimerStarted = false;
        coroutineIsRunning = false;
        IsTimeLimitActive = false;
        Start();
        //m_TimeCounterText.color = Color.black;
        //m_TimeCounterText.gameObject.SetActive(false);
    }

    IEnumerator Timer()
    {
        if (isActivated)
        {
            if (!isTimerStarted || coroutineIsRunning) { yield break; }
            coroutineIsRunning = true;
            while (!m_TotalTime.Equals(m_EndTime) && isTimerStarted)
            {
                m_TotalTime -= Time.deltaTime;
                m_TotalTime = (Mathf.Clamp(m_TotalTime, 0, Mathf.Infinity));
                m_SecondsInToTimeFormate = TimeSpan.FromSeconds(m_TotalTime);
                //m_TimeCounterText.text = timeLimitComponentData.Purpose + ": " + m_SecondsInToTimeFormate.ToString(m_TimeFormate);
                yield return null;
            }
            StopTimer();
        }
    }


    public void Init(TimeLimitComponentData timeLimitComponentData)
    {
        this.timeLimitComponentData = timeLimitComponentData;

        isActivated = true;
    }


    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("Timer Limit Trigger: " + _other.gameObject.name);
        if (isActivated)
        {
            if (_other.CompareTag("Player") || (_other.gameObject.tag == "PhotonLocalPlayer" && _other.gameObject.GetComponent<PhotonView>().IsMine))
            {
                StartTimerEvent?.Invoke();
            }
        }
    }
}