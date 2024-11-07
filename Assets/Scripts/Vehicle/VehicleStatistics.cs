using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleStatistics : MonoBehaviour
{
    [SerializeField] VehicleDriftFrictionStatistics driftFrictionStatistics;
    VehicleDriftFrictionStatistics currentFriction;
    [SerializeField] RSO_IntValue rsoPeopleAmount;
    [SerializeField] RSE_BasicEvent rseOnGameStart;

    int peopleCount;

    private void Start()
    {
        currentFriction = new VehicleDriftFrictionStatistics(
            driftFrictionStatistics.turnFriction,
            driftFrictionStatistics.slideFriction,
            driftFrictionStatistics.slideAngle,
            driftFrictionStatistics.driftFriction,
            driftFrictionStatistics.driftAngle
             );

        peopleCount = 0;
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
        currentFriction.turnFriction += frictionToAdd.turnFriction;
        currentFriction.driftFriction += frictionToAdd.driftFriction;
        currentFriction.slideFriction += frictionToAdd.slideFriction;

        currentFriction.driftAngle += frictionToAdd.driftAngle;
        if (currentFriction.driftAngle < 0) currentFriction.driftAngle = 0;

        currentFriction.slideAngle += frictionToAdd.slideAngle;
        if (currentFriction.slideAngle < 0) currentFriction.slideAngle = 0;

        peopleCount++;
        rsoPeopleAmount.Value = peopleCount;
    }

    private void OnEnable()
    {
        rseOnGameStart.action += Start;
    }
    private void OnDisable()
    {
        rseOnGameStart.action -= Start;
    }
}