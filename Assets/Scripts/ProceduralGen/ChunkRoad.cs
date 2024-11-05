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
    [SerializeField] private GameObject interactablesParent;
    
    [field:Header("Settings")]
    [field:SerializeField] public Vector3 sizeColliderExit { get; private set; } = new Vector3(1, 1, 1);

    private List<Interactible> interactiblesProps  = new List<Interactible>();
    [HideInInspector] public bool chunkActive = true; 
    
    
    private void OnValidate()
    {
        RecalibrateCollider();
        interactiblesProps.Clear();
        interactiblesProps.AddRange(interactablesParent.GetComponentsInChildren<Interactible>());
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
