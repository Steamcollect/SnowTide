using System.Collections;
using System.Collections.Generic;
using BT.Save;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RSE_Event OnCommandSave;
    [SerializeField] private UnityEvent BackgroundMusicEvent;
    
    private void Start() => BackgroundMusicEvent.Invoke();
    
    private void OnApplicationQuit()
    {
        OnCommandSave.Call();
    }
}
