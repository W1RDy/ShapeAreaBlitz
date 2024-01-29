using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyConfig : IRandomizable
{
    public EnemyType type;
    public Enemy enemy;
    public float chance;
    public float chanceStep;
    public List<string> spawnerPlaceIndex;
    public bool isDestroyableForTime;

    [ShowIf(nameof(isDestroyableForTime))]
    public float destroyingTime;

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
        return spawnerPlaceIndex;
    }

    public int GetIndex()
    {
        return (int)type;
    }
}
