using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Models;
using Photon.Pun;

//[RequireComponent(typeof(Rigidbody))]
public class RandomNumberComponent : ItemComponent
{
    float _minNumber = 0, _maxNumber = 0, GeneratedNumber = 0;

    [SerializeField]
    private RandomNumberComponentData randomNumberComponentData;

    private bool isActivated = false;

    private TextMeshProUGUI m_TimeCounterText;

    private string m_TimerUIName = "RandomNumberText";


    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        //GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        _minNumber = randomNumberComponentData.minNumber;
        _maxNumber = randomNumberComponentData.maxNumber;
        GenerateNumber();
    }

    void GenerateNumber()
    {
        GeneratedNumber = (int)UnityEngine.Random.Range(_minNumber, _maxNumber);
        //LoadUI();
    }

    public void LoadUI()
    {
        Transform canvas = FindObjectOfType<Canvas>().transform;
        GameObject textComponent = (GameObject)Instantiate(Resources.Load(m_TimerUIName, typeof(GameObject)), canvas);
        textComponent.gameObject.name = m_TimerUIName;
        m_TimeCounterText = textComponent.GetComponent<TextMeshProUGUI>();
        m_TimeCounterText.gameObject.SetActive(false);
        m_TimeCounterText.transform.SetSiblingIndex(1);
    }

    public void Init(RandomNumberComponentData randomNumberComponentData)
    {
        this.randomNumberComponentData = randomNumberComponentData;

        isActivated = true;
    }

    //onCollsion Enter to ontrigger enter
    /*private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Random Number Collision Enter: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player") || (other.gameObject.tag == "PhotonLocalPlayer" && other.gameObject.GetComponent<PhotonView>().IsMine))
        {
            BuilderEventManager.OnRandomCollisionEnter?.Invoke(GeneratedNumber);
            *//*m_TimeCounterText.gameObject.SetActive(true);
            m_TimeCounterText.text = "Generated Number On This Item : " + GeneratedNumber;*//*
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Random Number Collision Enter: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player") || (other.gameObject.tag == "PhotonLocalPlayer" && other.gameObject.GetComponent<PhotonView>().IsMine))
        {
            BuilderEventManager.OnRandomCollisionEnter?.Invoke(GeneratedNumber);
            /*m_TimeCounterText.gameObject.SetActive(true);
            m_TimeCounterText.text = "Generated Number On This Item : " + GeneratedNumber;*/
        }
    }

    //onCollsion Exit to ontrigger exit
    /*private void OnCollisionExit(Collision other)
    {
        Debug.Log("Random Number Collision Exit: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player") || (other.gameObject.tag == "PhotonLocalPlayer" && other.gameObject.GetComponent<PhotonView>().IsMine))
        {
            BuilderEventManager.OnRandomCollisionExit?.Invoke();
            *//*m_TimeCounterText.gameObject.SetActive(false);
            m_TimeCounterText.text = "";*//*

        }
    }*/
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Random Number Collision Exit: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player") || (other.gameObject.tag == "PhotonLocalPlayer" && other.gameObject.GetComponent<PhotonView>().IsMine))
        {
            BuilderEventManager.OnRandomCollisionExit?.Invoke();
            /*m_TimeCounterText.gameObject.SetActive(false);
            m_TimeCounterText.text = "";*/

        }
    }
}
