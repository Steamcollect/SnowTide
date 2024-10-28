using System;
using BT.Save;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private bool vehicleStopAtStart;
    
    [Header("References")]
    [SerializeField] private RSE_Event rse_StartBuildAuto;
    [SerializeField] private RSE_Event rse_StopBuildAuto;
    [SerializeField] private RSE_SwapChunk rse_SwapChunk;
    [Space(10)]
    [SerializeField] private RSE_Event OnPlayerDeath;
    [SerializeField] private RSE_Event OnCommandSave;
    [SerializeField] private RSE_UIAction OnUIAction;
    [Space(10)]
    [SerializeField] private RSE_SetStateActive rse_SetStateJoystick;
    [SerializeField] private RSO_VehicleMovement rso_VehicleMovement;
    [Space(10)]
    [SerializeField] private UnityEvent BackgroundMusicEvent;
    
    private void Start()
    {
        if (vehicleStopAtStart)
        {
            rso_VehicleMovement.Value.ToggleMovement(false);
        }
        BackgroundMusicEvent.Invoke();
        rse_SetStateJoystick.Call(false);
    }

    private void OnEnable()
    {
        OnPlayerDeath.action += GameOver;
        OnUIAction.action += UIActionGame;
    }

    private void OnDisable()
    {
        OnPlayerDeath.action -= GameOver;
        OnUIAction.action -= UIActionGame;
    }

    private void GameOver()
    {
        rse_SetStateJoystick.Call(false);
        rse_StopBuildAuto.Call();
    }

    private void Play()
    {
        rse_SetStateJoystick.Call(true);
        rse_SwapChunk.Call(GameState.Road);
    }

    private void BackMenu()
    {
        rse_SetStateJoystick.Call(false);
        rse_SwapChunk.Call(GameState.Menu);
        rse_StopBuildAuto.Call();
        rse_StartBuildAuto.Call();
        rso_VehicleMovement.Value.ToggleMovement(true);
    }

    private void Pause()
    {
        rse_SetStateJoystick.Call(false);
        rso_VehicleMovement.Value.ToggleMovement(false);
    }

    private void Resume()
    {
        rse_SetStateJoystick.Call(true);
        rso_VehicleMovement.Value.ToggleMovement(true);
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
            case UiActionGame.Pause:
                Pause();
                break;
            case UiActionGame.Resume:
                Resume();
                break;
            case UiActionGame.Restart:
                BackMenu();
                Play();
                break;
        }
        ev?.Invoke();
    }

    private void OnApplicationQuit()
    {
        OnCommandSave.Call();
    }
}
