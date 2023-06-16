using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

//Rigidbody is required as force is added to the Rigidbody component
[RequireComponent(typeof(Rigidbody))]
public class AddForceComponent : ItemComponent
{

    //Reference to the component data class
    [SerializeField]
    private AddForceComponentData addForceComponentData;

    //Rigidbody of the component this script is attached to
    Rigidbody rigidBody;

    //Checks if the force be applied or not
    bool isActivated = false;

    int forceMultiplier = 10;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnValidate()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Init(AddForceComponentData addForceComponentData)
    {
        this.addForceComponentData = addForceComponentData;
        isActivated = addForceComponentData.isActive;
        ApplyAddForce();
    }


    //Applies force if isActivated is true
    private void Update()
    {
    }

    public void ApplyAddForce()
    {
        if (isActivated)
        {
            rigidBody.isKinematic = false;
            rigidBody.AddRelativeForce(addForceComponentData.forceDirection * addForceComponentData.forceAmount * forceMultiplier * Time.deltaTime, ForceMode.VelocityChange);
            isActivated = false;
            StartCoroutine(SetIsKinematiceTrue());
        }
    }

    IEnumerator SetIsKinematiceTrue()
    {
        //wait so the applied force takes effect
        yield return new WaitForSeconds(1);

        while (rigidBody.velocity.magnitude > 0.0001f)
        {
            yield return null;
        }

        rigidBody.isKinematic = true;
    }


}
