using UnityEngine;

namespace BT.Audio
{
    public interface IAudioComponent
    {
        //Event to communicate with AudioManager
        public RSE_AudioEvent OnCallAudioPlay { get; set; }
        public RSE_AudioEvent OnCallAudioStop { get; set; }
        
        //Data to know in which state is the sound in the component
        public SoundType SoundType { get; set; }
        public AudioSource AudioSourcePlaying { get; set;}
        public Coroutine CoroutineAudioPlaying { get; set; }

        //Need to callback in the component to do action before manager take the hand
        public void CallbackAudioPlay();
        public void CallbackAudioStop();
    }
}
