using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UiManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panelHome;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelEnd;
    [SerializeField] private GameObject panelHUD;
    
    [Header("References")]
    [SerializeField] private RSE_UiCallback rse_callbackAction;

    private void Start()
    {
        panelHome.SetActive(true);
        panelSettings.SetActive(false);
        panelPause.SetActive(false);
        panelEnd.SetActive(false);
        panelHUD.SetActive(false);
    }
    
    public void Play()
    {
        panelHome.SetActive(false);
        rse_callbackAction.Call(UiActionGame.Play, () => { });
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
        panelHome.SetActive(open);
    }

    public void SettingsButton(bool open)
    {
        panelPause.SetActive(!open);
        panelSettings.SetActive(open);
    }
    
    public void ReplayButton()
    {
        panelHUD.SetActive(false);
        rse_callbackAction.Call(UiActionGame.Play, () => { });
        panelHUD.SetActive(true);
    }

    public void BackHomeButton()
    {
        panelEnd.SetActive(false);
        rse_callbackAction.Call(UiActionGame.BackMenu, () => { });
        panelHome.SetActive(true);
    }
    
}

[Serializable]
public enum UiActionGame
{
    BackMenu,
    Play
}
