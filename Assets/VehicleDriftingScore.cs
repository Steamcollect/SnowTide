using System.Collections;
using System.Collections.Generic;
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

    bool isDrifting = false;

    [SerializeField] RSE_IntEvent rse_AddScore;

    private void Update()
    {
        if (isDrifting)
        {
            currentDriftTime += Time.deltaTime;

            if (currentDriftTime >= (bigComboTime * currentBigCombo))
            {
                currentBigCombo++;
            }

            if (currentDriftTime >= (miniComboTime * currentDriftTime))
            {
                miniComboTime++;
                rse_AddScore.Call(driftScoreGiven + comboScoreGiven * currentBigCombo);
            }
        }
        else
        {
            currentDriftTime = 0;
            currentBigCombo = 0;
            currentMiniCombo = 0;
        }        
    }

    public void SetDriftState(bool _isDrifting) => isDrifting = _isDrifting;
}
