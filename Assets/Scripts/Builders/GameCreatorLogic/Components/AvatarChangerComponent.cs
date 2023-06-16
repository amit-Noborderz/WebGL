using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class AvatarChangerComponent : ItemComponent
{
    public AvatarChangerComponentData componentData;

    public void InitAvatarChanger(AvatarChangerComponentData componentData)
    {
        this.componentData = componentData;
    }

    //OnCollisionEnter Convert OnTriggerEnter
    /*private void OnCollisionEnter(Collision collision)
    {
        if (*//*collision.gameObject.CompareTag("Player") || *//*(collision.gameObject.tag == "PhotonLocalPlayer" && collision.gameObject.GetComponent<PhotonView>().IsMine))
        {
            collision.gameObject.GetComponent<BuildingDetect>().OnAvatarChangerEnter(componentData.setTimer, componentData.avatarIndex);
            Destroy(this.gameObject);
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (/*other.gameObject.CompareTag("Player") || */(other.gameObject.tag == "PhotonLocalPlayer" && other.gameObject.GetComponent<PhotonView>().IsMine))
        {
            other.gameObject.GetComponent<BuildingDetect>().OnAvatarChangerEnter(componentData.setTimer, componentData.avatarIndex);
            Destroy(this.gameObject);
        }
    }
}
