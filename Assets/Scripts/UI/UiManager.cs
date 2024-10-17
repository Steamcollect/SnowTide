using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UiManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject canvasMenu;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelEnd;
    [SerializeField] private GameObject panelHUD;
    
    [Header("References")]
    [SerializeField] private RSE_UiCallback callbackUiAction;

    private void Start()
    {
        canvasMenu.SetActive(true);
        panelSettings.SetActive(false);
        panelPause.SetActive(false);
        panelEnd.SetActive(false);
        panelHUD.SetActive(false);
    }
    
    public void Play()
    {
        canvasMenu.SetActive(false);
        callbackUiAction.Call(UiActionGame.Play, () => { });
        panelHUD.SetActive(true);
    }

    public void End()
    {
        panelHUD.SetActive(false);
        panelEnd.SetActive(true);
    }
    
    public void PauseButton(bool open)
    {
        panelHUD.SetActive(!open);
        panelPause.SetActive(open);
    }

    public void SettingsButton(bool open)
    {
        panelPause.SetActive(!open);
        panelSettings.SetActive(open);
    }
    
    public void ReplayButton()
    {
        panelEnd.SetActive(false);
        panelPause.SetActive(false);
        panelHUD.SetActive(false);
        callbackUiAction.Call(UiActionGame.Play, () => { });
        panelHUD.SetActive(true);
    }

    public void BackHomeButton()
    {
        panelEnd.SetActive(false);
        panelPause.SetActive(false);
        callbackUiAction.Call(UiActionGame.BackMenu, () => { });
        canvasMenu.SetActive(true);
    }
    
}

[Serializable]
public enum UiActionGame
{
    BackMenu,
    Play
}
