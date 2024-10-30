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
            if (i < rsePeopleAmount.Value)
            {
                if (peoplesPlace[i].activeSelf) continue;
                peoplesPlace[i].SetActive(true);
            }
            else
            {
                peoplesPlace[i].SetActive(false);
            }
        }
        
        transform.DOPunchScale(Vector3.one * 0.8f, 0.2f);
    }
}
