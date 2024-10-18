using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : Interactible
{
    [SerializeField] VehicleDriftFrictionStatistics frictionToAdd;

    [SerializeField] int scoreGiven;

    [SerializeField] RSE_IntEvent rse_AddScore;
    [SerializeField] private GameObject visual;

    public override void ResetComponent()
    {
        gameObject.SetActive(true);
    }

    public override void OnPlayerCollision(Transform player)
    {
        if (!player.TryGetComponent(out VehicleStatistics vehicleStatistics)) return;
        rse_AddScore.Call(scoreGiven);
        vehicleStatistics.AddFriction(frictionToAdd);
        gameObject.SetActive(false);
    }
}