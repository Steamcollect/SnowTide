using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalancheFollow : MonoBehaviour
{
    public Transform player;

    public Vector3 hidenPos, showPos, destroyPos;
    Vector3 posOffset;

    public float timeOffset;
    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.position + posOffset, ref velocity, timeOffset);
    }

    public void Hide() => posOffset = hidenPos;
    public void Show() => posOffset = showPos;
    public void Bury() => posOffset = destroyPos;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(player.position + hidenPos, .3f);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(player.position + showPos, .3f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(player.position + destroyPos, .3f);
    }
}