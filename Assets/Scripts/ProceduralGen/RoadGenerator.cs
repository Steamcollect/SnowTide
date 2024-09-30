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
    [SerializeField] private SettingsRoadGeneration scoRoadGen;
    
    private List<ChunkRoad> _loadedRoadChunks = new List<ChunkRoad>();
    private ChunkRoad _frontChunk = null;
    
    private void Start()
    {
        if (loaderRoadChunk)
        {
            foreach (var chunkT in roadChunkT)
            {
                loaderRoadChunk.LoadChunk(chunkT, scoRoadGen.chunksVisibe);
            }
        }
        if (!roadGm) Debug.LogWarning(message: "No road assigned");
    }

    private void InitializationRoad()
    {
        for (int i = 0; i < scoRoadGen.chunksVisibe; ++i)
        {
            BuildRoad();
        }
    }

    private void BuildRoad()
    {
        var currentChunk = SelectionChunk();
        print(_frontChunk is null);
        currentChunk.transform.position = _frontChunk is null ? Vector3.zero : _frontChunk.anchorEnd.position;
        currentChunk.gameObject.SetActive(true);
        _frontChunk = currentChunk;
    }

    private void OnEnable()
    {
        if (loaderRoadChunk) loaderRoadChunk.OnChunkLoaded += NotifyLoadComplete;
    }

    private void OnDisable()
    {
        if (loaderRoadChunk) loaderRoadChunk.OnChunkLoaded -= NotifyLoadComplete;
    }

    private void NotifyExitChunk(ChunkRoad chunk)
    {
        if (unloaderRoadChunk) unloaderRoadChunk.UnloadChunk(chunk.gameObject);
        BuildRoad();
    }
    
    private void NotifyLoadComplete(GameObject[] chunks)
    {
        foreach (var chunk in chunks)
        {
            chunk.SetActive(false);
            chunk.GetComponent<TriggerExitChunk>().OnChunkExit += NotifyExitChunk;
            _loadedRoadChunks.Add(chunk.GetComponent<ChunkRoad>());
            if (roadGm) chunk.transform.SetParent(roadGm.transform);
        }
        if (_loadedRoadChunks.Count == scoRoadGen.chunksVisibe * roadChunkT.Count) InitializationRoad();
    }
    
    private ChunkRoad SelectionChunk()
    {
        return _loadedRoadChunks.FindAll(o => !o.gameObject.activeSelf).GetRandom();
    }
    
}
