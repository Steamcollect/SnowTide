using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChunkRoad : MonoBehaviour
{
    [field:Header("Components")]
    [field:SerializeField] public Transform anchorStart { get; private set; }
    [field:SerializeField] public Transform anchorEnd { get; private set; }
    [SerializeField] private new BoxCollider collider;
    [SerializeField] private GameObject mesh;

    [Header("Settings")]
    [SerializeField] private SettingsRoadGeneration scoRoadGen;
    [field:SerializeField] public Vector3 sizeChunk { get; private set; } = new Vector3(1, 1, 1);
    [SerializeField] private bool automaticMoveAnchor = false;

    private void OnValidate()
    { 
        if (scoRoadGen == null) return;
        RecalibrateMesh();
        RecalibrateCollider();
        RecalibrateAnchors();
    }

    private void RecalibrateMesh()
    {
        if (mesh != null)
        {
            mesh.transform.localScale = new Vector3(sizeChunk.x, 0.1f,sizeChunk.z);
            mesh.transform.localPosition = new Vector3(0,0.05f,0);
        }
    }

    private void RecalibrateCollider()
    {
        if (collider == null) return;
        collider.center = new Vector3(0,sizeChunk.y/2,sizeChunk.z/2);
        collider.size = new Vector3(sizeChunk.x, sizeChunk.y, 0.2f);
    }

    private void RecalibrateAnchors()
    {
        if(!automaticMoveAnchor) return;
        anchorEnd.position = new Vector3(0, 0, sizeChunk.z / 2);
        anchorStart.position = new Vector3(0, 0, -sizeChunk.z / 2);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position + new Vector3(0,sizeChunk.y/2,0), sizeChunk);
    }
}
