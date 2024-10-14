using System.Collections;
using UnityEngine;

namespace BT.Audio
{
    public class Playlist : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private AudioClipSetting[] clipsPlaylist;
        [Space(5)]
        [SerializeField] private RSE_AudioEvent onCallAudioPlay;
        [SerializeField] private RSE_EventBasic onInitPlaylist;

        [Header("Parameters")]
        [Tooltip("If value equal \"-1\" so infinite loop")]
        [SerializeField] private int maxLoop = -1;
        [SerializeField] private float volumeMultiplier;
        [SerializeField] private float transitionTime;

        [HideInInspector] public Coroutine timerCoroutine;
        [HideInInspector] public int indexCurrentClip;

        private void Start()
        {
            onInitPlaylist.Call();
        }

        private IEnumerator LaunchPlaylist()
        {
            yield break;
        }

        public void StopPlaylist()
        {

        }
    }
}

