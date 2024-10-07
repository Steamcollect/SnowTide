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

        BumpVisual();
    }

    void BumpVisual()
    {
        scoreTxt.transform.DOScale(1.1f, .06f).OnComplete(() =>
        {
            scoreTxt.transform.DOScale(1, .08f);
        });
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