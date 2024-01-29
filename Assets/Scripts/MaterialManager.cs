using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] MaterialConfig[] materials;
    public static MaterialManager instance;

    Dictionary<string, MaterialConfig> materialsDict;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeMaterialsDictionary();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);
    }

    private void InitializeMaterialsDictionary()
    {
        materialsDict = new Dictionary<string, MaterialConfig>();

        foreach (var material in materials) materialsDict[material.materialIndex] = material; 
    }

    public MaterialConfig GetMaterial(string materialIndex) => materialsDict[materialIndex];
}
