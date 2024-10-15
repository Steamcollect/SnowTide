
using UnityEngine;

namespace BT.Audio
{
    public class SoundPlayer : MonoBehaviour, IAudioComponent
    {
        [field:Header("Reference")]
        [field:SerializeField] public RSE_AudioEvent OnCallAudioPlay { get; set; }
        [field:SerializeField] public RSE_AudioEvent OnCallAudioStop { get; set; }
        [Space(5)] 
        [SerializeField] private ClipData clip;
        [field:SerializeField] public SoundType SoundType { get; set; }

        public AudioSource AudioSourcePlaying  { get; set; }
        public Coroutine CoroutineAudioPlaying { get; set; }

        public void LaunchAudio()
        {
            OnCallAudioPlay.Call(this,clip);
        }

        public void StopAudio()
        {
            OnCallAudioStop.Call(this,null);
        }
        
        public void CallbackAudioPlay()
        {
        }

        public void CallbackAudioStop()
        {
            CoroutineAudioPlaying = null;
            AudioSourcePlaying = null;
        }
    }
}

