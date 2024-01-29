using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    AudioSource audioSource;

    public void PlaySound(string soundIndex)
    {
        AudioManager.instance.PlaySound(soundIndex);
    }

    public void PlayLoopingSound(string soundIndex)
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            AudioManager.instance.AddLoopingSource(audioSource, name);
        }
        AudioManager.instance.PlayLoopingSound(name, soundIndex);       
    }

    public void StopLoopingSound()
    {
        AudioManager.instance.StopLoopingSound(name);
    }
}
