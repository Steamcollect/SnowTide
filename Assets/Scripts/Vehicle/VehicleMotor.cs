using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Joystick joystick;

    public Vector2 GetInputs()
    {
        return joystick.Direction.normalized;
    }
}