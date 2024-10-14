using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

namespace BT.Audio
{
    public class AudioManager : MonoBehaviour
    {
        

        [Header("References")]
        [SerializeField] private AudioMixer audioMixer;
        [Space(5)]
        [SerializeField] private AudioMixerGroup musicMixerGroup;
        [SerializeField] private AudioMixerGroup soundMixerGroup;
        [Space(10)]
        [SerializeField] private RSE_AudioEvent onCallAudioPlay;
        [SerializeField] private RSE_EventBasic onInitPlaylist;

        [Header("System References")]
        [SerializeField, Tooltip("Number of GameObject create on start for the sound")] private int startingAudioObjectsCount = 30;

        private Queue<AudioSource> soundsGo = new Queue<AudioSource>();
        private Transform musicsGoParent, soundsGoParent;


        private void OnEnable() 
        { 
            onCallAudioPlay.action += PlayClip;
        }

        private void OnDisable()
        {
            //onInitPlaylist
            onCallAudioPlay.action -= PlayClip;
        }

        private void Start() => InitAudioManager();

        public void CreateAudioObject(AudioMixerGroup mixer, Transform parent,string name)
        {

        }

        public void PlayClip(AudioSource audioSource, float volumeMultiplier, AudioType type) { }

        #region Music
        public void SetMusicsGO(Playlist[] newPlaylists)
        {
            // Setup parent
            if (musicsGoParent == null)
            {
                musicsGoParent = new GameObject("____MusicObjects____").transform;
                musicsGoParent.SetParent(transform);
            }

            // Change playlist if its different
            // if(playlists != newPlaylists.ToList()) StartCoroutine(SetNewPlaylists(newPlaylists));
        }

        IEnumerator SetNewPlaylists(Playlist[] newPlaylists)
        {
            // Stop current playlists
            for (int i = 0; i < playlists.Count; i++)
            {
                playlists[i].FadeOut(transitionTime / 2);
            }
            yield return new WaitForSeconds(transitionTime / 2);

            // Clear current playlists
            for (int i = 0; i < playlists.Count; i++)
            {
                Destroy(playlists[i].audioSource.gameObject);
            }
            playlists.Clear();

            // Setup new playlists
            for (int i = 0; i < newPlaylists.Length; i++)
            {
                playlists.Add(CreatePlaylistGO(newPlaylists[i]));
                playlists[i].timerCoroutine = StartCoroutine(SetAudioSourceClip(playlists[i], playlists[i].maxLoop));
                playlists[i].FadeIn(playlists[i].volum, transitionTime / 2);
            }
        }

        Playlist CreatePlaylistGO(Playlist playlist)
        {
            // Create GameObject
            playlist.audioSource = new GameObject("Music GO").AddComponent<AudioSource>();
            playlist.audioSource.transform.SetParent(musicsGoParent);

            // Set Audio source references
            playlist.audioSource.outputAudioMixerGroup = musicMixerGroup;
            playlist.audioSource.volume = 0;

            return playlist;
        }

        IEnumerator SetAudioSourceClip(Playlist playlist, int maxLoop)
        {
            // Set clip
            playlist.audioSource.clip = playlist.clips[playlist.currentClipIndex];
            playlist.audioSource.Play();

            yield return new WaitForSeconds(playlist.clips[playlist.currentClipIndex].length);

            // Set clip index
            playlist.currentClipIndex = (playlist.currentClipIndex + 1) % playlist.clips.Length;

            maxLoop -= 1;
            if (maxLoop > 0 || maxLoop == -1)
            {
                StartCoroutine(SetAudioSourceClip(playlist, maxLoop));
            }

            // End the loop
            playlist.timerCoroutine = null;
        }

        #endregion

        /// <summary>
        /// Initialize audio objects child
        /// </summary>
        private void InitAudioManager()
        {
            soundsGoParent = new GameObject("____SoundObjects____").transform;
            soundsGoParent.SetParent(transform);

            for (int i = 0; i < startingAudioObjectsCount; i++)
            {
                AudioSource current = CreateAudioObject();
                current.gameObject.SetActive(false);
                soundsGo.Enqueue(current);
            }
        }

        private AudioSource CreateAudioObject()
        {
            AudioSource tmpAudioSource = new GameObject("Sound GO").AddComponent<AudioSource>();
            tmpAudioSource.transform.SetParent(soundsGoParent);
            tmpAudioSource.outputAudioMixerGroup = soundMixerGroup;
            soundsGo.Enqueue(tmpAudioSource);

            return tmpAudioSource;
        }


        /// <summary>
        /// Require the clip and the power of the sound
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="soundPower"></param>
        public void PlayClip(AudioClip clip, float volumMultiplier = 1)
        {
            AudioSource tmpAudioSource;
            if (soundsGo.Count <= 0) tmpAudioSource = CreateAudioObject();
            else tmpAudioSource = soundsGo.Dequeue();

            tmpAudioSource.gameObject.SetActive(true);

            // Set the volum
            volumMultiplier = Mathf.Clamp(volumMultiplier, 0, 1);
            tmpAudioSource.volume = volumMultiplier;

            // Set the clip
            tmpAudioSource.clip = clip;
            tmpAudioSource.Play();
            StartCoroutine(EnqueueAudioSource(tmpAudioSource));
        }

        private IEnumerator EnqueueAudioSource(AudioSource current)
        {
            yield return new WaitForSeconds(current.clip.length);
            current.gameObject.SetActive(false);
            soundsGo.Enqueue(current);
        }

        private void FadeInClip()
        {

        }

        private void FadeOutClip()
        {

        }
    }

    public enum AudioType 
    {
        Playlist,
        Sound
    }
}



