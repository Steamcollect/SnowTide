using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleStatistics : MonoBehaviour
{
    [SerializeField] VehicleDriftFrictionStatistics driftFrictionStatistics;
    VehicleDriftFrictionStatistics currentFriction;
    [SerializeField] RSO_IntValue rsoPeopleAmount;

    int peopleCount;

    private void Start()
    {
        currentFriction = driftFrictionStatistics;
        rsoPeopleAmount.Value = 0;
    }

    public int PeopleGet
    {
        get
        {
            return peopleCount;
        }
    }
    public VehicleDriftFrictionStatistics Friciton
    {
        get
        {
            return currentFriction;
        }
    }
    public int DistanceTravelled
    {
        get
        {
            return (int)transform.position.z;
        }
    }

    public void AddFriction(VehicleDriftFrictionStatistics frictionToAdd)
    {
        driftFrictionStatistics.turnFriction += frictionToAdd.turnFriction;
        driftFrictionStatistics.driftFriction += frictionToAdd.driftFriction;
        driftFrictionStatistics.slideFriction += frictionToAdd.slideFriction;

        driftFrictionStatistics.driftAngle += frictionToAdd.driftAngle;
        driftFrictionStatistics.slideAngle += frictionToAdd.slideAngle;

        peopleCount++;
        rsoPeopleAmount.Value = peopleCount;
    }

    [SerializeField] RSE_BasicEvent rse_OnGameStart;
    private void OnEnable()
    {
        rse_OnGameStart.action += ResetPeopleAmount;
    }
    private void OnDisable()
    {
        rse_OnGameStart.action -= ResetPeopleAmount;
    }

    void ResetPeopleAmount()
    {
        peopleCount = 0;
        rsoPeopleAmount.Value = 0;
    }
}