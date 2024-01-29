using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelState
{
    public int index;
    public bool isActive;

    public void ChangeState()
    {
        isActive = true;
        //PlayerPrefs.SetString(index + "LevelIsActive", "true");
    }

    public bool isActivated() => isActive;
}
