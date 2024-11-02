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

    private void Start()
    {
        StartCoroutine(StopCountingScore(.5f));
    }

    private void Update()
    {
        if (isDrifting && canCountScore)
        {
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

            if (currentDriftTime >= (miniComboTime * currentMiniCombo))
            {
                currentMiniCombo++;
                rse_AddScore.Call(driftScoreGiven + comboScoreGiven * currentBigCombo);
            }
        }
        else
        {
            noDriftDelay += Time.deltaTime;

            if(noDriftDelay >= maxDelayBetweenDrift)
            {
                currentDriftTime = 0;
                currentBigCombo = 0;
                currentMiniCombo = 0;

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
