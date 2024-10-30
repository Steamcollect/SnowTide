using UnityEngine;

public class VehicleMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] VehicleMovement vehicleMovement;
    [SerializeField] RSO_FloatingJoystick rsoFloatingJoystick;

    private void Update()
    {
        vehicleMovement.SetInput(rsoFloatingJoystick.Value.Direction);
    }
}