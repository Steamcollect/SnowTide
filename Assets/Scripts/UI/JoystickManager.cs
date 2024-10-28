using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : MonoBehaviour
{
    public GameObject joystick;

    public void SetActiveJoystick(bool active)
    {
        joystick.SetActive(active);
    }
}