using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class JoystickManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Joystick joystick;
    
    [Header("References")]
    [SerializeField] private RSE_SetStateActive rseSetStateActive;

    private void OnEnable() => rseSetStateActive.action += SetActiveJoystick;
    private void OnDisable() => rseSetStateActive.action -= SetActiveJoystick;


    private void SetActiveJoystick(bool active)
    {
        joystick.enabled = active;
    }
}