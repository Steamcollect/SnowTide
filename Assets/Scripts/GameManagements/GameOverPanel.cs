using BT.Save;
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    int score;
    int maxCombo;
    int peopleAmount;

    int finalScore;
    int bestScore;

    [SerializeField] float targetTime;
    [SerializeField] float delayBetween;
    [Space(10)]
    [SerializeField] int peopleScoreGiven;

    [Header("References")]
    [SerializeField] TMP_Text scoreTxt;
    [SerializeField] TMP_Text maxComboTxt;
    [SerializeField] TMP_Text peopleAmountTxt;
    [SerializeField] TMP_Text bestScoreTxt;
    [SerializeField] TMP_Text finalScoreTxt;
    [Space(10)]
    [SerializeField] GameObject[] buttons;
    [Space(10)]
    [SerializeField] Transform peopleCountPos;
    [SerializeField] GameObject scoreNotifPrefab;
    [SerializeField] Color scoreNotifColor;

    [Header("RSE")]
    [SerializeField] RSE_BasicEvent rseSetupGameOverPanel;

    [Header("RSO")]
    [SerializeField] RSO_IntValue rsoScore;
    [SerializeField] RSO_IntValue rsoMaxCombo;
    [SerializeField] RSO_IntValue rsoPeopleAmount;
    [SerializeField] RSO_IntValue rsoObstacleTouch;
    [SerializeField] RSO_ContentSaved rsoContentSave;

    private void OnEnable()
    {
        rseSetupGameOverPanel.action += SetupPanel;
    }
    private void OnDisable()
    {
        rseSetupGameOverPanel.action -= SetupPanel;
    }

    void SetupPanel()
    {
        GetValues();
        StartCoroutine(SetText());
    }

    void GetValues()
    {
        score = rsoScore.Value;
        maxCombo = rsoMaxCombo.Value;
        peopleAmount = rsoPeopleAmount.Value;
        print($"{score},{maxCombo},{peopleAmount}");
        finalScore = score + peopleAmount * peopleScoreGiven;

        rsoContentSave.Value.totalScore += score;
        rsoContentSave.Value.totalPeopleSaved += peopleAmount;
        if(rsoContentSave.Value.maxPeopleSaved < peopleAmount) rsoContentSave.Value.maxPeopleSaved = peopleAmount;
        rsoContentSave.Value.totalDriftCombo += maxCombo;
        if(rsoContentSave.Value.maxDriftCombo < maxCombo) rsoContentSave.Value.maxDriftCombo = maxCombo;
        rsoContentSave.Value.totalObstacleTouch += rsoObstacleTouch.Value;
        if(rsoObstacleTouch.Value > rsoContentSave.Value.maxObstacleTouch) rsoContentSave.Value.maxObstacleTouch = rsoObstacleTouch.Value;
        rsoContentSave.Value.gamePlayed += 1;

        rsoContentSave.Value.AddScore(finalScore);
        bestScore = rsoContentSave.Value.highscores[0];
    }

    IEnumerator SetText()
    {
        scoreTxt.text = "Score : 0";
        maxComboTxt.text = "Max Combo : 0";
        peopleAmountTxt.text = "People Amount : 0";
        finalScoreTxt.text = "0";
        bestScoreTxt.text = "Best : 0";
        
        for (int i = 0; i < buttons.Length; i++) buttons[i].SetActive(false);

        float targetDelay = 0;

        SetTextValue(scoreTxt, "Score : ", score, false);

        targetDelay = GetTargetDelay(score);
        yield return new WaitForSeconds(targetDelay);

        SetTextValue(maxComboTxt, "Max Combo : ", maxCombo, false);

        targetDelay = GetTargetDelay(maxCombo);
        yield return new WaitForSeconds(targetDelay);

        SetTextValue(peopleAmountTxt, "People Amount : ", peopleAmount, false);

        targetDelay = GetTargetDelay(peopleAmount);
        yield return new WaitForSeconds(targetDelay);

        SetTextValue(finalScoreTxt, "", score, false);
        yield return new WaitForSeconds (targetDelay + .5f);

       if(peopleAmount > 0)
        {
            TMP_Text text = Instantiate(scoreNotifPrefab, transform).GetComponent<TMP_Text>();
            text.transform.BumpVisual();
            text.fontSize = finalScoreTxt.fontSize * .65f;
            text.color = scoreNotifColor;
            text.transform.position = peopleCountPos.position;
            text.transform.rotation = Quaternion.identity;
            text.text = "+" + peopleAmount.ToString("#,0") + " people saved";

            yield return new WaitForSeconds(.7f);

            bool canContinue = false;
            text.transform.DOMove(finalScoreTxt.transform.position, .2f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                canContinue = true;
                Destroy(text.gameObject);
                finalScoreTxt.transform.BumpVisual();
            });

            yield return new WaitUntil(() => canContinue);
        }

        SetTextValue(finalScoreTxt, "", finalScore, true);

        targetDelay = GetTargetDelay(finalScore);
        yield return new WaitForSeconds(targetDelay);

        bestScoreTxt.text = "Best : " + bestScore.ToString("#,0");
        bestScoreTxt.transform.BumpVisual();

        for (int i = 0; i < buttons.Length; i++) buttons[i].SetActive(true);
    }

    void SetTextValue(TMP_Text TXT, string initTxt, int targetValue, bool isFinalScore)
    {
        TXT.text = initTxt + "0";

        int currentValue = 0;
        if (isFinalScore) currentValue = score;

        DOTween.To(() => currentValue, x => currentValue = x, targetValue, targetTime).SetEase(Ease.OutExpo)
            .OnUpdate(() =>
            {
                string text = currentValue.ToString("#,0");
                TXT.text = initTxt + text;
            })
            .OnComplete(() =>
            {
                string text = currentValue.ToString("#,0");
                TXT.text = initTxt + text;
            });
    }

    float GetTargetDelay(int value)
    {
        if (value < 10) return targetTime / 10;
        else return targetTime;
    }
}