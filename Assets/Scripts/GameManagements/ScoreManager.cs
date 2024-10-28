using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int score;

    [Header("References")]
    [SerializeField] TMP_Text scoreTxt;
    
    [Header("RSE")]
    [SerializeField] RSE_IntEvent rse_AddScore;
    [SerializeField] RSE_SetStateActive rseSetStateActive;
    
    private void Start()
    {
        SetTextVisual();
    }

    private void AddScore(int scoreGiven)
    {
        score += scoreGiven;
        SetTextVisual();

        scoreTxt.transform.BumpVisual();
    }

    void SetTextVisual()
    {
        scoreTxt.text = score.ToString();
    }

    private void SetActiveVisual(bool active)
    {
        scoreTxt.gameObject.SetActive(active);
    }

    private void OnEnable()
    {
        rse_AddScore.action += AddScore;
        rseSetStateActive.action += SetActiveVisual;
    }
    private void OnDisable()
    {
        rse_AddScore.action -= AddScore;
        rseSetStateActive.action -= SetActiveVisual;
    }
}