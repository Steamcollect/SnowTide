using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Tooltip("Forward drifting speed")] float driftminimumSpeed = 5;
    [SerializeField, Tooltip("Forward movement speed")] float moveMaximumSpeed = 10;

    float speedVelocity;
    [SerializeField, Tooltip("Time to reach the max speed")] float accelerationTime;

    Vector3 rotationVelocity;

    [Space(10), Header("Rotation")]
    [SerializeField, Tooltip("Time to reach the target angle")] float turnSmoothTime = .3f;
    float turnSmoothVelocity;

    [SerializeField, Range(0, 180)] float maxRotation;
    Vector3 posMaxRotationDir, negMaxRotationDir;

    [Space(10), Header("Drift")]
    [SerializeField] DriftFrictionStatistics driftFrictionStatistics;
	    float currentDriftAngle;

    [System.Serializable]
    struct DriftFrictionStatistics
    {
        public float turnFriction;

        [Space(5)]
        public float slideAngle;
        public float slideFriction;

        [Space(5)]
        public float driftAngle;
        public float driftFriction;
    }

    [SerializeField, Tooltip("TyreMarksReferences")] TrailRenderer[] tyreMarks;

    [Space(10), Header("References")]
    [SerializeField] Rigidbody rb;
    [SerializeField] VehicleDriftingScore vehicleDriftScore;

    Vector2 input;

    private void Start()
    {
        OnValidate();
    }

    void Update()
    {
        CheckDrift();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
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

        // Get Move speed
        float targetSpeed = Mathf.Lerp(driftminimumSpeed, moveMaximumSpeed, 1 - Mathf.Clamp(currentDriftAngle, 0, 90) / 90);

float speed = targetSpeed;
        if (speed < targetSpeed) speed = Mathf.SmoothDamp(speed, targetSpeed, ref speedVelocity, accelerationTime);
        else speed = targetSpeed;

        // Set velocity
        // Vector3 forward = Utils.Clamp(transform.forward, negMaxRotationDir, posMaxRotationDir);
		
		if(currentDriftAngle < .1f) currentDriftAngle = friction;
		
		Vector3 velocity = rb.velocity;
        velocity = Vector3.SmoothDamp(velocity.normalized, transform.forward, ref rotationVelocity, friction / currentDriftAngle) * speed;
	
		print(velocity);
        velocity.y = 0;
        return velocity;
    }

    void Rotate()
    {
        float currentAngle = transform.eulerAngles.y;
        float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

        //targetAngle = Mathf.Clamp(targetAngle, currentAngle - maxRotationAngle, currentAngle + maxRotationAngle);

        float angle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        rb.rotation = Quaternion.Euler(0, angle, 0);
    }
    #endregion

    public void AddFriction(float _friction)
    {
        //friction += friction;
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

    private void OnValidate()
    {
        posMaxRotationDir = Quaternion.AngleAxis(maxRotation, Vector3.up) * Vector3.forward;
        negMaxRotationDir = Quaternion.AngleAxis(-maxRotation, Vector3.up) * Vector3.forward;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(driftFrictionStatistics.slideAngle, Vector3.up) * Vector3.forward * 2);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(driftFrictionStatistics.driftAngle, Vector3.up) * Vector3.forward * 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(-maxRotation, Vector3.up) * Vector3.forward * 2);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(maxRotation, Vector3.up) * Vector3.forward * 2);

        if(rb)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + rb.velocity.normalized * 2);
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        }
    }
}