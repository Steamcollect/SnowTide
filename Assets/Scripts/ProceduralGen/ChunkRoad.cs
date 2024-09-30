using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChunkRoad : MonoBehaviour
{
    [field:Header("Components")]
    [field:SerializeField] public Transform anchorEnd { get; private set; }
    [SerializeField] private new BoxCollider collider;

    [field:Header("Settings")]
    [field:SerializeField] public Vector3 sizeColliderExit { get; private set; } = new Vector3(1, 1, 1);

    private void OnValidate()
    {
        RecalibrateCollider();
    }

    private void RecalibrateCollider()
    {
        if (collider == null) return;
        collider.center = anchorEnd.position + Vector3.up * sizeColliderExit.y/2;
        collider.size = sizeColliderExit;
    }
}
