using System;
using System.Collections;
using System.Collections.Generic;
using BT.Save;
using UnityEngine;
using UnityEngine.Serialization;

public class AvalancheFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private RSO_Life rsoLife;
    
    [Header("Parameters")]
    [SerializeField] private Vector3 hidenPos, showPos, destroyPos;
    [SerializeField] private float timeOffset;
    
    private Vector3 posOffset;
    private Vector3 velocity = Vector3.zero;


    private void OnEnable()
    {
        rsoLife.OnChanged += ChangePosOffset;
    }

    private void OnDisable()
    {
        rsoLife.OnChanged -= ChangePosOffset;
    }


    private void ChangePosOffset()
    {
        if (rsoLife.Value.health <= 0) Bury();
        else if (!rsoLife.Value.isRegen) Show();
        else if(rsoLife.Value.health >= rsoLife.Value.maxHealth /2) Hide();
        print(posOffset);
    }
    
    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position + posOffset, ref velocity, timeOffset);
    }

    private void Hide() => posOffset = hidenPos;
    private void Show() => posOffset = showPos;
    private void Bury() => posOffset = destroyPos;

    private void OnDrawGizmosSelected()
    {
        if (!target) return;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target.position + hidenPos, .5f);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(target.position + showPos, .3f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target.position + destroyPos, .3f);
    }
}