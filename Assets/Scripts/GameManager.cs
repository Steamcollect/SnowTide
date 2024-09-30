using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RSE_EventBasic rseOnDeath;

    private void OnEnable()
    {
        rseOnDeath.action += OnDeath;
    }

    private void OnDeath() { }
}
