using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GamificationComponentUIManager : MonoBehaviour
{
    private void OnEnable()
    {
        //subscribe Narration event
        Debug.Log("Subscribe Event");
        BuilderEventManager.OnNarrationCollisionEnter += EnableNarrationUI;
        BuilderEventManager.OnNarrationCollisionExit += DisableNarrationUI;
        BuilderEventManager.OnRandomCollisionEnter += EnableRandomNumberUI;
        BuilderEventManager.OnRandomCollisionExit += DisableRandomNumberUI;
        BuilderEventManager.OnTimerLimitTriggerEnter += EnableTimeLimitUI;
        BuilderEventManager.OnTimerCountDownTriggerEnter += EnableTimerCountDownUI;
        BuilderEventManager.OnElapseTimeCounterTriggerEnter += EnableElapseTimeCounDownUI;
        BuilderEventManager.OnDisplayMessageCollisionEnter += EnableDisplayMessageUI;
        BuilderEventManager.OnHelpButtonCollisionEnter += EnableHelpButtonUI;
        BuilderEventManager.OnSituationChangerTriggerEnter += EnableSituationChangerUI;
    }
    private void OnDisable()
    {
        //unsubscribe Narration event
        Debug.Log("UnSubscribe Event");
        BuilderEventManager.OnNarrationCollisionEnter -= EnableNarrationUI;
        BuilderEventManager.OnNarrationCollisionExit -= DisableNarrationUI;
        BuilderEventManager.OnRandomCollisionEnter -= EnableRandomNumberUI;
        BuilderEventManager.OnRandomCollisionExit -= DisableRandomNumberUI;
        BuilderEventManager.OnTimerLimitTriggerEnter -= EnableTimeLimitUI;
        BuilderEventManager.OnTimerCountDownTriggerEnter -= EnableTimerCountDownUI;
        BuilderEventManager.OnElapseTimeCounterTriggerEnter -= EnableElapseTimeCounDownUI;
        BuilderEventManager.OnDisplayMessageCollisionEnter -= EnableDisplayMessageUI;
        BuilderEventManager.OnHelpButtonCollisionEnter -= EnableHelpButtonUI;
        BuilderEventManager.OnSituationChangerTriggerEnter -= EnableSituationChangerUI;
    }



    //Narration Comopnent 
    public GameObject narrationUIParent;
    public TextMeshProUGUI narrationTextUI;

    //Random Number Component
    public GameObject RandomNumberUIParent;
    public TextMeshProUGUI RandomNumberText;

    //Timer Limit Component
    public GameObject TimeLimitUIParent;
    public GameObject TimeLimitPrefab;
    public Transform TimeLimitParent;

    //Elapse Time Component
    public GameObject ElapseTimeUIParent;
    public TextMeshProUGUI ElapseTimerText;

    //Countdown Component
    public GameObject TimerCountDownUIParent;
    public TextMeshProUGUI TimerCountDownText;

    //Display Messages Component
    public GameObject DisplayMessageParentUI;
    public TextMeshProUGUI DisplayMessageText;
    public TextMeshProUGUI DisplayMessageTimeText;

    //Help Button Component
    public GameObject HelpButtonParentUI;
    public TextMeshProUGUI HelpButtonTitleText;
    public TextMeshProUGUI HelpText;

    //Situation Changer Component
    public GameObject SituationChangerParentUI;
    public TextMeshProUGUI SituationChangerTimeText;
    void EnableNarrationUI(string narrationText)
    {
        narrationUIParent.SetActive(true);
        narrationTextUI.text = narrationText;
    }
    void DisableNarrationUI()
    {
        narrationUIParent.SetActive(false);
    }

    public void EnableRandomNumberUI(float r)
    {
        RandomNumberUIParent.SetActive(true);
        RandomNumberText.text = "Generated Number On This Item : " + r.ToString();
    }
    public void DisableRandomNumberUI()
    {
        RandomNumberUIParent.SetActive(false);
    }

    //Time Limit Component
    public void EnableTimeLimitUI(string purpose, float time)
    {
        if (!TimeLimitUIParent.activeInHierarchy)
        {
            TimeLimitUIParent.SetActive(true);
        }
        GameObject g = Instantiate(TimeLimitPrefab, TimeLimitParent);
        g.GetComponent<TimeLimitUI>().time = time;
        g.GetComponent<TimeLimitUI>().Purpose = purpose;
    }

    public void DisableTimeLimitUI()
    {
        TimeLimitUIParent.SetActive(false);
    }

    public Coroutine TimerCountdownCoroutine;
    public void EnableTimerCountDownUI(int time, bool isRunning)
    {
        if (isRunning)
        {
            if (TimerCountdownCoroutine == null)
            {
                TimerCountdownCoroutine = StartCoroutine(IETimerCountDown(time, isRunning));
            }
            /*TimerCountDownUIParent.SetActive(true);
            TimerCountDownText.text = (time + 1).ToString();*/
        }
        else
        {
            DisableTimerCounDownUI();
            StopCoroutine(IETimerCountDown(time, isRunning));
        }
    }
    public IEnumerator IETimerCountDown(int time, bool isRunning)
    {
        while (time >= 0 && isRunning)
        {
            TimerCountDownUIParent.SetActive(true);
            TimerCountDownText.text = (time + 1).ToString();
            yield return new WaitForSeconds(1f);
            time--;
            TimerCountDownText.text = (time + 1).ToString();
        }
        TimeStatsCountdown._timeStop?.Invoke();
        DisableTimerCounDownUI();
    }
    public void DisableTimerCounDownUI()
    {
        TimerCountDownUIParent.SetActive(false);
    }

    public Coroutine ElapsedTimerCoroutine;
    public void EnableElapseTimeCounDownUI(float time, bool isRunning)
    {
        Debug.LogError("EnableElapseTimeCounDownUI" + time);
        if (isRunning)
        {
            ElapseTimeUIParent.SetActive(true);
            if (ElapsedTimerCoroutine == null)
            {
                ElapsedTimerCoroutine = StartCoroutine(IEElapsedTimer(time, isRunning));
            }
        }
        else
        {
            StopCoroutine(IEElapsedTimer(time, isRunning));
            DisableElapseTimeCounDownUI();
        }
    }
    public IEnumerator IEElapsedTimer(float time, bool isRunning)
    {
        while (time >= 0 && isRunning)
        {
            TimeSpan m_SecondsInToTimeFormate = TimeSpan.FromSeconds(time);
            ElapseTimerText.text = m_SecondsInToTimeFormate.ToString(@"mm\:ss");
            yield return new WaitForSeconds(1);
            time++;
        }
        DisableElapseTimeCounDownUI();
    }
    public void DisableElapseTimeCounDownUI()
    {
        ElapseTimeUIParent.SetActive(false);
    }

    public void EnableDisplayMessageUI(string DisplayMessage, float time)
    {
        StartCoroutine(IEEnableDisplayMessageUI(DisplayMessage, time));
    }
    public IEnumerator IEEnableDisplayMessageUI(string DisplayMessage, float time)
    {
        if (!DisplayMessageParentUI.activeInHierarchy)
        {
            DisplayMessageParentUI.SetActive(true);
            yield return new WaitForSeconds(.1f);
            DisplayMessageText.text = DisplayMessage;
        }
        while (time > 0)
        {
            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);
            DisplayMessageTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            //CanvasComponenetsManager._instance.timeLeft.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            time -= Time.deltaTime;
            yield return null;
        }
        DisplayMessageParentUI.SetActive(false);
        StopCoroutine(IEEnableDisplayMessageUI(DisplayMessage, time));
    }

    public void DisableDisplayMessageUI()
    {
        DisplayMessageParentUI.SetActive(false);
    }

    public void EnableHelpButtonUI(string helpButtonTitle, List<string> HelpTexts)
    {
        HelpButtonTitleText.text = helpButtonTitle;
        HelpText.text = "";
        if (HelpTexts.Count == 0)
        {
            HelpText.text = "Define Rules here !";
        }
        else
        {
            for (int i = 0; i < HelpTexts.Count; i++)
            {
                HelpText.text += "->  " + HelpTexts[i] + "\n" + "\n";
            }
        }
        HelpButtonParentUI.SetActive(true);
    }
    public void DisableHelpButtonUI()
    {
        HelpButtonParentUI.SetActive(false);
    }

    public Coroutine SituationChangerCoroutine;
    public void EnableSituationChangerUI(float timer)
    {
        if (timer > 0)
        {
            if (SituationChangerCoroutine == null)
            {
                SituationChangerCoroutine = StartCoroutine(IESituationChanger(timer));
            }
            /*SituationChangerTimeText.text = timer.ToString("00");
            SituationChangerParentUI.SetActive(true);*/
        }
        else
        {
            SituationChangerParentUI.SetActive(false);
        }
    }
    public IEnumerator IESituationChanger(float timer)
    {
        while (timer > 0)
        {
            SituationChangerTimeText.text = timer.ToString("00");
            SituationChangerParentUI.SetActive(true);
            yield return new WaitForSeconds(1f);
            timer--;
        }
        TimeStats._intensityChangerStop?.Invoke();
        SituationChangerParentUI.SetActive(false);
    }
}
