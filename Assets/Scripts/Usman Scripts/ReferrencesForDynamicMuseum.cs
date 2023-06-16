using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferrencesForDynamicMuseum : MonoBehaviour
{
    public GameObject[] overlayPanels;
    public GameObject workingCanvas, PlayerParent, MainPlayerParent;
    public GameObject[] disableObjects;
    public static ReferrencesForDynamicMuseum instance;
    public Camera randerCamera;
    public List<GameObject> disableObjectsInMuseums;
    public GameObject onBtnUsername;
    public GameObject offBtnUsername;

    public GameObject m_34player;
    public PlayerControllerNew playerControllerNew;
    public GameObject minimap;
    public GameObject minimapSettingsBtn;
    public TMPro.TextMeshProUGUI totalCounter; // Counter to show total connected peoples.
    public GameObject ReferenceObject;
    public GameObject ReferenceObjectPotrait;
    public GameObject FirstPersonCam;
    public Button RotateBtn;

    // Start is called before the first frame update
    void Awake()
    {

        if (instance != null && instance != this)
        {
            if (instance.m_34player != null)
            {
                m_34player = instance.m_34player;
            }
        }

        instance = this;
        if (XanaConstants.xanaConstants.IsMuseum)
        {
            foreach (GameObject go in disableObjectsInMuseums)
            {
                go.SetActive(false);
            }
        }
        if (XanaConstants.xanaConstants.EnviornmentName.Contains("AfterParty") || XanaConstants.xanaConstants.IsMuseum)
        {
            if (XanaConstants.xanaConstants.EnviornmentName.Contains("J&J WORLD_5"))
            {
                if (XanaConstants.xanaConstants.minimap == 1)
                {
                    minimap.SetActive(true);
                }
                minimapSettingsBtn.SetActive(true);
            }
            else
            {
                minimap.SetActive(false);
                minimapSettingsBtn.SetActive(false);
            }
        }
        else
        {
            if (XanaConstants.xanaConstants.minimap == 1)
            {
                minimap.SetActive(true);
            }
            minimapSettingsBtn.SetActive(true);
        }
        playerControllerNew = MainPlayerParent.GetComponent<PlayerControllerNew>();
    }


    private void OnEnable()
    {
        if (instance != null && instance != this)
        {
            if (instance.totalCounter != null)
                totalCounter.text = totalCounter.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + XanaConstants.xanaConstants.userLimit;
        }

        instance = this;
        print("Waqas : Reference : Instance : " + instance);

        if (ReferenceObject.activeInHierarchy && m_34player != null)
        {
            Debug.Log("call hua texture Landscap");
            m_34player.GetComponent<MyBeachSelfieCam>().SelfieCapture_CamRender.SetActive(true);
            m_34player.GetComponent<MyBeachSelfieCam>().SelfieCapture_CamRenderPotraiat.SetActive(false);
        }

        if (ReferenceObjectPotrait.activeInHierarchy)
        {
            Debug.Log("call hua texture potriat");
            m_34player.GetComponent<MyBeachSelfieCam>().SelfieCapture_CamRender.SetActive(false);
            m_34player.GetComponent<MyBeachSelfieCam>().SelfieCapture_CamRenderPotraiat.SetActive(true);
        }

        if (XanaConstants.xanaConstants.EnviornmentName.Contains("AfterParty") || XanaConstants.xanaConstants.IsMuseum)
        {
            if (XanaConstants.xanaConstants.EnviornmentName.Contains("J&J WORLD_5"))
            {
                if (XanaConstants.xanaConstants.minimap == 1)
                    ReferrencesForDynamicMuseum.instance.minimap.SetActive(true);
                else
                    ReferrencesForDynamicMuseum.instance.minimap.SetActive(false);
            }
            return;
        }
        else
        {
            if (XanaConstants.xanaConstants.minimap == 1)
                ReferrencesForDynamicMuseum.instance.minimap.SetActive(true);
            else
                ReferrencesForDynamicMuseum.instance.minimap.SetActive(false);
        }

    }


    public void forcetodisable()
    {
        foreach (GameObject go in disableObjects)
        {
            go.SetActive(false);
        }
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.UpdateCanvasForMuseum(false);
    }
    public void forcetoenable()
    {
        foreach (GameObject go in disableObjects)
        {
            go.SetActive(true);
        }
        if (GamePlayButtonEvents.inst != null) GamePlayButtonEvents.inst.UpdateCanvasForMuseum(true);
    }

    private void Start()
    {
        StartCoroutine(SetPlayerCounter());
    }

    IEnumerator SetPlayerCounter()
    {
        CheckAgain:
        try
        {
            if (totalCounter != null)
            {
                //        Debug.LogError("Player count====" + PhotonNetwork.CurrentRoom.PlayerCount+"------"+XanaConstants.xanaConstants.userLimit);
                totalCounter.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + XanaConstants.xanaConstants.userLimit;
            }
        }
        catch (Exception e)
        {

        }

        yield return new WaitForSeconds(2f);
        goto CheckAgain;
    }
}
