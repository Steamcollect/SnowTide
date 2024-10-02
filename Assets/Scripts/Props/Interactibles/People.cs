using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : Interactible
{
    [SerializeField, Range(.05f, 1)] float frictionToAdd;

    public override void OnPlayerCollision(Transform player)
    {
        if (player.TryGetComponent(out VehicleMovement vehicleMovement))
        {
            vehicleMovement.AddFriction(frictionToAdd);
            gameObject.SetActive(false);
        }
    }
}