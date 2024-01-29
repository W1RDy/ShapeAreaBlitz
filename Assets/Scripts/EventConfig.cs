using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class EventConfig : IRandomizable
{
    public Event _event;    
    public bool isChanceEvent;

    [FoldoutGroup("ChanceSettings"), ShowIf(nameof(isChanceEvent))]
    public float chance;

    [FoldoutGroup("ChanceSettings"), ShowIf(nameof(isChanceEvent))]
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
        return (int)_event.type;
    }
}
