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
            BuildRoad(i);
        }
    }

    private void BuildRoad(Vector3 referencePosition)
    {
        ChunkRoad chunkRoad = SelectionChunk();
        chunkRoad.transform.position = referencePosition + new Vector3( scoRoadGen.sizeChunk.x * Vector3.forward.x * scoRoadGen.chunksVisibe, 0, scoRoadGen.sizeChunk.z * Vector3.forward.z * scoRoadGen.chunksVisibe);
        chunkRoad.gameObject.SetActive(true);
    }

    private void BuildRoad(int indexReferencePos)
    {
        ChunkRoad chunkRoad = SelectionChunk();
        chunkRoad.transform.position = new Vector3(scoRoadGen.sizeChunk.x * Vector3.forward.x * indexReferencePos, 0, scoRoadGen.sizeChunk.z * Vector3.forward.z *indexReferencePos);
        chunkRoad.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (loaderRoadChunk) loaderRoadChunk.OnChunkLoaded += NotifyLoadComplete;
    }

    private void OnDisable()
    {
        if (loaderRoadChunk) loaderRoadChunk.OnChunkLoaded -= NotifyLoadComplete;
    }

    private void NotifyExitChunk(GameObject chunk)
    {
        if (unloaderRoadChunk) unloaderRoadChunk.UnloadChunk(chunk);
        BuildRoad(chunk.transform.position);
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
