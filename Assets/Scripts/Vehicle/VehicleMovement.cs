using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [Header("Statistics")]
    [SerializeField] float maxAcceleration = 30;
    [SerializeField] float braleAcceleration = 50;

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

    Vector2 input;

    private void Update()
    {
        SetInputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = input.y * maxAcceleration * Time.fixedDeltaTime;
        }
    }

    void SetInputs()
    {
        input = motor.GetInputs();
    }
}
