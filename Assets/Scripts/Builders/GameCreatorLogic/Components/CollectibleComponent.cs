using UnityEngine;
using Models;

public class CollectibleComponent : ItemComponent
{
    private CollectibleComponentData collectibleComponentData;

    private bool activateComponent = false;

    public void Init(CollectibleComponentData collectibleComponentData)
    {
        this.collectibleComponentData = collectibleComponentData;

        activateComponent = true;

        Rigidbody rb=gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PhotonLocalPlayer"))
        {
            gameObject.SetActive(false);
            Debug.LogError("Here we need to show message to user for collectible object");
        }
    }
}