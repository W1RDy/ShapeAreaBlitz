using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] PoolElementConfig[] poolElements;
    Dictionary<Enum, PoolMono<MonoBehaviour>> poolsDictionaryWithTypes;
    Dictionary<Type, PoolMono<MonoBehaviour>> poolsDictionary;
    Transform objects;

    private void Awake()
    {
        objects = GameObject.Find("Objects").transform;
        InitializePoolsDictionaries();
    }

    private void InitializePoolsDictionaries()
    {
        poolsDictionaryWithTypes = new Dictionary<Enum, PoolMono<MonoBehaviour>>();
        poolsDictionary = new Dictionary<Type, PoolMono<MonoBehaviour>>();

        foreach (var element in poolElements) 
        {
            var IType = element.prefab.GetComponent<IType>();
            var container = GetContainer(IType, element.prefab);

            if (IType != null)
                poolsDictionaryWithTypes[IType.GetType()] = new PoolMono<MonoBehaviour>(element.prefab, element.poolSize, container);
            else poolsDictionary[element.prefab.GetType()] = new PoolMono<MonoBehaviour>(element.prefab, element.poolSize, container);
        }
    }

    public PoolMono<MonoBehaviour> GetPool(Enum type)
    {
        if (!poolsDictionaryWithTypes.ContainsKey(type)) return null;
        return poolsDictionaryWithTypes[type];
    }

    public PoolMono<MonoBehaviour> GetPool(Type type)
    {
        return poolsDictionary[type];
    }

    private Transform GetContainer(IType IType, MonoBehaviour element)
    {
        string name;
        if (IType != null)
        {
            var type = element.GetType();
            var baseType = type.BaseType;
            while (baseType != null && baseType.IsAbstract)
            {
                type = baseType;
                baseType = type.BaseType;
            }
            name = type + "Container";
        }
        else name = element.GetType() + "Container";

        var container = name == "BonusContainer" ? objects.Find(name) : transform.Find(name);
        if (container == null)
        {
            container = new GameObject(name).transform;
            if (name == "BonusContainer")
                container.transform.SetParent(objects);
            else container.transform.SetParent(transform);
        }
        return container;
    }
}
