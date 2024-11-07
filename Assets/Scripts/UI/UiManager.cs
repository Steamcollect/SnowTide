using System;
using System.Collections;
using BT.Save;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject canvasMenu;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelEnd;
    [SerializeField] private GameObject panelHUD;
    
    [Header("References")]
    [SerializeField] private RSE_UIAction onUIAction;
    [SerializeField] private RSE_Event onPlayerDeath;
    [SerializeField] RSE_BasicEvent rse_SetupDeathPanel;
    [SerializeField] private BT.Audio.SoundLauncher loseSoundLauncher;
    [SerializeField] RSE_PauseGame rsePauseGame;
    [SerializeField] RSE_BasicEvent rseOnGameStart;

    private void Start()
    {
        canvasMenu.SetActive(true);
        panelSettings.SetActive(false);
        panelPause.SetActive(false);
        panelEnd.SetActive(false);
        panelHUD.SetActive(false);
    }

    private void OnEnable() => onPlayerDeath.action += End;

    private void OnDisable() => onPlayerDeath.action -= End;

    public void Play()
    {
        canvasMenu.SetActive(false);
        onUIAction.Call(UiActionGame.Play, () => { });
        panelHUD.SetActive(true);
    }

    private void End()
    {
        panelHUD.SetActive(false);
        StartCoroutine(OpenDeathPanel());
    }
    IEnumerator OpenDeathPanel()
    {
        yield return new WaitForSeconds(1.5f);

        loseSoundLauncher.LaunchAudio();
        panelEnd.SetActive(true);
        panelEnd.transform.BumpVisual();
        rse_SetupDeathPanel.Call();
    }
    
    public void PauseButton(bool open)
    {
        panelHUD.SetActive(!open);
        onUIAction.Call(open ? UiActionGame.Pause : UiActionGame.Resume, () => { });
        rsePauseGame.Call(open);
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
        onUIAction.Call(UiActionGame.Restart, () => { panelHUD.SetActive(true); });
        rsePauseGame.Call(false);
    }

    public void BackHomeButton()
    {
        panelEnd.SetActive(false);
        panelPause.SetActive(false);
        onUIAction.Call(UiActionGame.BackMenu, () => {canvasMenu.SetActive(true); });
        rsePauseGame.Call(false);
        rseOnGameStart.Call();
    }

}

[Serializable]
public enum UiActionGame
{
    BackMenu,
    Pause,
    Resume,
    Play,
    Restart
}
