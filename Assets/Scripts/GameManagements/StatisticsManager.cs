using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    [SerializeField] StatisticData[] datas;

    [SerializeField] TMP_Text[] highScoreTxt;

    [Header("RSO")]
    [SerializeField] RSO_ContentSaved rsoContentSaved;

    private void OnEnable()
    {
        for (int i = 0; i < datas.Length; i++)
        {
            datas[i].SetupSlot();
        }

        datas[0].SetValue(rsoContentSaved.Value.gamePlayed);
        datas[1].SetValue(rsoContentSaved.Value.gameTime);
        datas[2].SetValue(rsoContentSaved.Value.totalScore);
        datas[3].SetValue(rsoContentSaved.Value.totalDistanceReach);
        datas[4].SetValue(rsoContentSaved.Value.maxDistanceReach);
        datas[5].SetValue(rsoContentSaved.Value.totalDriftReach);
        datas[6].SetValue(rsoContentSaved.Value.maxDriftReach);
        datas[7].SetValue(rsoContentSaved.Value.totalPeopleSaved);
        datas[8].SetValue(rsoContentSaved.Value.maxPeopleSaved);
        datas[9].SetValue(rsoContentSaved.Value.totalDriftCombo);
        datas[10].SetValue(rsoContentSaved.Value.maxDriftCombo);
        datas[11].SetValue(rsoContentSaved.Value.totalObstacleTouch);
        datas[12].SetValue(rsoContentSaved.Value.maxObstacleTouch);

        for (int i = 0; i < highScoreTxt.Length; i++)
        {
            highScoreTxt[i].text = rsoContentSaved.Value.highscores[i].ToString("#,0");
        }
    }
}

[System.Serializable]
public class StatisticData
{
    public Sprite visual;
    public string statName;
    public float value;

    public StatisticSlot slot;

    public void SetupSlot()
    {
        slot.SetSlot(statName, visual, value);
    }

    public void SetValue(float value)
    {
        this.value = value;
        slot.SetValue(value);
    }
}