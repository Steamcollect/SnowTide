using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloaderRoadChunk : MonoBehaviour
{
    private Queue<GameObject> chunksWaitUnload = new Queue<GameObject>();
    
    public void UnloadChunk(GameObject chunk)
    {
        chunksWaitUnload.Enqueue(chunk);
        if (chunksWaitUnload.Count > 1)
        {
            DisableChunk(chunksWaitUnload.Dequeue());
        }
    }

    public void DisableChunk(GameObject chunk)
    {
        chunk.GetComponent<ChunkRoad>().ResetChunk();
        chunk.SetActive(false);
    }

    public void DisableChunk(ChunkRoad chunk)
    {
        chunk.ResetChunk();
        chunk.gameObject.SetActive(false);
    }
    
    public void ResetUnloader()
    {
        chunksWaitUnload.Clear();
    }
}
