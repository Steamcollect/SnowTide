using BT.Save;
using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
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

    Vector3 startDirftPosition;


    [Header("References")]
    [SerializeField] TMP_Text comboCountTxt;
    [SerializeField] RSE_IntEvent rse_AddScore;
    [SerializeField] RSO_IntValue rsoComboAmount;
    [Space(10)]
    [SerializeField] GameObject currentScorePrefab;
    [SerializeField] RSO_ScoreCanvas rsoScoreCanvas;
    [SerializeField] Vector2 currentScorePosOffset;
    [SerializeField] RSO_ScorePosition rsoScorePosition;
    [SerializeField] RSO_ComboPosition rsoComboPosition;
    [SerializeField] Vector2 comboPosOffset;
    [Space(10)]
    [SerializeField] RSE_BasicEvent rseOnGameStart;
    [SerializeField] RSE_Event rseOnPlayerDeath;
    [Space(10)]
    [SerializeField] RSO_IntValue diftDistance;
    [SerializeField] RSO_ContentSaved rsoContentSaved;
    [Space(10)]
    [SerializeField] RSE_BasicEvent rseResetScore;
    [SerializeField] RSE_PauseGame rsePauseGame;

    int currentScore;
    TMP_Text currentScoreTxt;
    bool isScoreReset = false;
    bool isPaused = false;

    void OnGamePaused(bool paused)
    {
        isPaused = paused;
    }

    private void Start()
    {
        StartCoroutine(StopCountingScore(.5f));
    }

    private void Update()
    {
        if (isPaused) return;

        if (isDrifting && canCountScore)
        {
            isScoreReset = false;
            currentDriftTime += Time.deltaTime;
            noDriftDelay = 0;
            if(currentDriftTime < .2f)
            {
                startDirftPosition = transform.position;
            }


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
                ResetScore();
            }
        }

        rsoComboPosition.Value.position = (Vector2)Camera.main.WorldToScreenPoint(transform.position) + comboPosOffset;
        if (currentScoreTxt != null)
        {
            currentScoreTxt.transform.position = (Vector2)Camera.main.WorldToScreenPoint(transform.position) + currentScorePosOffset;
        }
    }

    void ResetScore()
    {
        isScoreReset = true;

        if (currentScoreTxt != null)
        {
            GameObject go = currentScoreTxt.gameObject;
            int cScore = currentScore;
            currentScoreTxt = null;
            go.transform.DOMove(rsoScorePosition.Value.position, .25f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                rse_AddScore.Call(cScore);
                Destroy(go);
            });
        }

        currentDriftTime = 0;
        currentBigCombo = 0;
        currentMiniCombo = 0;
        currentScore = 0;

        comboCountTxt.DOKill();
        comboCountTxt.DOFade(0, .1f);

        int driftDist = (int)Vector3.Distance(startDirftPosition, transform.position);
        rsoContentSaved.Value.totalDriftReach += driftDist;
        if (driftDist > rsoContentSaved.Value.maxDriftReach) rsoContentSaved.Value.maxDriftReach = driftDist;
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
        rseOnPlayerDeath.action += OnPlayerDeath;
        rseOnGameStart.action += OnGameStart;
        rseResetScore.action += ResetScore;
        rsePauseGame.action += OnGamePaused;
    }
    private void OnDisable()
    {
        rse_OnGameStart.action -= ResetPeopleAmount;
        rseOnPlayerDeath.action -= OnPlayerDeath;
        rseOnGameStart.action -= OnGameStart;
        rseResetScore.action -= ResetScore;
        rsePauseGame.action -= OnGamePaused;
    }

    void ResetPeopleAmount()
    {
        maxCombo = 0;
        rsoComboAmount.Value = 0;
    }
    void OnPlayerDeath()
    {
        canCountScore = false;
        rse_AddScore.Call(currentScore);
        currentScore = 0;
    }
    void OnGameStart()
    {
        StartCoroutine(StopCountingScore(.5f));
    }
}
