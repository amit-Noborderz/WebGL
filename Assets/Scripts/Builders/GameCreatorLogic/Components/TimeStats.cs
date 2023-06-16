using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeStats : MonoBehaviour
{
    public static Action _timeStart;
    public static Action _timeStop;

    public static Action<Light[], float[], float> _intensityChanger;
    public static Action _intensityChangerStop;

    private float m_TotalTime;
    public bool IsElapsedTimeActive;
    public bool IsSituationChangerActive;

    private static bool _stopTimer = false, canRun = false;

    public TextMeshProUGUI m_TimeCounterText;

    IEnumerator coroutine, dimmerCoroutine;

    private TimeSpan m_SecondsInToTimeFormate;
    public Light[] lights;
    public float[] Intensity;
    private const string m_TimeFormat = @"mm\:ss";

    private void OnEnable()
    {
        _timeStart += StartTimer;
        _timeStop += StopTimer;
        _intensityChanger += SituationStarter;
        _intensityChangerStop += SituationStoper;
    }
    private void OnDisable()
    {
        _timeStart -= StartTimer;
        _timeStop -= StopTimer;
        _intensityChanger -= SituationStarter;
        _intensityChangerStop -= SituationStoper;
    }

    private void Start()
    {
        _stopTimer = true;
        coroutine = Timer();
    }

    public void StartTimer()
    {
        if (!IsElapsedTimeActive)
        {
            IsElapsedTimeActive = true;
            m_TotalTime = 0;
            _stopTimer = false;
            //m_TimeCounterText.transform.GetChild(0).gameObject.SetActive(true);
            //StartCoroutine(coroutine);
            BuilderEventManager.OnElapseTimeCounterTriggerEnter?.Invoke(m_TotalTime, true);
        }
    }


    public void StopTimer()
    {
        _stopTimer = true;
        IsElapsedTimeActive = false;
        //m_TimeCounterText.transform.GetChild(0).gameObject.SetActive(false);
        BuilderEventManager.OnElapseTimeCounterTriggerEnter?.Invoke(m_TotalTime, false);
        StopCoroutine(coroutine);
    }

    public void SituationStarter(Light[] _lights, float[] _intensities, float _value)
    {
        if (!IsSituationChangerActive)
        {
            lights = _lights;
            Intensity = _intensities;
            IsSituationChangerActive = true;
            dimmerCoroutine = DimLights(_lights, _intensities, _value);
            canRun = true;
            for (int i = 0; i < _lights.Length; i++)
            {
                _lights[i].intensity = 0.33f;
            }
            BuilderEventManager.OnSituationChangerTriggerEnter?.Invoke(_value);
            //StartCoroutine(dimmerCoroutine);
        }
    }
    public void SituationStoper()
    {
        canRun = false;
        IsSituationChangerActive = false;
        BackToNormalSituation();
        if (dimmerCoroutine != null)
        { StopCoroutine(dimmerCoroutine); }
    }


    IEnumerator Timer()
    {
        while (!_stopTimer)
        {
            m_TotalTime += Time.deltaTime;
            m_SecondsInToTimeFormate = TimeSpan.FromSeconds(m_TotalTime);
            //m_TimeCounterText.text = m_SecondsInToTimeFormate.ToString(m_TimeFormat);
            //BuilderEventManager.OnElapseTimeCounterTriggerEnter?.Invoke(m_TotalTime, true);
            yield return null;
        }
    }
    public void BackToNormalSituation()
    {
        if (lights.Length != 0)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].intensity = Intensity[i];
            }
        }
    }
    IEnumerator DimLights(Light[] _light, float[] _lightsIntensity, float timeCheck)
    {
        lights = _light;
        Intensity = _lightsIntensity;
        //m_TimeCounterText.gameObject.SetActive(true);
        //m_TimeCounterText.transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log("DimLights Run : ");
        for (int i = 0; i < _light.Length; i++)
        {
            _light[i].intensity = 0.33f;
        }
        while (!timeCheck.Equals(0) && canRun)
        {
            timeCheck -= Time.deltaTime;
            timeCheck = Mathf.Clamp(timeCheck, 0, Mathf.Infinity);
            //m_TimeCounterText.text = timeCheck.ToString("00");
            //BuilderEventManager.OnSituationChangerTriggerEnter?.Invoke(timeCheck);
            yield return null;

        }
        for (int i = 0; i < _light.Length; i++)
        {
            _light[i].intensity = _lightsIntensity[i];
        }

        /*m_TimeCounterText.text="";
        m_TimeCounterText.transform.GetChild(0).gameObject.SetActive(false);*/
        //    running = false;
    }
}
