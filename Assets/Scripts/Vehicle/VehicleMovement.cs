using Unity.VisualScripting;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [Header("Statistics")]
    [SerializeField] float moveSpeed = 1500;

    [Space(5)]
    [SerializeField] float turnSensitivity = 1;
    [SerializeField] float maxSteerAngle = 30;

    [Space(5)]
    [SerializeField] Vector3 massCenterPos;
    [SerializeField] float brakeForce;
    [SerializeField] AnimationCurve brakeForceCurve;

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
    float diffAngleInput;

    private void Start()
    {
        rb.centerOfMass = massCenterPos;
    }

    private void Update()
    {
        SetInputs();
    }

    private void LateUpdate()
    {
        AnimateWheel();
    }

    private void FixedUpdate()
    {
        Move();
        Steer();
        //Brake();
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
                float steerAngle = diffAngleInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, .6f);
            }
        }
    }
    void Brake()
    {
        foreach(Wheel wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = brakeForceCurve.Evaluate(Mathf.Abs(diffAngleInput)) * Time.fixedDeltaTime;
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

        if(input.magnitude > .01f)
        {
            float angle = Vector2.Angle(transform.forward, input);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + massCenterPos, .1f);
    }
}