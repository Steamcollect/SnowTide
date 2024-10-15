using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Audio
{
    [CreateAssetMenu(fileName = "SCO_ClipData", menuName = "BoxTool/Audio/ClipData")]
    public class ClipData : ScriptableObject
    {
        [field:Header("Reference")]
        [field:SerializeField]
        public AudioClip Clip {get; private set;}

        [field: Header("Settings")]
        [field: SerializeField][field: Range(0, 255)][field:Tooltip("0 = High, 255 = Low")]
        public int Priority { get; private set; } = 128;
        [field: SerializeField][field: Range(0f, 1f)]
        public float Volume { get; private set; } = 1f;
        [field:SerializeField][field:Range(-3f,3f)]
        public float Picth { get; private set; } = 1f;
        [field:SerializeField][field:Tooltip("0 = 2D, 1 = 3D")][field:Range(0,1)] 
        public float SpatialBlend {get; private set;}

    } 
    public enum SoundType
    {
        Sound,
        Music
    }
}
