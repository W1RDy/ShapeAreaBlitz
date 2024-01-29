using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossAttack : IRandomizable
{
    public int index;
    public string attackableIndex;
    public float duration;
    public IAttackable attackable;
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
