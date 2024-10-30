using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

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

    [Header("RSE")]
    [SerializeField] RSE_BasicEvent rseSetupGameOverPanel;

    [Header("RSO")]
    [SerializeField] RSO_IntValue rsoScore;
    [SerializeField] RSO_IntValue rsoMaxCombo;
    [SerializeField] RSO_IntValue rsoPeopleAmount;
    [SerializeField] RSO_IntValue rsoBestScore;

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

        finalScore = score + peopleAmount * peopleScoreGiven;
        bestScore = rsoBestScore.Value;        
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

        SetTextValue(scoreTxt, "Score : ", score);

        targetDelay = GetTargetDelay(score);
        yield return new WaitForSeconds(targetDelay);

        SetTextValue(maxComboTxt, "Max Combo : ", maxCombo);

        targetDelay = GetTargetDelay(maxCombo);
        yield return new WaitForSeconds(targetDelay);

        SetTextValue(peopleAmountTxt, "People Amount : ", peopleAmount);

        targetDelay = GetTargetDelay(peopleAmount);
        yield return new WaitForSeconds(targetDelay);

        SetTextValue(finalScoreTxt, "", finalScore);

        targetDelay = GetTargetDelay(finalScore);
        yield return new WaitForSeconds(targetDelay);

        bestScoreTxt.text = "Best : " + bestScore.ToString("#,0");
        bestScoreTxt.transform.BumpVisual();

        for (int i = 0; i < buttons.Length; i++) buttons[i].SetActive(true);
    }

    void SetTextValue(TMP_Text TXT, string initTxt, int targetValue)
    {
        TXT.text = initTxt + "0";

        int currentValue = 0;

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