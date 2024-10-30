using BT.Audio;
using UnityEngine;

public class ParametterButton : MonoBehaviour
{
    [SerializeField] private RSE_ModifyVolume OnModifyVolume;
    [SerializeField] private SoundType soundType;
    private float oldValue = -1.0f;
    
    public void OnButtonSwitch(float value)
    {
        value = Mathf.Clamp01(Mathf.Round(value));
        if (Mathf.Approximately(oldValue, Mathf.Round(value))) return;
        oldValue = value;
        OnModifyVolume.Call(value, soundType);
    }
}
