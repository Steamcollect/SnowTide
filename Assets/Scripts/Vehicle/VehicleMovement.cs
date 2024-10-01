using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Tooltip("Forward movement speed")] float forwardSpeed = 10;
    [SerializeField, Tooltip("Drifting speed")] float driftSpeed = 7;

    [Space(5)]
    [SerializeField, Tooltip("Transition time to changing movement speed (ex : drift speed to normal)")] float changementVelocityTime = .3f;
    float yMovement, yMovementVelocity;

    [Space(5)]
    [SerializeField, Tooltip("Lateral movement speed")] float moveSpeed = 15;
    [SerializeField, Tooltip("Time require to reach maximum speed")] float friction = 1.8f;
    float xInput, xMovement, xMovementVelocity;

    [Space(10), Header("Rotation")]
    [SerializeField, Tooltip("Time to reach the target angle")] float turnSmoothTime = .3f;
    float turnSmoothVelocity;
    [SerializeField, Tooltip("Minimum and Maximum rotation possible for the vehicle")] Vector2 minMaxRotation = new Vector2(-90, 90);

    [Space(10), Header("Drift")]
    [SerializeField, Tooltip("Minimum angle require to drift")] float minDriftAngle = 30;
    [SerializeField] TrailRenderer[] tyreMarks;
    float currentDriftAngle;
    bool isDrifting = false;

    [Space(10), Header("References")]
    [SerializeField] Rigidbody rb;

    Vector2 input;

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
    Vector2 GetVelocity()
    {
        yMovement = Mathf.SmoothDamp(yMovement, isDrifting ? driftSpeed : forwardSpeed, ref yMovementVelocity, changementVelocityTime);
        Vector3 forwardVelocity = Vector3.forward * yMovement;

        xInput = Mathf.SmoothDamp(xInput, input.x, ref xMovementVelocity, friction);
        xMovement = xInput * moveSpeed;

        Vector3 velocity = forwardVelocity;
        velocity.x = xMovement;

        return velocity;
    }

    void Rotate()
    {
        float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        targetAngle = Mathf.Clamp(targetAngle, -89, 89);

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        rb.rotation = Quaternion.Euler(0, angle, 0);
    }
    #endregion

    public void AddFriction(float _friction)
    {
        friction += friction;
    }

    #region Drift
    void CheckDrift()
    {
        currentDriftAngle = Vector3.Angle(transform.forward, rb.velocity.normalized);
        if (currentDriftAngle >= minDriftAngle)
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
}