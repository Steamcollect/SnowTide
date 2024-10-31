using UnityEngine;

[System.Serializable]
public class VehicleDriftFrictionStatistics
{
    public float turnFriction;

    [Space(5)]
    [SerializeField] public float slideAngle;
    public float slideFriction;

    [Space(5)]
    [SerializeField] public float driftAngle;
    public float driftFriction;

    public VehicleDriftFrictionStatistics(float turnFriction, float slideAngle, float slideFriction, float driftAngle, float driftFriction)
    {
        this.turnFriction = turnFriction;
        this.slideAngle = slideAngle;
        this.slideFriction = slideFriction;
        this.driftAngle = driftAngle;
        this.driftFriction = driftFriction;
    }
}