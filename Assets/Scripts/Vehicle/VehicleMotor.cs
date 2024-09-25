using UnityEngine;

public class VehicleMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Joystick joystick;

    public Vector3 GetInputs()
    {
        Vector2 input = joystick.Direction.normalized;
        return new Vector3(input.x, 0, input.y);
    }
}