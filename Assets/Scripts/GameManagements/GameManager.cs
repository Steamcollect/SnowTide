using System.Collections;
using System.Collections.Generic;
using BT.Save;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSE_IntEvent OnPlayerTakeDamage;
    [SerializeField] private RSE_Event OnCommandSave;
    [SerializeField] private UnityEvent BackgroundMusicEvent;
    
    private void Start() => BackgroundMusicEvent.Invoke();
    
    private void OnEnable(){OnPlayerTakeDamage.action+= PlayerTakeDamage;}
    private void OnDisable(){OnPlayerTakeDamage.action -= PlayerTakeDamage;}

    private void PlayerTakeDamage(int currentHealth)
    {
        
    }
    
    
    private void OnApplicationQuit()
    {
        OnCommandSave.Call();
    }
}
