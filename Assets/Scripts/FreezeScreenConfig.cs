using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FreezeScreenConfig
{
    public float duration;
    public float minSize, maxSize;
    public FreezeScreen freezeScreen;

    public void SetScreenParameters()
    {
        freezeScreen.SetParameters(minSize, maxSize, duration);
    }
}
