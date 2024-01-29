using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle soundsToggle;
    [SerializeField] Slider difficultySlider;
    //[SerializeField] Text difficultyValue;

    private void Awake() 
    {
        //musicToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetString("music", "true"));
        //soundsToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetString("sounds", "true"));
        //difficultySlider.value = PlayerPrefs.GetFloat("difficulty", 1);
        musicToggle.isOn = DataContainer.Instance.playerData.isMusicOn;
        soundsToggle.isOn = DataContainer.Instance.playerData.isSoundsOn;
    }

    public void ChangeMusicSettings()
    {
        //PlayerPrefs.SetString("music", musicToggle.isOn.ToString());
        DataContainer.Instance.playerData.isMusicOn = musicToggle.isOn;
        AudioManager.instance.SetSettings();
    }

    public void ChangeSoundsSettings()
    {
        //PlayerPrefs.SetString("sounds", soundsToggle.isOn.ToString());
        DataContainer.Instance.playerData.isSoundsOn = soundsToggle.isOn;
        AudioManager.instance.SetSettings();
    }

    //public void ChangeDifficultySettings()
    //{
    //    var value = (float)Math.Round(difficultySlider.value, 2);
    //    PlayerPrefs.SetFloat("difficulty", value);
    //    difficultyValue.text = value.ToString();
    //}
}
