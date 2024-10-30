using System;
using System.Collections;
using UnityEngine;


namespace BT.Audio
{
    public class Playlist : MonoBehaviour,IAudioComponent
    {
        [Header("Reference")]
        [SerializeField] private ClipData[] clipsPlaylist;
        [field:Space(5)]
        [field:SerializeField] public RSE_AudioEvent OnCallAudioPlay { get; set; }
        [field:SerializeField] public RSE_AudioEvent OnCallAudioStop { get; set; }

        [field:Header("Parameters")]
        [Tooltip("If value equal \"-1\" so infinite loop")]
        [field: SerializeField][field:Min(-1)]public int maxLoop { get; private set; } = -1;
        [SerializeField][Min(0)] private float transitionTimeClip = 0.2f;
        [field:SerializeField] public SoundType SoundType { get; set; }
        
        public AudioSource AudioSourcePlaying { get; set; }
        public Coroutine CoroutineAudioPlaying { get; set; }
        
        private int _indexCurrentClip;

        public void LaunchPlaylist()
        {
            StartCoroutine(LaunchPlaylistC());
        }

        public void StopPlaylist()
        {
            OnCallAudioStop.Call(this,null);
        }
        
        private IEnumerator LaunchPlaylistC()
        {
            int maxLoopMacro = maxLoop;
            while (maxLoopMacro is -1 or > 0)
            {
                //Send Message to Audio Manager to play the clip
                OnCallAudioPlay.Call(this,clipsPlaylist[_indexCurrentClip]);
                
                yield return new WaitForSeconds(clipsPlaylist[_indexCurrentClip].Clip.length + transitionTimeClip);
                
                //Increment clip index and loop
                _indexCurrentClip = (_indexCurrentClip + 1) % clipsPlaylist.Length;
                maxLoopMacro = maxLoopMacro == -1 ? -1 : maxLoopMacro - 1;
            }
        }

        public void CallbackAudioPlay()
        {
        }

        public void CallbackAudioStop()
        {
            StopCoroutine(LaunchPlaylistC());
            CoroutineAudioPlaying = null;
            AudioSourcePlaying = null;
        }
    }
}

