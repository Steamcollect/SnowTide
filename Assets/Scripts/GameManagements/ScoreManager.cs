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

    private void Start()
    {
        SetTextVisual();
    }

    public void AddScore(int scoreGiven)
    {
        score += scoreGiven;
        SetTextVisual();

        scoreTxt.transform.BumpVisual();
    }

    void SetTextVisual()
    {
        scoreTxt.text = score.ToString();
    }

    private void OnEnable()
    {
        rse_AddScore.action += AddScore;
    }
    private void OnDisable()
    {
        rse_AddScore.action -= AddScore;
    }
}