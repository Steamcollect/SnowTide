using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreReferences : MonoBehaviour
{
    [SerializeField] Transform scorePosition;
    [SerializeField] Transform scoreCanvas;

    [SerializeField] RSO_ScorePosition rsoScorePosition;
    [SerializeField] RSO_ScoreCanvas rsoScoreCanvas;

    private void Start()
    {
        rsoScorePosition.Value = scorePosition;
        rsoScoreCanvas.Value = scoreCanvas;
    }
}