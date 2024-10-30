using UnityEngine;

namespace BT.Audio
{
    [CreateAssetMenu(fileName = "RSE_AudioEvent",menuName = "BoxTool/Audio/AudioEvent")]
    public class RSE_AudioEvent : ScriptablesObject.WrapperAction<IAudioComponent, ClipData>{}
}

