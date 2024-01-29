using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexDistributor : MonoBehaviour
{
    public static IndexDistributor instance = null;

    private int minAvailableIndex;
    private List<int> usedIndex;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            instance.usedIndex = new List<int>();
            Destroy(gameObject);
        }

        DontDestroyOnLoad(instance);

        usedIndex = new List<int>();
    }

    public int GetIndex()
    {
        var index = minAvailableIndex;
        usedIndex.Add(index);
        FindNewMinIndex();
        return index;
    }

    public void DestroyIndex(int destroyedIndex)
    {
        if (usedIndex.Contains(destroyedIndex))
        {
            if (destroyedIndex < minAvailableIndex) minAvailableIndex = destroyedIndex;
            usedIndex.Remove(destroyedIndex);
        }
    }

    private void FindNewMinIndex()
    {
        while (usedIndex.Contains(minAvailableIndex)) minAvailableIndex++;
    }
}
