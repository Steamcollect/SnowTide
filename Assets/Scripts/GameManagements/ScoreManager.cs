using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int score;
        
    [Header("RSE")]
    [SerializeField] RSE_IntEvent rse_AddScore;
    [SerializeField] RSE_SetStateActive rseSetStateActive;

    [Header("RSO")]
    [SerializeField] RSO_IntValue rsoScore;
    [SerializeField] RSO_TxtSet rsoTxtSet;
    
    private void Start()
    {
        SetTextVisual();
    }

    private void AddScore(int scoreGiven)
    {
        score += scoreGiven;
        rsoScore.Value = score;
        SetTextVisual();
    }

    void SetTextVisual()
    {
        rsoTxtSet.Value.text = score.ToString();
        rsoTxtSet.Value.transform.BumpVisual();
    }

    private void SetActiveVisual(bool active)
    {
        rsoTxtSet.Value.gameObject.SetActive(active);
    }

    private void OnEnable()
    {
        rse_AddScore.action += AddScore;
        rseSetStateActive.action += SetActiveVisual;
        rse_OnGameStart.action += ResetPeopleAmount;
    }
    private void OnDisable()
    {
        rse_AddScore.action -= AddScore;
        rseSetStateActive.action -= SetActiveVisual;
        rse_OnGameStart.action -= ResetPeopleAmount;
    }

    [SerializeField] RSE_BasicEvent rse_OnGameStart;
    void ResetPeopleAmount()
    {
        score = 0;
        rsoScore.Value = 0;
    }
}