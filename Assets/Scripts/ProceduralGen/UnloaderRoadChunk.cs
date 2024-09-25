using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloaderRoadChunk : MonoBehaviour
{

    public void UnloadChunk(GameObject chunk)
    {
        chunk.SetActive(false);
    }
}
