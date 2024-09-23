using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderRoadChunk : MonoBehaviour
{
    public event Action<GameObject> OnChunkLoaded;

    public void LoadChunk(GameObject chunk, Transform parent)
    {
        StartCoroutine(LoadChunkAsync(chunk, parent));
        var obj = InstantiateAsync(chunk, parent);
    }

    private IEnumerator LoadChunkAsync(GameObject chunk, Transform parent)
    {
        yield return InstantiateAsync(chunk, parent);
        
    }
}
