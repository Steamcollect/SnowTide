using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int score;

    [Header("References")]
    [SerializeField] TMP_Text scoreTxt;

    public void AddScore()
    {

    }

    void SetTextVisual()
    {
        scoreTxt.text = score.ToString();
    }
}
