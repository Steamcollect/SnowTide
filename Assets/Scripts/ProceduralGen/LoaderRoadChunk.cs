using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderRoadChunk : MonoBehaviour
{
    public void LoadChunk(GameObject chunk, int quantity, Action<GameObject[]> callback)
    {
        StartCoroutine(LoadChunkAsync(chunk, quantity, callback));
    }

    private IEnumerator LoadChunkAsync(GameObject chunk, int quantity, Action<GameObject[]> callback = null)
    {
        var instantiateAsync = InstantiateAsync(chunk, quantity);
        yield return instantiateAsync;
        if (instantiateAsync.Result != null) callback?.Invoke(instantiateAsync.Result);
        
    }
}
