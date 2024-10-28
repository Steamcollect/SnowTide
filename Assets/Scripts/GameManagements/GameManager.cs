using System;
using BT.Save;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool vehicleStopAtStart;
    
    [Header("References")]
    [SerializeField] private RSE_Event OnPlayerDeath;
    [SerializeField] private RSE_Event OnCommandSave;
    [SerializeField] private RSE_UIAction OnUIAction;
    [SerializeField] private RSE_SetStateActive rseSetStateJoystick;
    [SerializeField] private RSO_VehicleMovement rsoVehicleMovement;
    [SerializeField] private UnityEvent BackgroundMusicEvent;
    
    private void Start()
    {
        if (vehicleStopAtStart)
        {
            rsoVehicleMovement.Value.ToggleMovement(false);
        }
        BackgroundMusicEvent.Invoke();
        rseSetStateJoystick.Call(false);
    }

    private void OnEnable()
    {
        OnPlayerDeath.action+= GameOver;
        OnUIAction.action += UIActionGame;
    }

    private void OnDisable()
    {
        OnPlayerDeath.action -= GameOver;
        OnUIAction.action -= UIActionGame;
    }

    private void GameOver()
    {
        
    }

    private void Play()
    {
        
    }

    private void BackMenu()
    {
        
    }

    /// <summary>
    /// Callback of ui, when an specific action is set, like restart or back menu
    /// </summary>
    /// <param name="action"> Type of the action</param>
    /// <param name="ev"> Event callback to ui</param>
    private void UIActionGame(UiActionGame actionUI, Action ev)
    {
        switch (actionUI)
        {
            case UiActionGame.Play:
                Play();
                break;
            case UiActionGame.BackMenu:
                BackMenu();
                break;
        }
    }
    
    private void OnApplicationQuit()
    {
        OnCommandSave.Call();
    }
}
