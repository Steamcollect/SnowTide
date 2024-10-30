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
}