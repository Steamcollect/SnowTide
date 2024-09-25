using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExitChunk : MonoBehaviour
{
    public event Action<GameObject> OnChunkExit;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnChunkExit?.Invoke(gameObject);
        }
    }
}
