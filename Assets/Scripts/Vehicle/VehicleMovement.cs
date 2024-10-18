using System.Collections;
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

    [SerializeField, Range(0, 180), Tooltip("Max angle possible to reach")] float maxVelocityAngle;
    [SerializeField, Range(0, 180), Tooltip("Max angle possible to rotate the car visual")] float maxRotationAngle;

    [Space(10), Header("Drift")]
	float currentDriftAngle;    

    [SerializeField, Tooltip("TyreMarksReferences")] TrailRenderer[] tyreMarks;

    [Space(10), Header("Others")]
   [SerializeField] float impactBumpForce;

    [Space(10), Header("References")]
    [SerializeField] Rigidbody rb;
    [SerializeField] VehicleDriftingScore vehicleDriftScore;
    [SerializeField] VehicleStatistics statistics;

    Vector2 input;

    bool canMove = true;
    bool canRotate = true;
    bool isMoving = true;

    void Update()
    {
        if(isMoving) CheckDrift();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            if(canRotate) Rotate();
            if(canMove) Move();
        }            
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
        if (currentDriftAngle > statistics.Friciton.driftAngle) friction = statistics.Friciton.driftFriction;
        else if (currentDriftAngle > statistics.Friciton.slideAngle) friction = statistics.Friciton.slideFriction;
        else friction = statistics.Friciton.turnFriction;

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

    public void ToggleMovement()
    {
        isMoving = !isMoving;
        if (!isMoving)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }
    public void ToggleMovement(bool isMoving)
    {
        this.isMoving = isMoving;
        if (!isMoving)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    void Rotate()
    {
        float currentAngle = transform.eulerAngles.y;
        float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        targetAngle = Mathf.Clamp(targetAngle, -maxRotationAngle, maxRotationAngle);

        float angle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        rb.rotation = Quaternion.Euler(0, angle, 0);
    }

    IEnumerator LockMovement(float delay)
    {
        canMove = false;
        yield return new WaitForSeconds(delay);
        canMove = true;
    }
    IEnumerator LockRotation(float delay)
    {
        canRotate = false;
        yield return new WaitForSeconds(delay);
        canRotate = true;
    }
    #endregion

    #region Drift
    void CheckDrift()
    {
        currentDriftAngle = Vector3.Angle(transform.forward, rb.velocity.normalized);
        if (currentDriftAngle >= statistics.Friciton.slideAngle)
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

    private void OnCollisionEnter(Collision collision)
    {
        print("pass");
        Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + collision.contacts[0].normal * impactBumpForce, Color.blue, 10);

        StartCoroutine(LockMovement(.4f));
        StartCoroutine(LockRotation(.4f));

        Vector3 bumpDir = (collision.contacts[0].point - transform.position).normalized;
        rb.AddForce(collision.contacts[0].normal * impactBumpForce, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        if (statistics)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(statistics.Friciton.slideAngle, Vector3.up) * Vector3.forward * 2);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(statistics.Friciton.driftAngle, Vector3.up) * Vector3.forward * 2);
        }        

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