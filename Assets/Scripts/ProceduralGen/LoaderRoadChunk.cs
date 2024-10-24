using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderRoadChunk : MonoBehaviour
{

    public void LoadChunk(GameObject chunk, int quantity, Action<GameObject[]> ev = null)
    {
        StartCoroutine(LoadChunkAsync(chunk, quantity, ev));
    }

    private IEnumerator LoadChunkAsync(GameObject chunk, int quantity, Action<GameObject[]> ev)
    {
        var instantiateAsync = InstantiateAsync(chunk, quantity);
        yield return instantiateAsync;
        if (instantiateAsync.Result != null)
        {
            ev?.Invoke(instantiateAsync.Result);
        }
        
    }
}
