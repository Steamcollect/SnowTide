using UnityEngine;

public class VehicleMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] VehicleMovement vehicleMovement;
    [SerializeField] Joystick joystick;

    private void Update()
    {
        vehicleMovement.SetInput(joystick.Direction);
    }
}