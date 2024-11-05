using BT.Audio;
using UnityEngine;

public class ParametterButton : MonoBehaviour
{
    [SerializeField] private RSE_ModifyVolume OnModifyVolume;
    [SerializeField] private SoundType soundType;
    
    public void OnButtonSwitch(float value) => OnModifyVolume.Call(value, soundType);
}
