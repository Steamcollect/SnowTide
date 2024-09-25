using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderRoadChunk : MonoBehaviour
{
    public event Action<GameObject[]> OnChunkLoaded;

    public void LoadChunk(GameObject chunk, int quantity)
    {
        StartCoroutine(LoadChunkAsync(chunk, quantity));
    }

    private IEnumerator LoadChunkAsync(GameObject chunk, int quantity)
    {
        var instantiateAsync = InstantiateAsync(chunk, quantity);
        yield return instantiateAsync;
        if (instantiateAsync.Result != null) OnChunkLoaded?.Invoke(instantiateAsync.Result);
        
    }
}
