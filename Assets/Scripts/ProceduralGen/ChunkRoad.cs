using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ChunkRoad : MonoBehaviour
{
    [field:Header("Components")]
    [field:SerializeField] public Transform anchorEnd { get; private set; }
    [SerializeField] private new BoxCollider collider;
    [SerializeField] private GameObject interactablesParent;
    
    [field:Header("Settings")]
    [field:SerializeField] public Vector3 sizeColliderExit { get; private set; } = new Vector3(1, 1, 1);

    private List<Interactible> interactiblesProps  = new ();
    [HideInInspector] public bool chunkActive = true; 
    
    
    private void OnValidate()
    {
        RecalibrateCollider();
    }
    
    private void Awake()
    {
        interactiblesProps.Clear();
        interactiblesProps =interactablesParent.GetComponentsInChildren<Interactible>(true).ToList();
    }

    public void ResetChunk()
    {
        chunkActive = true;
        if (interactiblesProps.Count <= 0) return;
        foreach (Interactible interactibleProp in interactiblesProps)
        {
            interactibleProp.ResetComponent();
        }
    }

    private void RecalibrateCollider()
    {
        if (collider == null) return;
        collider.center = anchorEnd.localPosition + Vector3.up * sizeColliderExit.y/2;
        collider.size = sizeColliderExit;
    }
}
