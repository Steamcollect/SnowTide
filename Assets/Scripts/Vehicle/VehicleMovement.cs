using Unity.VisualScripting;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [Header("Statistics")]
    [SerializeField] float moveSpeed = 1500;

    [Space(5)]
    [SerializeField] float turnSensitivity = 100;
    [SerializeField] float maxSteerAngle = 45;

    [Space(5)]
    [SerializeField] Vector3 massCenterPos;
    [SerializeField] AnimationCurve brakeForceCurve;
    [SerializeField] float brakeForce;

    [Space(10)]
    [SerializeField] Wheel[] wheels;

    [System.Serializable] class Wheel
    {
        public WheelCollider wheelCollider;

        [Space(5)]
        public WheelType wheelType;

        [Space(5)]
        public GameObject wheelModel;

        public enum WheelType
        {
            Front,
            Rear
        }
    }

    [Header("References")]
    [SerializeField] VehicleMotor motor;
    [SerializeField] Rigidbody rb;

    Vector3 input;
    float steerInput;

    private void Start()
    {
        rb.centerOfMass = massCenterPos;
    }

    private void Update()
    {
        SetInputs();

        if(Input.GetKeyDown(KeyCode.Space)) AddPeople();
    }

    private void LateUpdate()
    {
        AnimateWheel();
    }

    private void FixedUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    void Move()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveSpeed * Time.fixedDeltaTime;
        }
    }

    void Steer()
    {
        foreach (Wheel wheel in wheels)
        {
            if(wheel.wheelType == Wheel.WheelType.Front)
            {
                float steerAngle = steerInput * turnSensitivity;
                wheel.wheelCollider.steerAngle = steerInput;
            }
        }
    }

    void AddPeople()
    {
        foreach (Wheel wheel in wheels)
        {
            WheelFrictionCurve newCurve = wheel.wheelCollider.forwardFriction;
            newCurve.extremumSlip *= 1.1f;
            newCurve.asymptoteSlip *= 1.1f;

            wheel.wheelCollider.forwardFriction = newCurve;

            newCurve = wheel.wheelCollider.sidewaysFriction;
            newCurve.extremumSlip *= 1.1f;
            newCurve.asymptoteSlip *= 1.1f;

            wheel.wheelCollider.sidewaysFriction = newCurve;
        }
    }

    void Brake()
    {
        foreach  (Wheel wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = brakeForceCurve.Evaluate(steerInput) * brakeForce * Time.fixedDeltaTime;
        }
    }

    void AnimateWheel()
    {
        foreach(Wheel wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    void SetInputs()
    {
        input = motor.GetInputs();

        if (input.magnitude > .01f)
        {
            float angle = Vector3.SignedAngle(input, transform.forward, transform.up) * -1;
            steerInput = Mathf.Clamp(angle, -maxSteerAngle, maxSteerAngle);
        }
        else
        {
            steerInput = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + massCenterPos, .1f);
    }
}