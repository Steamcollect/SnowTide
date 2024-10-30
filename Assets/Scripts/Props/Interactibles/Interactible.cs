using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player")) OnPlayerCollision(collision.transform);
    }

    public abstract void ResetComponent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player")) OnPlayerCollision(other.attachedRigidbody.transform);
    }

    public abstract void OnPlayerCollision(Transform player);
}