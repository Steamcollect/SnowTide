using System;
using System.Collections;
using System.Collections.Generic;
using BT.Save;
using DG.Tweening;
using UnityEngine;

public class PeoplesRender : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] peoplesPlace;
    [SerializeField] private RSO_IntValue rsePeopleAmount;
    [SerializeField] private Transform renderParent;
    private void OnEnable()
    {
        rsePeopleAmount.OnChanged += TreasholdPeopleRender;
    }

    private void OnDisable()
    {
        rsePeopleAmount.OnChanged -= TreasholdPeopleRender;
    }

    private void Start() => TreasholdPeopleRender();
    
    private void TreasholdPeopleRender()
    {
        for (int i = 0; i < peoplesPlace.Length; i++)
        {
            if (i < rsePeopleAmount.Value && rsePeopleAmount.Value > 0)
            {
                if (peoplesPlace[i].activeSelf) continue;
                peoplesPlace[i].SetActive(true);
            }
            else
            {
                peoplesPlace[i].SetActive(false);
            }
        }
        
        renderParent.BumpVisual(1.45f,0.12f);
    }
}
