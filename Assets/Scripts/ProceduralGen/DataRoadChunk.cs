using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRoadChunk : MonoBehaviour
{
    [Header("System")]
    [SerializeField] private LoaderRoadChunk loaderRoadChunk;
    [SerializeField] private SettingsRoadGeneration settingsRoadGen;
    
    [Header("References")]
    [SerializeField] private List<GameObject> chunkRoadT;
    [SerializeField] private List<GameObject> chunkTutoT;
    [SerializeField] private List<GameObject> chunkMenuT;
    
    private List<ChunkRoad> _loadedChunkRoad = new();
    private List<ChunkRoad> _loadedChunkTuto = new();
    private List<ChunkRoad> _loadedChunkMenu = new();
    
    [HideInInspector] public List<ChunkRoad> currentChunkRoad;
    public Action OnAllDataLoaded;
    private GameObject roadGm;

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
    }
    
    private void Start()
    {
        foreach (var chunkT in chunkRoadT)
        {
            loaderRoadChunk.LoadChunk(chunkT, settingsRoadGen.chunksVisibe + 1,(data)=> PostLoadData(data,_loadedChunkRoad));
        }
        foreach (var chunkT in chunkTutoT)
        {
            loaderRoadChunk.LoadChunk(chunkT, settingsRoadGen.chunksVisibe,(data)=> PostLoadData(data,_loadedChunkTuto));
        }
        foreach (var chunkT in chunkMenuT)
        {
            loaderRoadChunk.LoadChunk(chunkT, settingsRoadGen.chunksVisibe + 1, (data)=> PostLoadData(data,_loadedChunkMenu));
        }
    }

    private void CheckAllDataLoaded()
    {
        var chunkRoadLoaded = _loadedChunkRoad.Count == (settingsRoadGen.chunksVisibe + 1) * chunkRoadT.Count;
        var chunkTutoLoaded = _loadedChunkTuto.Count == chunkTutoT.Count;
        var chunkMenuLoaded = _loadedChunkMenu.Count == (settingsRoadGen.chunksVisibe + 1) * chunkMenuT.Count;
        if (chunkRoadLoaded && chunkTutoLoaded && chunkMenuLoaded) OnAllDataLoaded?.Invoke();
    }

    private void PostLoadData(GameObject[] data, List<ChunkRoad> targetDataContainer)
    {
        foreach (var chunk in data)
        {
            chunk.SetActive(false);
            // chunk.GetComponent<TriggerExitChunk>().OnChunkExit += NotifyExitChunk;
            targetDataContainer.Add(chunk.GetComponent<ChunkRoad>());
            chunk.transform.SetParent(roadGm.transform);
        }
        CheckAllDataLoaded();
    }
    
    public ChunkRoad SelectionChunk()
    {
        return currentChunkRoad.FindAll(o => !o.gameObject.activeSelf).GetRandom();
    }
    
}
