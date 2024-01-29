using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerSkin
{
    public int skinIndex;
    public bool isEnabled;

    public PlayerSkin(int skinIndex)
    {
        this.skinIndex = skinIndex;
    }
}
