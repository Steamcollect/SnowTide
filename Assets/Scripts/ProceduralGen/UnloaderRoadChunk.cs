using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloaderRoadChunk : MonoBehaviour
{
    public event Action<GameObject> OnChunkUnloaded;
    
    public void UnloadChunk(GameObject chunk) { throw new NotImplementedException(); }
    
    private IEnumerator UnloadLoadChunkAsync(GameObject chunk)
    {
        yield return InstantiateAsync(chunk);
    }
}
