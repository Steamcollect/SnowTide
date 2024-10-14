using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SCO_AudioSetting", menuName = "SCO/AudioSetting")]
public class AudioSetting : ScriptableObject
{
    [field:Header("Reference")]
    [field:SerializeField] public AudioClip source {get; private set;}

    [field:Header("Settings")]
    [field:SerializeField] public float volume {get; private set;}
    [field:SerializeField] public float picth {get; private set;}
    [field:SerializeField][field:Tooltip("0 = 2D, 1 = 3D")][field:Range(0,1)] public float spatialBlend {get; private set;}
}
