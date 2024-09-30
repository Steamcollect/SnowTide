using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : Interactible
{
    [SerializeField, Range(1,2)] float wheelFrictionMulitplier;

    public override void OnPlayerCollision(Transform player)
    {
        if (player.TryGetComponent(out VehicleMovement vehicleMovement))
        {
            vehicleMovement.AddPeople(wheelFrictionMulitplier);
            gameObject.SetActive(false);
        }
    }
}