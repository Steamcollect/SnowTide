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
    [HideInInspector] public List<Interactible> interactiblesProps { get; private set; }  = new List<Interactible>();
    
    private void OnValidate()
    {
        RecalibrateCollider();
        interactiblesProps.Clear();
        interactiblesProps.AddRange(GetComponentsInChildren<Interactible>());
    }

    public void ResetChunk()
    {
        if (interactiblesProps.Count > 0)
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
