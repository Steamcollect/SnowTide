using System;
using System.Collections;
using BT.Save;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private bool vehicleStopAtStart;
    [SerializeField] private Transform vehicleSpawnPoint;
    
    [Header("References")]
    [SerializeField] private RSE_Event rse_StartBuildAuto;
    [SerializeField] private RSE_Event rse_StopBuildAuto;
    [SerializeField] private RSE_SwapChunk rse_SwapChunk;
    [Space(10)]
    [SerializeField] private RSE_Event OnPlayerDeath;
    [SerializeField] private RSE_Event OnCommandSave;
    [SerializeField] private RSE_UIAction OnUIAction;
    [Space(10)] 
    [SerializeField] private RSE_FadeInOut rseFadeInOut;
    [Space(10)]
    [SerializeField] private RSE_SetStateActive rse_SetStateJoystick;
    [SerializeField] private RSO_VehicleMovement rso_VehicleMovement;
    [Space(10)]
    [SerializeField] private BT.Audio.Playlist menuPlaylist;
    [SerializeField] private BT.Audio.Playlist gamePlaylist;
    [Space(10)]
    [SerializeField] RSE_BasicEvent rse_OnGameStart;

    private void Start()
    {
        if (vehicleStopAtStart)
        {
            rso_VehicleMovement.Value.ToggleMovement(false);
        }
        menuPlaylist.LaunchPlaylist();
        rse_SetStateJoystick.Call(false);
        rso_VehicleMovement.Value.SnapPosition(vehicleSpawnPoint.position);
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
        gamePlaylist.StopPlaylist();
        
        rse_SetStateJoystick.Call(false);
        rse_StopBuildAuto.Call();
    }

    private void Play()
    {
        rse_SwapChunk.Call(GameState.Road);
        rse_SetStateJoystick.Call(true);
    }

    private IEnumerator BackMenu()
    {
        rse_SetStateJoystick.Call(false);
        rseFadeInOut.Call(true);
        yield return new WaitForSeconds(0.30f);
        rso_VehicleMovement.Value.ResetVehicle(vehicleSpawnPoint.position);
        rse_SwapChunk.Call(GameState.Menu);
        rse_StopBuildAuto.Call();
        yield return new WaitForSeconds(0.2f);
        rse_StartBuildAuto.Call();
        yield return new WaitForSeconds(0.2f);
        rso_VehicleMovement.Value.ToggleMovement(true);
        rseFadeInOut.Call(false);
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

    private IEnumerator RestartGame()
    {
        yield return StartCoroutine(BackMenu());
        yield return new WaitForSeconds(0.3f);
        rse_OnGameStart.Call();
        Play();
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
                menuPlaylist.StopPlaylist();
                gamePlaylist.LaunchPlaylist();
                Play();
                break;
            case UiActionGame.BackMenu:
                gamePlaylist.StopPlaylist();
                menuPlaylist.LaunchPlaylist();
                StartCoroutine(BackMenu());
                StartCoroutine(Utils.Delay(() => ev?.Invoke(),0.3f));
                break;
            case UiActionGame.Pause:
                Pause();
                break;
            case UiActionGame.Resume:
                Resume();
                break;
            case UiActionGame.Restart:
                gamePlaylist.StopPlaylist();
                StartCoroutine(RestartGame());
                StartCoroutine(Utils.Delay(() =>
                {
                    gamePlaylist.LaunchPlaylist();
                    ev?.Invoke();
                },0.3f));
                break;
        }
    }

    private void OnApplicationQuit()
    {
        OnCommandSave.Call();
    }
}
