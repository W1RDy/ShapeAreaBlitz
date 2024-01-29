using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundScript : MonoBehaviour
{
    [SerializeField] private string soundIndex;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) AudioManager.instance.PlaySound(soundIndex);
    }
}
