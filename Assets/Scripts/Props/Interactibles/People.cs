using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class People : Interactible
{
    [SerializeField] VehicleDriftFrictionStatistics frictionToAdd;

    [SerializeField] int scoreGiven;

    [SerializeField] RSE_IntEvent rse_AddScore;
    [SerializeField] private UnityEvent OnPickedUp;

    public override void ResetComponent()
    {
        gameObject.SetActive(true);
    }

    public override void OnPlayerCollision(Transform player)
    {
        if (!player.TryGetComponent(out VehicleStatistics vehicleStatistics)) return;
        OnPickedUp?.Invoke();
        rse_AddScore.Call(scoreGiven);
        vehicleStatistics.AddFriction(frictionToAdd);
        gameObject.SetActive(false);
    }
}