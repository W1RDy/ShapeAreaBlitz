using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PointConfig : IRandomizable
{
    public int index;
    public Transform point;
    public float chance;
    public float chanceStep;

    public void ChangeChance(float value)
    {
        chance += value;
    }

    public float GetChance()
    {
        return chance;
    }

    public float GetChanceStep()
    {
        return chanceStep;
    }

    public List<string> GetCorrectIndex()
    {
        return null;
    }

    public int GetIndex()
    {
        return index;
    }
}
