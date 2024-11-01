using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class People : Interactible
{
    [Header("Parameters")]
    [SerializeField] VehicleDriftFrictionStatistics frictionToAdd;
    [SerializeField] int scoreGiven;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("References")]
    [SerializeField] RSE_IntEvent rse_AddScore;
    
    [Header("Events")]
    [SerializeField] private UnityEvent OnPickedUp;
    [SerializeField] private UnityEvent OnReset;

    public override void ResetComponent()
    {
        gameObject.SetActive(true);
        OnReset?.Invoke();
    }

    public override void OnPlayerCollision(Transform player)
    {
        if (!player.TryGetComponent(out VehicleStatistics vehicleStatistics)) return;
        OnPickedUp?.Invoke();
        rse_AddScore.Call(scoreGiven);
        vehicleStatistics.AddFriction(frictionToAdd);
        StartCoroutine(Utils.Delay(()=> gameObject.SetActive(false),fadeDuration));
    }
}