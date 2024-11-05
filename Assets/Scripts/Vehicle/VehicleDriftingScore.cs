using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class VehicleDriftingScore : MonoBehaviour
{
    [Header("Drift score")]
    [SerializeField] int driftScoreGiven;
    [SerializeField] int comboScoreGiven;
    [SerializeField] float miniComboTime;
    [SerializeField] float bigComboTime;
    float currentDriftTime;
    int currentMiniCombo, currentBigCombo;
    int maxCombo = 0;

    [Space(10)]
    float noDriftDelay;
    [SerializeField] float maxDelayBetweenDrift;

    bool isDrifting = false;
    bool canCountScore = false;

    [Header("References")]
    [SerializeField] TMP_Text comboCountTxt;
    [SerializeField] RSE_IntEvent rse_AddScore;
    [SerializeField] RSO_IntValue rsoComboAmount;
    [Space(10)]
    [SerializeField] GameObject currentScorePrefab;
    [SerializeField] RSO_ScoreCanvas rsoScoreCanvas;
    [SerializeField] Vector2 currentScorePosOffset;
    [SerializeField] RSO_ScorePosition rsoScorePosition;

    int currentScore;
    TMP_Text currentScoreTxt;
    bool isScoreReset = false;

    private void Start()
    {
        StartCoroutine(StopCountingScore(.5f));
    }

    private void Update()
    {
        if (isDrifting && canCountScore)
        {
            isScoreReset = false;
            currentDriftTime += Time.deltaTime;
            noDriftDelay = 0;

            if (currentDriftTime >= (bigComboTime * currentBigCombo))
            {
                currentBigCombo++;
                if(maxCombo < currentBigCombo)
                {
                    maxCombo = currentBigCombo;
                    rsoComboAmount.Value = maxCombo;
                }

                comboCountTxt.DOKill();
                comboCountTxt.DOFade(1, .1f);
                comboCountTxt.text = "Combo x" + currentBigCombo;
                comboCountTxt.transform.BumpVisual();
            }

            if(currentScoreTxt != null)
            {
                currentScoreTxt.transform.position = (Vector2)Camera.main.WorldToScreenPoint(transform.position) + currentScorePosOffset;
            }

            if (currentDriftTime >= (miniComboTime * currentMiniCombo))
            {
                if(currentScoreTxt == null)
                {
                    currentScoreTxt = Instantiate(currentScorePrefab, rsoScoreCanvas.Value).GetComponent<TMP_Text>();
                    currentScoreTxt.transform.rotation = Quaternion.Euler(0, 0, 15);
                }

                currentMiniCombo++;
                currentScore += driftScoreGiven + (comboScoreGiven * currentBigCombo);
                currentScoreTxt.text = "+" + currentScore.ToString("#,0");
                currentScoreTxt.transform.BumpVisual();
            }
        }
        else
        {
            noDriftDelay += Time.deltaTime;

            if(noDriftDelay >= maxDelayBetweenDrift && !isScoreReset)
            {
                isScoreReset = true;

                GameObject go = currentScoreTxt.gameObject;
                int cScore = currentScore;
                currentScoreTxt = null;
                go.transform.DOMove(rsoScorePosition.Value.position, .25f).SetEase(Ease.InOutCubic).OnComplete(()=>
                {
                    rse_AddScore.Call(cScore);
                    Destroy(go);
                });

                currentDriftTime = 0;
                currentBigCombo = 0;
                currentMiniCombo = 0;
                currentScore = 0;

                comboCountTxt.DOKill();
                comboCountTxt.DOFade(0, .1f);
            }
        }
    }

    public void SetDriftState(bool _isDrifting) => isDrifting = _isDrifting;

    // Attendre un peu que la voiture atteigne une vitesse minimum
    IEnumerator StopCountingScore(float delay)
    {
        canCountScore = false;
        yield return new WaitForSeconds(delay);
        canCountScore = true;
    }

    [SerializeField] RSE_BasicEvent rse_OnGameStart;
    private void OnEnable()
    {
        rse_OnGameStart.action += ResetPeopleAmount;
    }
    private void OnDisable()
    {
        rse_OnGameStart.action -= ResetPeopleAmount;
    }

    void ResetPeopleAmount()
    {
        maxCombo = 0;
        rsoComboAmount.Value = 0;
    }
}
