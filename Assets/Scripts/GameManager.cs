using System.Collections;
using System.Collections.Generic;
using BT.Save;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RSE_Event OnCommandSave;
    [SerializeField] private UnityEvent PlayPlaylist;
    
    private void Start() => PlayPlaylist.Invoke();
    
    private void OnApplicationQuit()
    {
        OnCommandSave.Call();
    }
}
