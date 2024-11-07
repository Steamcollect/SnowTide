using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace BT.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioMixer audioMixer;
        [Space(5)]
        [SerializeField] private AudioMixerGroup musicMixerGroup;
        [SerializeField] private AudioMixerGroup soundMixerGroup;
        [Space(5)]
        [SerializeField] private RSE_AudioEvent OnCallAudioPlay;
        [SerializeField] private RSE_AudioEvent OnCallAudioStop;
        [SerializeField] private RSE_ModifyVolume OnModifyVolume;

        [Header("Parameter")]
        [SerializeField, Tooltip("Number of GameObject create on start for audio sound clip")] private int startingAudioObjectsCount = 30;
        
        private Queue<AudioSource> _queueSoundAudioSource = new Queue<AudioSource>();
        private Queue<AudioSource> _queueMusicAudioSource = new Queue<AudioSource>();
        private Transform _musicGameObjectParent, _soundsGameObjectParent;
        
        #region Initialization
        private void Awake() //Create objects need to play audio
        {
            CreateParentAudioContainer(out _musicGameObjectParent,"____MusicObjects____");
            CreateParentAudioContainer(out _soundsGameObjectParent,"____SoundObjects____");

            AudioSource audioSourceCreated;
            
            for (var i = 0; i < startingAudioObjectsCount; ++i)
            {
                audioSourceCreated = CreateAudioGameObject(_soundsGameObjectParent, "SoundGameObject", soundMixerGroup);
                audioSourceCreated.gameObject.SetActive(false);
                _queueSoundAudioSource.Enqueue(audioSourceCreated);
            }
        }
        
        
        private void OnEnable()
        {
            OnCallAudioPlay.action += PlayClip;
            OnCallAudioStop.action += StopClip;
            OnModifyVolume.action += ModifyVolume;
        }
        //Assign and Unassigne event to communicate with audio component
        private void OnDisable()
        {
            OnCallAudioPlay.action -= PlayClip;
            OnCallAudioStop.action -= StopClip;
            OnModifyVolume.action -= ModifyVolume;
        }
        #endregion
        
        #region Creation Objects
        private AudioSource CreateAudioGameObject(Transform parent, string nameObject, AudioMixerGroup mixerGroup)
        {
            var audioSource = new GameObject(nameObject).AddComponent<AudioSource>();
            audioSource.transform.SetParent(parent);
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.volume = 0;
            return audioSource;
        }
        
        private void CreateParentAudioContainer(out Transform transformTarget, string nameObject)
        {
            transformTarget = new GameObject(nameObject).transform;
            transformTarget.transform.SetParent(transform);
        }
        #endregion

        #region AudioGestion
        /// <summary>
        /// Find an available object and play the clip
        /// </summary>
        /// <param name="audioComponent"> Component implemented interface to send data</param>
        /// <param name="clipData"> Scriptable Object which contain the clip and parameter</param>
        private void PlayClip(IAudioComponent audioComponent, ClipData clipData)
        {
            Queue<AudioSource> queue;
            
            //Find an audio source available to play the clip
            switch (audioComponent.SoundType)
            {
                case SoundType.Sound:
                    queue = _queueSoundAudioSource;
                    break;
                case SoundType.Music:
                    queue = _queueMusicAudioSource;
                    break;
                default:
                    queue = _queueSoundAudioSource;
                    break;
            }
            audioComponent.AudioSourcePlaying = queue.Count <= 0 ? CreateAudioGameObject(_musicGameObjectParent,"MusicGameObject",musicMixerGroup) : queue.Dequeue();
            audioComponent.AudioSourcePlaying.gameObject.SetActive(true);

            // Set parameter of audio clip in the audio source
            audioComponent.AudioSourcePlaying.volume = clipData.Volume;
            audioComponent.AudioSourcePlaying.pitch = clipData.Picth;
            audioComponent.AudioSourcePlaying.spatialBlend = clipData.SpatialBlend;
            audioComponent.AudioSourcePlaying.priority = clipData.Priority;

            // Set the clip
            audioComponent.AudioSourcePlaying.clip = clipData.Clip;
            audioComponent.AudioSourcePlaying.Play();
            audioComponent.CoroutineAudioPlaying = StartCoroutine(AwaitEnqueueAudioSource(audioComponent.AudioSourcePlaying,queue));
            audioComponent.CallbackAudioPlay();
        }
        
        /// <summary>
        /// Wait the end th clip playing to free it
        /// </summary>
        /// <param name="audioSource"> Audio Source use to play the clip</param>
        /// <returns></returns>
        private IEnumerator AwaitEnqueueAudioSource(AudioSource audioSource,Queue<AudioSource> queue)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            EnqueueAudioSource(audioSource,queue);
        }

        private void EnqueueAudioSource(AudioSource audioSource, Queue<AudioSource> queue)
        {
            queue.Enqueue(audioSource);
            audioSource.gameObject.SetActive(false);
        }

        /// <summary>
        /// Stop the current clip playing in the audioComponent and send it the control
        /// </summary>
        /// <param name="audioComponent"></param>
        /// <param name="clipData"></param>
        private void StopClip(IAudioComponent audioComponent, ClipData clipData)
        {
            
            EnqueueAudioSource(audioComponent.AudioSourcePlaying,audioComponent.SoundType  == SoundType.Music? _queueMusicAudioSource : _queueSoundAudioSource);
            StopCoroutine(audioComponent.CoroutineAudioPlaying);
            audioComponent.CallbackAudioStop();
        }
        #endregion

        #region VolumeMixers

        private void ModifyVolume(float value,SoundType soundType)
        {
            var nameParameter = soundType switch
            {
                SoundType.Sound => "SoundVolume",
                SoundType.Music => "MusicVolume",
                _ => ""
            };
            if (nameParameter == "") return;
            audioMixer.SetFloat(nameParameter, value == 0 ?-80.0f:Mathf.Log10(value) * 20f);
        }

        #endregion
    }
}

