using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExitChunk : MonoBehaviour
{
    public event Action<ChunkRoad> OnChunkExit;
    [SerializeField] private ChunkRoad chunk;
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnChunkExit?.Invoke(chunk);
        }
    }
}
