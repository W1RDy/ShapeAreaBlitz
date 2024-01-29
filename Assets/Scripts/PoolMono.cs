using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMono<T> where T: MonoBehaviour
{
    public T prefab { get; }
    public bool autoExpand { get; set; }
    public Transform container;
    private List<T> pool;

    public PoolMono(T prefab, int count, Transform container)
    {
        this.prefab = prefab;
        this.container = container;
        autoExpand = true;
        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        pool = new List<T>();
        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = Object.Instantiate(prefab, container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        pool.Add(createdObject);
        return createdObject;
    }

    public bool HasFreeElement()
    {
        foreach (var mono in pool)
        {
            if (!mono.gameObject.activeInHierarchy) return true;
        }
        return false;
    }

    public T GetFreeElement(bool activateImmediately = true)
    {
        foreach(var mono in pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                if (activateImmediately) mono.gameObject.SetActive(true);
                return mono;
            }
        }

        if (autoExpand) return CreateObject(true);

        throw new System.Exception($"There is no free elements in pool of type{typeof(T)}");
    }

    public List<T> GetActiveElements()
    {
        var list = new List<T>();
        foreach (var mono in pool)
            if (mono.gameObject.activeInHierarchy) list.Add(mono);
        return list;
    }

    public T GetFirstElement() => pool[0];
}
