using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    AudioSource audioSource;

    [SerializeField] AudioConfig[] audios;
    private bool isMusic;
    private bool isSounds;
    private bool onPause;
    AudioConfig currentAudio;
    private Dictionary<string, (AudioSource source, bool isEnabled)> loopingSoundsSources;
    Dictionary<string, AudioConfig> audioDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            loopingSoundsSources = new Dictionary<string, (AudioSource, bool)>();
            SetSettings();
            InitializeAudioDictionary();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);
        instance.RemoveAllLoopingSource();
    }

    private void InitializeAudioDictionary()
    {
        audioDictionary = new Dictionary<string, AudioConfig>();

        foreach (var audio in audios) audioDictionary[audio.index] = audio;
    }

    public void SetSettings()
    {
        //isMusic = Convert.ToBoolean(PlayerPrefs.GetString("music", "true"));
        //isSounds = Convert.ToBoolean(PlayerPrefs.GetString("sounds", "true"));
        isMusic = DataContainer.Instance.playerData.isMusicOn;
        isSounds = DataContainer.Instance.playerData.isSoundsOn;
        DataContainer.Instance.SaveDataOnServer();
        StartStopLoopingSounds(isSounds);
        PlayStopMusic();
    }

    public AudioConfig GetAudioConfig(string index) => audioDictionary[index];

    public void PlayMusic(string index)
    {
        if (currentAudio == null || currentAudio.index != index)
        {
            audioSource.Stop();
            currentAudio = GetAudioConfig(index);
            audioSource.volume = currentAudio.volume;
            audioSource.clip = currentAudio.audioClip;
            PlayStopMusic();
        }
    }

    private void PlayStopMusic()
    {
        if (isMusic && !onPause && !audioSource.isPlaying) audioSource.Play();
        else if (!isMusic && audioSource.isPlaying) audioSource.Stop();
    }

    public void PlaySound(string index)
    {
        if (isSounds && !onPause)
        {
            var audioConfig = audioDictionary[index];
            audioSource.PlayOneShot(audioConfig.audioClip, audioConfig.volume / audioSource.volume);
        }
    }

    public void AddLoopingSource(AudioSource audioSource, string index)
    {
        if (!loopingSoundsSources.ContainsKey(index)) loopingSoundsSources.Add(index, (audioSource, true));
    }

    public void RemoveLoopingSource(string index)
    {
        loopingSoundsSources[index].source.Stop();
        loopingSoundsSources.Remove(index);
    }

    private void RemoveAllLoopingSource()
    {
        loopingSoundsSources.Clear();
    }

    public void PlayLoopingSound(string sourceIndex, string index)
    {
        var audioSource = loopingSoundsSources[sourceIndex];
        audioSource.isEnabled = true;
        var sound = GetAudioConfig(index);
        if (audioSource.source.clip != sound.audioClip)
        {
            if (audioSource.source.isPlaying) audioSource.source.Stop();
            audioSource.source.volume = sound.volume;
            audioSource.source.clip = sound.audioClip;
        }

        if (!audioSource.source.isPlaying && isSounds && !onPause) audioSource.source.Play();
    }

    public void PauseStartAudio(string startIndex)
    {
        var isStart = Convert.ToBoolean(startIndex);
        onPause = !isStart;
        if (isStart) audioSource.UnPause();
        else
        {
            audioSource.Pause();
            StartStopLoopingSounds(false);
        }
    }

    public void StopLoopingSound(string sourceIndex)
    {
        var audioSource = loopingSoundsSources[sourceIndex];
        audioSource.isEnabled = false;
        if (audioSource.source.isPlaying) audioSource.source.Stop();
    }

    private void StartStopLoopingSounds(bool isStart)
    {
        foreach (var audioSource in loopingSoundsSources.Values)
        {
            if (audioSource.source != null)
            {
                if (isStart && audioSource.isEnabled)
                    audioSource.source.Play();
                else if (!isStart) audioSource.source.Stop();
            }
        }
    }
}
