using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    [Header("Playlist Parameters")]
    [SerializeField] private float transitionTime;

    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;
    [Space(5)]
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundMixerGroup;
    [Space(10)]
    [SerializeField] private RSE_AudioEvent _rseAudioEvent;
    
    [Header("System References")]
    [SerializeField, Tooltip("Number of GameObject create on start for the sound")] private int startingAudioObjectsCount = 30;

    private List<Playlist> playlists = new List<Playlist>();
    private Queue<AudioSource> soundsGo = new Queue<AudioSource>();
    private Transform musicsGoParent, soundsGoParent;



    private void OnEnable() {if (_rseAudioEvent) _rseAudioEvent.action += PlayClipAt;}

    private void OnDisable() {if (_rseAudioEvent) _rseAudioEvent.action -= PlayClipAt;}

    private void Start() => SetSoundsGO(); //Create Audio Object

    #region Music
    public void SetMusicsGO(Playlist[] newPlaylists)
    {
        // Setup parent
        if(musicsGoParent == null)
        {
            musicsGoParent = new GameObject("======MUSICS GO======").transform;
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
        if(maxLoop > 0 || maxLoop == -1)
        {
            StartCoroutine(SetAudioSourceClip(playlist, maxLoop));
        }

        // End the loop
        playlist.timerCoroutine = null;
    }

    #endregion

    #region Sound
    void SetSoundsGO()
    {
        soundsGoParent = new GameObject("======SOUND GO======").transform;
        soundsGoParent.SetParent(transform);
        
        for (int i = 0; i < startingAudioObjectsCount; i++)
        {
            AudioSource current = CreateSoundsGO();
            current.gameObject.SetActive(false);
            soundsGo.Enqueue(current);
        }
    }

    /// <summary>
    /// Require the clip and the power of the sound
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="soundPower"></param>
    public void PlayClipAt(AudioClip clip, float volumMultiplier = 1)
    {
        AudioSource tmpAudioSource;
        if (soundsGo.Count <= 0) tmpAudioSource = CreateSoundsGO();
        else tmpAudioSource = soundsGo.Dequeue();

        tmpAudioSource.gameObject.SetActive(true);

        // Set the volum
        volumMultiplier = Mathf.Clamp(volumMultiplier, 0, 1);
        tmpAudioSource.volume = volumMultiplier;

        // Set the clip
        tmpAudioSource.clip = clip;
        tmpAudioSource.Play();
        StartCoroutine(AddAudioSourceToQueue(tmpAudioSource));
    }
    IEnumerator AddAudioSourceToQueue(AudioSource current)
    {
        yield return new WaitForSeconds(current.clip.length);
        current.gameObject.SetActive(false);
        soundsGo.Enqueue(current);
    }

    AudioSource CreateSoundsGO()
    {
        AudioSource tmpAudioSource = new GameObject("Sound GO").AddComponent<AudioSource>();
        tmpAudioSource.transform.SetParent(soundsGoParent);
        tmpAudioSource.outputAudioMixerGroup = soundMixerGroup;
        soundsGo.Enqueue(tmpAudioSource);

        return tmpAudioSource;
    }
    #endregion
}


