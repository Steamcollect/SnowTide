using TMPro;
using UnityEngine;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int score;

    [Header("References")]
    [SerializeField] TMP_Text scoreTxt;

    [Header("RSE")]
    [SerializeField] RSE_IntEvent _RSE_IntEvent;

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
        _RSE_IntEvent.action += AddScore;
    }
    private void OnDisable()
    {
        _RSE_IntEvent.action -= AddScore;
    }
}