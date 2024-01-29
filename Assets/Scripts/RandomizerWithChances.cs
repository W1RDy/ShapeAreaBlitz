using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomizerWithChances<T> where T : IRandomizable
{
    public static T RandomWithChances(T[] randomizables)
    {
        return RandomWithChances(randomizables, null);
    }

    public static T RandomWithChances(T[] randomizables, List<string> correctIndexes)
    {
        var randValue = Random.Range(1, 101);
        float sum = 0;
        T chosenObject = default(T);
        bool isChoosen = false;

        foreach (var randomizable in randomizables)
        {
            sum += randomizable.GetChance();
            if (randValue <= sum)
            {
                isChoosen = IsCorrectObj(randomizable, correctIndexes);
                if (isChoosen) chosenObject = randomizable;
                break;
            }
        }
        if (isChoosen) UpdateChances(randomizables, chosenObject);
        return chosenObject;
    }

    private static void UpdateChances(T[] randomizables, T chosenObj)
    {
        var step = chosenObj.GetChanceStep();
        var index = chosenObj.GetIndex();
        foreach (var randomizable in randomizables)
        { 
            if (randomizable.GetIndex() != index) randomizable.ChangeChance(step);
            else randomizable.ChangeChance(-step * (randomizables.Length - 1));
        }
    }

    private static bool IsCorrectObj(T randomizable, List<string> correctIndexes)
    {
        if (correctIndexes == null) return true;

        foreach (var index in randomizable.GetCorrectIndex())
            if (correctIndexes.Contains(index)) return true; 

        return false;
    }
}
