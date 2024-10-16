using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Tooltip("Forward drifting speed")] float driftminimumSpeed = 5;
    [SerializeField, Tooltip("Forward movement speed")] float moveMaximumSpeed = 10;

    float speedVelocity;
    Vector3 velocity;
    [SerializeField, Tooltip("Time to reach the max speed")] float accelerationTime;

    Vector3 rotationVelocity;

    [Space(10), Header("Rotation")]
    [SerializeField, Tooltip("Time to reach the target angle")] float turnSmoothTime = .3f;
    float turnSmoothVelocity;

    [SerializeField, Range(0, 180)] float maxVelocityAngle;
    [SerializeField, Range(0, 180)] float maxRotationAngle;

    [Space(10), Header("Drift")]
    [SerializeField] DriftFrictionStatistics driftFrictionStatistics;
	    float currentDriftAngle;

    [System.Serializable]
    public class DriftFrictionStatistics
    {
        public float turnFriction;

        [Space(5)]
        [SerializeField, Range(0, 180)] public float slideAngle;
        public float slideFriction;

        [Space(5)]
        [SerializeField, Range(0, 180)] public float driftAngle;
        public float driftFriction;

        public DriftFrictionStatistics(float turnFriction, float slideAngle, float slideFriction, float driftAngle, float driftFriction)
        {
            this.turnFriction = turnFriction;
            this.slideAngle = slideAngle;
            this.slideFriction = slideFriction;
            this.driftAngle = driftAngle;
            this.driftFriction = driftFriction;
        }
    }

    [SerializeField, Tooltip("TyreMarksReferences")] TrailRenderer[] tyreMarks;

    [Space(10), Header("References")]
    [SerializeField] Rigidbody rb;
    [SerializeField] VehicleDriftingScore vehicleDriftScore;

    Vector2 input;

    void Update()
    {
        CheckDrift();
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
    }

    #region Movement
    void Move()
    {
        rb.velocity = GetVelocity();
    }

    Vector3 GetVelocity()
    {
        // Get current friction
        float friction;
        if (currentDriftAngle > driftFrictionStatistics.driftAngle) friction = driftFrictionStatistics.driftFriction;
        else if (currentDriftAngle > driftFrictionStatistics.slideAngle) friction = driftFrictionStatistics.slideFriction;
        else friction = driftFrictionStatistics.turnFriction;

        // if there is no drift angle
		if(currentDriftAngle < .1f) currentDriftAngle = friction;

        // Get Move speed
        float targetSpeed = Mathf.Lerp(driftminimumSpeed, moveMaximumSpeed, 1 - Mathf.Clamp(currentDriftAngle, 0, 90) / 90);

        float speed = targetSpeed;
        if (speed < targetSpeed) speed = Mathf.SmoothDamp(speed, targetSpeed, ref speedVelocity, accelerationTime);
        else speed = targetSpeed;

        // Set velocity
        Vector3 direction = GetForwardDirection();
		
		velocity = Vector3.SmoothDamp(velocity.normalized, direction, ref rotationVelocity, friction / currentDriftAngle) * speed;
	
        velocity.y = 0;
        return velocity;
    }

    Vector3 GetForwardDirection()
    {
        Vector3 forward = transform.forward;
        float angle = Vector3.SignedAngle(Vector3.forward, forward, Vector3.up);

        if(Mathf.Abs(angle) > maxVelocityAngle)
        {
            angle = angle < 0 ? -maxVelocityAngle : maxVelocityAngle;
            forward = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
        }

        return forward;
    }

    void Rotate()
    {
        float currentAngle = transform.eulerAngles.y;
        float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        targetAngle = Mathf.Clamp(targetAngle, -maxRotationAngle, maxRotationAngle);

        float angle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        rb.rotation = Quaternion.Euler(0, angle, 0);
    }
    #endregion

    public void AddFriction(DriftFrictionStatistics frictionToAdd)
    {
        driftFrictionStatistics.turnFriction += frictionToAdd.turnFriction;
        driftFrictionStatistics.driftFriction += frictionToAdd.driftFriction;
        driftFrictionStatistics.slideFriction += frictionToAdd.slideFriction;

        driftFrictionStatistics.driftAngle += frictionToAdd.driftAngle;
        driftFrictionStatistics.slideAngle += frictionToAdd.slideAngle;
    }

    #region Drift
    void CheckDrift()
    {
        currentDriftAngle = Vector3.Angle(transform.forward, rb.velocity.normalized);
        if (currentDriftAngle >= driftFrictionStatistics.slideAngle)
        {
            StartEmmiter();
            vehicleDriftScore.SetDriftState(true);
        }
        else
        {
            StopEmmiter();
            vehicleDriftScore.SetDriftState(false);
        }
    }
    void StartEmmiter()
    {
        foreach (TrailRenderer T in tyreMarks)
        {
            T.emitting = true;
        }
    }
    void StopEmmiter()
    {
        foreach (TrailRenderer T in tyreMarks)
        {
            T.emitting = false;
        }
    }
    #endregion

    public void SetInput(Vector2 inputs)
    {
        input = inputs;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(driftFrictionStatistics.slideAngle, Vector3.up) * Vector3.forward * 2);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(driftFrictionStatistics.driftAngle, Vector3.up) * Vector3.forward * 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(-maxVelocityAngle, Vector3.up) * Vector3.forward * 2);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(maxVelocityAngle, Vector3.up) * Vector3.forward * 2);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(-maxRotationAngle, Vector3.up) * Vector3.forward * 2);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(maxRotationAngle, Vector3.up) * Vector3.forward * 2);

        if(rb)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + rb.velocity.normalized * 2);
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        }
    }
}