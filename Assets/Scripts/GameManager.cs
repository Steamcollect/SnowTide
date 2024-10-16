using System.Collections;
using System.Collections.Generic;
using BT.Save;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RSE_Event OnCommandSave;
    
    private void OnApplicationQuit()
    {
        OnCommandSave.Call();
    }
}
