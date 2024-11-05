using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreReferences : MonoBehaviour
{
    [SerializeField] Transform scorePosition;
    [SerializeField] Transform currentScorePosition;
    [SerializeField] Transform scoreCanvas;

    [SerializeField] RSO_ScorePosition rsoScorePosition;
    [SerializeField] RSO_CurrentScorePosition rsoCurrentScorePosition;
    [SerializeField] RSO_ScoreCanvas rsoScoreCanvas;

    private void Start()
    {
        rsoScorePosition.Value = scorePosition;
        rsoCurrentScorePosition.Value = currentScorePosition;
        rsoScoreCanvas.Value = scoreCanvas;
    }
}