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
    [SerializeField] private GameObject roadChunkMenuT;
    [SerializeField] private List<GameObject> roadChunkT = new ();
    [SerializeField] private SettingsRoadGeneration scoRoadGen;
    [Space(10)] 
    [SerializeField] private RSE_Event rse_StartBuildAuto;
    [SerializeField] private RSE_Event rse_StopBuildAuto;
    [SerializeField] private RSE_SwapChunk rse_SwapChunk;
    
    private GameObject roadGm;
    private List<ChunkRoad> _loadedRoadChunks = new();
    private List<ChunkRoad> _loadedMenuChunk = new();
    private ChunkRoad _frontChunk;
    private List<ChunkRoad> _currentsChunks = new();
    
    
    private void Awake()
    {
        roadGm = new GameObject("Road")
        {
            transform =
            {
                parent = transform,
                position = Vector3.zero
            }
        };
        SwapChunkSelection(GameState.Menu);
    }
    
    private void OnEnable()
    {
        rse_StartBuildAuto.action += StartBuildAuto;
        rse_StopBuildAuto.action += StopBuildAuto;
        rse_SwapChunk.action += SwapChunkSelection;
    }

    private void OnDisable()
    {
        rse_StartBuildAuto.action -= StartBuildAuto;
        rse_StopBuildAuto.action -= StopBuildAuto;
        rse_SwapChunk.action -= SwapChunkSelection;
    }

    private void StopBuildAuto()
    {
        for (int i = 0; i < _loadedRoadChunks.Count; ++i)
        {
            unloaderRoadChunk.DisableChunk(_loadedRoadChunks[i]);
        }

        for (int i = 0; i < _loadedMenuChunk.Count; ++i)
        {
            unloaderRoadChunk.DisableChunk(_loadedMenuChunk[i]);
        }
        unloaderRoadChunk.ResetUnloader();
    }

    private void StartBuildAuto()
    {
        InitializationRoad();
    }

    private void Start()
    {
        
        loaderRoadChunk.LoadChunk(roadChunkMenuT, scoRoadGen.chunksVisibe + 1,chunks=>NotifyLoadComplete(chunks,_loadedMenuChunk));
        
        foreach (var chunkT in roadChunkT)
        {
            loaderRoadChunk.LoadChunk(chunkT, scoRoadGen.chunksVisibe + 1,chunks=>NotifyLoadComplete(chunks,_loadedRoadChunks));
        }
    }

    private void InitializationRoad()
    {
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
        unloaderRoadChunk.UnloadChunk(chunk.gameObject);
        BuildRoad();
    }
    
    private void NotifyLoadComplete(GameObject[] chunks, List<ChunkRoad> targetSampler)
    {
        foreach (var chunk in chunks)
        {
            chunk.SetActive(false);
            chunk.GetComponent<TriggerExitChunk>().OnChunkExit += NotifyExitChunk;
            targetSampler.Add(chunk.GetComponent<ChunkRoad>());
            chunk.transform.SetParent(roadGm.transform);
        }
        if (targetSampler == _loadedMenuChunk && startBuildAutoAtStart) InitializationRoad();
    }
    
    private ChunkRoad SelectionChunk()
    {
        return _currentsChunks.FindAll(o => !o.gameObject.activeSelf).GetRandom();
    }

    private void SwapChunkSelection(GameState gameState)
    {
        _currentsChunks = gameState switch
        {
            GameState.Road => _loadedRoadChunks,
            GameState.Menu => _loadedMenuChunk,
            _ => _currentsChunks
        };
    }
    
}

[Serializable]
public enum GameState
{
    Menu,
    Road
}
