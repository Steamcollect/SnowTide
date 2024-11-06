using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatisticSlot : MonoBehaviour
{
    [SerializeField] Image statVisual;
    [SerializeField] TMP_Text statNameTxt;
    [SerializeField] TMP_Text valueTxt;

    [SerializeField] bool isTime = false;
    [SerializeField] bool isDistance = false;

    public void SetSlot(string slotName, Sprite icon, float value)
    {
        statNameTxt.text = slotName;
        if(icon) statVisual.sprite = icon;
        if (isTime) valueTxt.text = value.ToString();
        else if (isDistance) valueTxt.text = value.ToString() + "m";
        else valueTxt.text = value.ToString();
    }

    public void SetValue(float value)
    {
        valueTxt.text = value.ToString();
    }
}