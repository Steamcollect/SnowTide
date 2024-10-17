using System;
using System.Collections.Generic;
using BT.Save;
using UnityEngine;
using UnityEngine.Serialization;

public class RoadGenerator : MonoBehaviour
{
    [Header("Parameter")] 
    [SerializeField] private bool startBuildAutoAtStart;
    
    [Header("Components Generator")]
    [SerializeField] private UnloaderRoadChunk unloaderRoadChunk;
    [SerializeField] private LoaderRoadChunk loaderRoadChunk;

    [Header("References")] 
    [SerializeField] private GameObject roadChunkMenu;
    [SerializeField] private List<GameObject> roadChunkT = new List<GameObject>();
    [SerializeField] private SettingsRoadGeneration scoRoadGen;
    [Space(10)] 
    [SerializeField] private RSE_Event rse_StartBuildAuto;
    [SerializeField] private RSE_Event rse_StopBuildAuto;
    
    private GameObject roadGm;
    private List<ChunkRoad> _loadedRoadChunks = new List<ChunkRoad>();
    private ChunkRoad _frontChunk = null;

    private void Awake()
    {
        roadGm = new GameObject("Road");
        roadGm.transform.parent = transform;
        roadGm.transform.position = Vector3.zero;
    }
    
    private void OnEnable()
    {
        if (loaderRoadChunk) loaderRoadChunk.OnChunkLoaded += NotifyLoadComplete;
        rse_StartBuildAuto.action += StartBuildAuto;
        rse_StopBuildAuto.action += StopBuildAuto;
    }

    private void OnDisable()
    {
        if (loaderRoadChunk) loaderRoadChunk.OnChunkLoaded -= NotifyLoadComplete;
        rse_StartBuildAuto.action -= StartBuildAuto;
        rse_StopBuildAuto.action -= StopBuildAuto;
    }

    private void StopBuildAuto()
    {
        for (int i = 0; i < _loadedRoadChunks.Count; ++i) _loadedRoadChunks[i].gameObject.SetActive(false);
        unloaderRoadChunk.ResetUnloader();
    }

    private void StartBuildAuto()
    {
        InitializationRoad();
    }

    private void Start()
    {
        if (loaderRoadChunk)
        {
            foreach (var chunkT in roadChunkT)
            {
                loaderRoadChunk.LoadChunk(chunkT, scoRoadGen.chunksVisibe + 1);
            }
        }
        if (!roadGm) Debug.LogWarning(message: "No road assigned");
    }

    private void InitializationRoad()
    {
        if (!startBuildAutoAtStart) return;
        _frontChunk = SelectionChunk();
        _frontChunk.transform.position = -_frontChunk.anchorEnd.position;
        _frontChunk.gameObject.SetActive(true);
        unloaderRoadChunk.UnloadChunk(_frontChunk.gameObject);
        
        for (int i = 0; i < scoRoadGen.chunksVisibe; ++i)
        {
            BuildRoad();
        }
    }

    private void BuildRoad()
    {
        var currentChunk = SelectionChunk();
        currentChunk.transform.position = _frontChunk.anchorEnd.position;
        currentChunk.gameObject.SetActive(true);
        _frontChunk = currentChunk;
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
        if (_loadedRoadChunks.Count == (scoRoadGen.chunksVisibe + 1) * roadChunkT.Count) InitializationRoad();
    }
    
    private ChunkRoad SelectionChunk()
    {
        return _loadedRoadChunks.FindAll(o => !o.gameObject.activeSelf).GetRandom();
    }
    
}
