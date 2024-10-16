using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : Interactible
{
    [SerializeField, Range(.05f, 1)] float frictionToAdd;
    [SerializeField] int scoreGiven;

    [SerializeField] RSE_IntEvent rse_AddScore;

    public override void ResetComponent()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerCollision(Transform player)
    {
        if (player.TryGetComponent(out VehicleMovement vehicleMovement))
        {
            rse_AddScore.Call(scoreGiven);

            vehicleMovement.AddFriction(default);
            gameObject.SetActive(false);
        }
    }
}