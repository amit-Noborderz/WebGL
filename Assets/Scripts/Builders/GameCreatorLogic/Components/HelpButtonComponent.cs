using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Models;
using Photon.Pun;

//[RequireComponent(typeof(Rigidbody))]
public class HelpButtonComponent : ItemComponent
{
    [SerializeField]
    private HelpButtonComponentData helpButtonComponentData;

    string collectingAllTheStrings;
    public void Init(HelpButtonComponentData helpButtonComponentData)
    {
        this.helpButtonComponentData = helpButtonComponentData;
        //GetComponent<Rigidbody>().isKinematic = true;
        //GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void SetHelpButtonNarration()
    {
        Debug.Log("SetHelpButtonNarration");
        BuilderEventManager.OnHelpButtonCollisionEnter?.Invoke(helpButtonComponentData.titleHelpButton, helpButtonComponentData.helpButtonData);
        /*if (CanvasComponenetsManager._instance != null)
        {
            CanvasComponenetsManager._instance.helpParentReference.SetActive(true);
            collectingAllTheStrings = "";
            CanvasComponenetsManager._instance.helpButtonTitle.text = helpButtonComponentData.titleHelpButton;
            if (helpButtonComponentData.helpButtonData.Count == 0)
            {
                CanvasComponenetsManager._instance.helpButtonTextInformation.text = "Define Rules here !";
            }
            else
            {
                for (int i = 0; i < helpButtonComponentData.helpButtonData.Count; i++)
                {
                    collectingAllTheStrings += "->  " + helpButtonComponentData.helpButtonData[i] + "\n" + "\n";
                    CanvasComponenetsManager._instance.helpButtonTextInformation.text = collectingAllTheStrings;
                }
            }
        }*/
    }

    //OnCollisionenter to OnTriggerEnter
    /*private void OnCollisionEnter(Collision _other)
    {
        Debug.Log("Help Button Collision Enter: " + _other.gameObject.name);
        if (_other.gameObject.CompareTag("Player") || (_other.gameObject.tag == "PhotonLocalPlayer" && _other.gameObject.GetComponent<PhotonView>().IsMine))
        {
            //if (CanvasComponenetsManager._instance != null)
            {
                SetHelpButtonNarration();
            }
            //StartTriggerEvent?.Invoke();
        }
    }*/
    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("Help Button Collision Enter: " + _other.gameObject.name);
        if (_other.gameObject.CompareTag("Player") || (_other.gameObject.tag == "PhotonLocalPlayer" && _other.gameObject.GetComponent<PhotonView>().IsMine))
        {
            //if (CanvasComponenetsManager._instance != null)
            {
                SetHelpButtonNarration();
            }
            //StartTriggerEvent?.Invoke();
        }
    }
}
