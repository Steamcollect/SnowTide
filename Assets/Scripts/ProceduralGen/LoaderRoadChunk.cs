using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderRoadChunk : MonoBehaviour
{
    public void LoadChunk(GameObject chunk, int quantity,Vector3 position,Action<GameObject[]> callback)
    {
        StartCoroutine(LoadChunkAsync(chunk, quantity,position, callback));
    }

    private IEnumerator LoadChunkAsync(GameObject chunk, int quantity,Vector3 position, Action<GameObject[]> callback = null)
    {
        var instantiateAsync = InstantiateAsync(chunk, quantity,position,Quaternion.identity);
        yield return instantiateAsync;
        if (instantiateAsync.Result != null) callback?.Invoke(instantiateAsync.Result);
        
    }
}
