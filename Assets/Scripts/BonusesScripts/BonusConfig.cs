using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BonusConfig : IRandomizable
{
    public EffectType type;
    public Bonus bonus;
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
        return (int)type;
    }
}
