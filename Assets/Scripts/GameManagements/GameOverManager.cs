using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] RSE_EventBasic rse_onDeath;
    [SerializeField] GameObject gameOverPanel;

    private void OnEnable() => rse_onDeath.action += OnDeath;

    private void OnDisable() => rse_onDeath.action -= OnDeath;

    public void ReloadButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  
    }

    public void OnDeath()
    {
        gameOverPanel.SetActive(true);
    }
}