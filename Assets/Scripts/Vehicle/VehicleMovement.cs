using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Tooltip("Forward drifting speed")] float driftminimumSpeed = 5;
    [SerializeField, Tooltip("Forward movement speed")] float moveMaximumSpeed = 10;

    float speedVelocity;
    float speed;
    [SerializeField] float accelerationTime;

    Vector3 velocity;
    Vector3 rotationVelocity;

    [Space(10), Header("Rotation")]
    [SerializeField, Tooltip("Time to reach the target angle")] float turnSmoothTime = .3f;
    [SerializeField] float maxRotationAngle;
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

        // Get Move speed
        float targetSpeed = Mathf.Lerp(driftminimumSpeed, moveMaximumSpeed, 1 - Mathf.Clamp(currentDriftAngle, 0, 90) / 90);

        if (speed < targetSpeed) speed = Mathf.SmoothDamp(speed, targetSpeed, ref speedVelocity, accelerationTime);
        else speed = targetSpeed;

        // Set velocity
        velocity = Vector3.SmoothDamp(velocity.normalized, transform.forward, ref rotationVelocity, rotationSpeed / currentDriftAngle) * speed;

        Debug.DrawRay(transform.position, rb.velocity.normalized, Color.red);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);

        velocity.y = 0;
        return velocity;
    }

    void Rotate()
    {
        float currentAngle = transform.eulerAngles.y;
        float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

        targetAngle = Mathf.Clamp(targetAngle, currentAngle - maxRotationAngle, currentAngle + maxRotationAngle);

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