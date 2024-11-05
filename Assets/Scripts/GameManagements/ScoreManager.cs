using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int score;
    [SerializeField] GameObject scoreNotifPrefab;
    
    [Header("RSE")]
    [SerializeField] RSE_IntEvent rse_AddScore;
    [SerializeField] RSE_SetStateActive rseSetStateActive;
    [SerializeField] RSE_AddScoreNotif rseAddScoreNotif;

    [Header("RSO")]
    [SerializeField] RSO_IntValue rsoScore;
    [SerializeField] RSO_TxtSet rsoTxtSet;
    [SerializeField] RSO_ScoreCanvas rsoScoreCanvas;

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

    void AddScoreNotif(Vector3 position, int score)
    {
        TMP_Text text = Instantiate(scoreNotifPrefab, rsoScoreCanvas.Value).GetComponent<TMP_Text>();
        text.transform.BumpVisual();
        text.transform.position = (Vector2)Camera.main.WorldToScreenPoint(position) + Vector2.up * 100;
        text.transform.rotation = Quaternion.identity;
        text.text = "+" + score.ToString("#,0");
        text.transform.DOMove((Vector2)Camera.main.WorldToScreenPoint(position) + Vector2.up * 300, 1);
        text.DOFade(0, 1).OnComplete(()=>
        {
            Destroy(text.gameObject);
        });
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
        rseAddScoreNotif.action += AddScoreNotif;
    }
    private void OnDisable()
    {
        rse_AddScore.action -= AddScore;
        rseSetStateActive.action -= SetActiveVisual;
        rse_OnGameStart.action -= ResetPeopleAmount;
        rseAddScoreNotif.action -= AddScoreNotif;
    }

    [SerializeField] RSE_BasicEvent rse_OnGameStart;
    void ResetPeopleAmount()
    {
        score = 0;
        rsoScore.Value = 0;
    }
}