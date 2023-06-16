using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class ChecklPostProcessing : MonoBehaviour
{
    public Camera thirdPersonCam;
    public Camera firstPersonCam;

    public List<string> m_SceneNames;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Work selfie 5===" + GamePlayButtonEvents.inst);
        if (GamePlayButtonEvents.inst != null)
            GamePlayButtonEvents.inst.OnSelfieButton += OnSelfieOpen;

        if (XanaConstants.xanaConstants.isBuilderScene)
        {
            firstPersonCam.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
            thirdPersonCam.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
            if (ReferrencesForDynamicMuseum.instance.m_34player != null)
                ReferrencesForDynamicMuseum.instance.m_34player.GetComponent<MyBeachSelfieCam>().Selfie.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
        }
        else
        {
            if (CheckPostProcessEnable())
            {
                firstPersonCam.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
                thirdPersonCam.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
                if (ReferrencesForDynamicMuseum.instance.m_34player != null)
                    ReferrencesForDynamicMuseum.instance.m_34player.GetComponent<MyBeachSelfieCam>().Selfie.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
            }
        }

        
    }

    void OnSelfieOpen()
    {
        //  Debug.LogError("on selfie open ");
        Debug.Log("Work selfie 6===" + ReferrencesForDynamicMuseum.instance.m_34player);
        if (ReferrencesForDynamicMuseum.instance.m_34player != null)
        {
            //if (XanaConstants.xanaConstants.EnviornmentName.Contains("Xana Festival") || XanaConstants.xanaConstants.EnviornmentName.Contains("NFTDuel Tournament"))
            if (CheckPostProcessEnable())
            {
                ReferrencesForDynamicMuseum.instance.m_34player.GetComponent<MyBeachSelfieCam>().Selfie.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
            }
            else if (XanaConstants.xanaConstants.isBuilderScene)
            {
                ReferrencesForDynamicMuseum.instance.m_34player.GetComponent<MyBeachSelfieCam>().Selfie.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
            }
        }
    }


    private void OnDisable()
    {
        if (GamePlayButtonEvents.inst != null)
            GamePlayButtonEvents.inst.OnSelfieButton -= OnSelfieOpen;
    }


    bool CheckPostProcessEnable()
    {
        if (m_SceneNames.Contains(XanaConstants.xanaConstants.EnviornmentName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}