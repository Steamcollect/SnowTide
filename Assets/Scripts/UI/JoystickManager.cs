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
    [SerializeField] RSO_FloatingJoystick rso_FloatingJoystick;
    [SerializeField] RSE_BasicEvent rse_OnStartEvent;

    private void OnEnable()
    {
        rse_OnStartEvent.action += ResetJoystick;
        rseSetStateActive.action += SetActiveJoystick;
    }
    private void OnDisable()
    {
        rse_OnStartEvent.action -= ResetJoystick;
        rseSetStateActive.action -= SetActiveJoystick;
    }

    private void Awake()
    {
        rso_FloatingJoystick.Value = joystick as FloatingJoystick;
    }

    private void SetActiveJoystick(bool active)
    {
        ResetJoystick();
        joystick.gameObject.SetActive(active);
    }

    void ResetJoystick()
    {
        joystick.OnReset();
    }
}