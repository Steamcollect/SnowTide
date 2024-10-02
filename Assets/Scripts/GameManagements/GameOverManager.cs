using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] RSE_EventBasic rse_onDeath;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject buttonPanel;

    private void OnEnable() => rse_onDeath.action += OnDeath;

    private void OnDisable() => rse_onDeath.action -= OnDeath;

    public void ReloadButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  
    }

    public void OnDeath()
    {
        buttonPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }
}