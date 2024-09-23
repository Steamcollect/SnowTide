using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneratorRoad : MonoBehaviour
{
    [Header("Components Generator")]
    [SerializeField] private UnloaderRoadChunk unloaderRoadChunk;
    [SerializeField] private LoaderRoadChunk loaderRoadChunk;

    [Header("References")]
    [SerializeField] private GameObject road;
    [SerializeField] private HashSet<GameObject> chunksTemplate;
    [SerializeField] private List<GameObject> loadedChunks;

    [Header("Parameter")]
    [SerializeField] private float distanceShow;

    private void Start()
    {
        foreach (GameObject chunkT in chunksTemplate)
        {
            loaderRoadChunk.LoadChunk(chunkT, transform);
        }
    }

    private void OnEnable()
    {
        unloaderRoadChunk.OnChunkUnloaded += NotifyUnloadComplete;
        loaderRoadChunk.OnChunkLoaded += NotifyLoadComplete;
    }

    private void OnDisable()
    {
        unloaderRoadChunk.OnChunkUnloaded -= NotifyUnloadComplete;
        loaderRoadChunk.OnChunkLoaded -= NotifyLoadComplete;
    }
    
    private void NotifyLoadComplete(GameObject chunk){}
    private void NotifyUnloadComplete(GameObject chunk){}
}
