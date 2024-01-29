using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioActivator : MonoBehaviour
{
    [SerializeField] string musicIndex;

    private void Start()
    {
        PlayMusic(musicIndex);
    }

    public void PlayMusic(string musicIndex) => AudioManager.instance.PlayMusic(musicIndex);
}
