using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChunkRoad : MonoBehaviour
{
    [field:Header("Components")]
    [field:SerializeField] public Transform _anchorStart { get; private set; }
    [field:SerializeField] public Transform _anchorEnd { get; private set; }
    [SerializeField] private new BoxCollider collider;
    [SerializeField] private GameObject mesh;

    [Header("References")]
    [SerializeField] private SettingsRoadGeneration scoRoadGen;

    private void OnValidate()
    { 
        if (scoRoadGen == null) return;
        RecalibrateMesh();
        RecalibrateCollider();
    }

    private void RecalibrateMesh()
    {
        if (mesh == null) return;
        mesh.transform.localScale = new Vector3(scoRoadGen.sizeChunk.x, 0.1f,scoRoadGen.sizeChunk.x);
        mesh.transform.localPosition = new Vector3(0,0.05f,0);
    }

    private void RecalibrateCollider()
    {
        if (collider == null) return;
        collider.center = new Vector3(0,scoRoadGen.sizeChunk.y/2,scoRoadGen.sizeChunk.z/2);
        collider.size = new Vector3(scoRoadGen.sizeChunk.x, scoRoadGen.sizeChunk.y, 0.1f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position + new Vector3(0,scoRoadGen.sizeChunk.y/2,0), scoRoadGen.sizeChunk);
    }
}
