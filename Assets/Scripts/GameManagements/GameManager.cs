using System;
using System.Collections;
using System.Collections.Generic;
using BT.Save;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSE_Event OnPlayerDeath;
    [SerializeField] private RSE_Event OnCommandSave;
    [SerializeField] private RSE_UIAction OnUIAction;
    [SerializeField] private UnityEvent BackgroundMusicEvent;
    
    
    private void Start() => BackgroundMusicEvent.Invoke();

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
