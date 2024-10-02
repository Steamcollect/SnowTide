using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Tooltip("Forward drifting speed")] float driftminimumSpeed = 5;
    [SerializeField, Tooltip("Forward movement speed")] float moveMaximumSpeed = 10;

    Vector3 velocity;
    Vector3 rotationVelocity;

    //[Space(5)]
    //[SerializeField, Tooltip("Transition time to changing movement speed (ex : drift speed to normal)")] float changementVelocityTime = .3f;
    //float yMovement, yMovementVelocity;

    //[Space(5)]
    //[SerializeField, Tooltip("Lateral movement speed")] float moveSpeed = 15;
    //[SerializeField, Tooltip("Time require to reach maximum speed")] float friction = 1.8f;
    //float xInput, xMovement, xMovementVelocity;

    [Space(10), Header("Rotation")]
    [SerializeField, Tooltip("Time to reach the target angle")] float turnSmoothTime = .3f;
    float turnSmoothVelocity;

    [Space(10), Header("Drift")]

    // Drift rotation statistics
    [SerializeField] DriftRotationStatistics driftRotationStatistics;
    [System.Serializable]
    struct DriftRotationStatistics
    {
        public float turnRotationSpeed;

        [Space(5)]
        public float slideAngle;
        public float slideRotationSpeed;

        [Space(5)]
        public float driftAngle;
        public float driftRotationSpeed;
    }

    [SerializeField] TrailRenderer[] tyreMarks;
    float currentDriftAngle;
    bool isDrifting = false;

    [Space(10), Header("References")]
    [SerializeField] Rigidbody rb;

    Vector2 input;

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
        // Get rotationSpeed
        float rotationSpeed;
        if (currentDriftAngle > driftRotationStatistics.driftAngle) rotationSpeed = driftRotationStatistics.driftRotationSpeed;
        else if (currentDriftAngle > driftRotationStatistics.slideAngle) rotationSpeed = driftRotationStatistics.slideRotationSpeed;
        else rotationSpeed = driftRotationStatistics.turnRotationSpeed;

        float moveSpeed =  Mathf.Lerp(driftminimumSpeed, moveMaximumSpeed, -(Mathf.Clamp(currentDriftAngle, 0,90) / 90));

        // Set velocity
        velocity = Vector3.SmoothDamp(velocity.normalized, transform.forward, ref rotationVelocity, rotationSpeed / currentDriftAngle) * moveSpeed;

        Debug.DrawRay(transform.position, rb.velocity.normalized, Color.red);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);

        velocity.y = 0;
        return velocity;
    }

    //Vector3 GetVelocity()
    //{
    //    Vector3 velocity;

    //    yMovement = Mathf.SmoothDamp(yMovement, isDrifting ? driftSpeed : forwardSpeed, ref yMovementVelocity, changementVelocityTime);
    //    Vector3 forwardVelocity = Vector3.Lerp(rb.velocity.normalized, transform.forward, .5f) * forwardSpeed;
    //    Debug.DrawRay(transform.position, forwardVelocity, Color.red);

    //    xInput = Mathf.SmoothDamp(xInput, input.x, ref xMovementVelocity, friction);
    //    xMovement = xInput * moveSpeed;

    //    Vector3 velocity = forwardVelocity;
    //    velocity.x = xMovement;

    //    return velocity;
    //}

    void Rotate()
    {
        float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        //targetAngle = Mathf.Clamp(targetAngle, minMaxRotation.x, minMaxRotation.y);

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

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
        if (currentDriftAngle >= driftRotationStatistics.slideAngle)
        {
            StartEmmiter();
            isDrifting = true;
        }
        else
        {
            StopEmmiter();
            isDrifting = false;
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
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(driftRotationStatistics.slideAngle, Vector3.up) * Vector3.forward * 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(driftRotationStatistics.driftAngle, Vector3.up) * Vector3.forward * 2);
    }
}