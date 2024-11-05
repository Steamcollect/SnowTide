using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeDamageCrack : MonoBehaviour
{
    [SerializeField] Image[] cracks;
    [SerializeField] RSO_TakeDamageCrack rsoTakeDamageCrack;
    [SerializeField] RSE_BasicEvent rseOnGameStart;

    private void Start()
    {
        rsoTakeDamageCrack.Value = cracks;
    }

    private void OnEnable()
    {
        rseOnGameStart.action += OnGameStart;
    }
    private void OnDisable()
    {
        rseOnGameStart.action -= OnGameStart;
    }

    void OnGameStart()
    {
        for (int i = 0; i < cracks.Length; i++)
        {
            cracks[i].color = new Color(1, 1, 1, 0);
        }
    }
}