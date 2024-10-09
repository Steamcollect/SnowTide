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
            
            chunksWaitUnload.Dequeue().SetActive(false);
        }
    }
}
