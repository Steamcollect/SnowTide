using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoadGenerator : MonoBehaviour
{
    [Header("Components Generator")]
    [SerializeField] private UnloaderRoadChunk unloaderRoadChunk;
    [SerializeField] private LoaderRoadChunk loaderRoadChunk;

    [Header("References")]
    [SerializeField] private GameObject roadGm;
    [SerializeField] private List<GameObject> roadChunkT = new List<GameObject>();
    private List<GameObject> loadedRoadChunks = new List<GameObject>();

    [Header("Parameter")]
    [SerializeField] [Min(1)] private int chunkVisible;
    
    private void Start()
    {
        if (loaderRoadChunk)
        {
            foreach (var chunkT in roadChunkT)
            {
                loaderRoadChunk.LoadChunk(chunkT, chunkVisible);
            }
        }
        if (!roadGm) Debug.LogWarning(message: "No road assigned");
    }

    private void InitializationRoad()
    {
        GameObject chunkRoad;
        Vector3 forward = roadGm? roadGm.transform.forward : Vector3.forward;
        for (int i = 0; i < chunkVisible; ++i)
        {
            chunkRoad = SelectionChunk();
            chunkRoad.transform.position = new Vector3(ChunkRoad.Sizechunk.x * i * forward.x,0,ChunkRoad.Sizechunk.z * i * forward.z);
            chunkRoad.SetActive(true);
        }
    }

    private void BuildRoad(){}
    
    private void AnalyzeSateChunk(GameObject chunk)
    {
        if (unloaderRoadChunk) unloaderRoadChunk.UnloadChunk(chunk);
    }

    private void OnEnable()
    {
        if (loaderRoadChunk) loaderRoadChunk.OnChunkLoaded += NotifyLoadComplete;
    }

    private void OnDisable()
    {
        if (loaderRoadChunk) loaderRoadChunk.OnChunkLoaded -= NotifyLoadComplete;
    }

    private void NotifyLoadComplete(GameObject[] chunks)
    {
        foreach (var chunk in chunks)
        {
            chunk.SetActive(false);
            chunk.GetComponent<TriggerExitChunk>().OnChunkExit += AnalyzeSateChunk;
            loadedRoadChunks.Add(chunk);
            if (roadGm) chunk.transform.SetParent(roadGm.transform);
        }
        if (loadedRoadChunks.Count == chunkVisible * roadChunkT.Count) InitializationRoad();
    }
    
    private GameObject SelectionChunk()
    {
        return loadedRoadChunks.FindAll(o => !o.activeSelf).GetRandom();
    }
    
}
