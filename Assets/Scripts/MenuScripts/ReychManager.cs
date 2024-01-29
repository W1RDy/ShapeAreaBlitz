using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReychManager : MonoBehaviour
{
    public static ReychManager instance;

    [SerializeField] ReychConfig[] reyches;
    Dictionary<ReychType, Sprite> reychesDict;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeReychDictionary();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);
    }

    private void InitializeReychDictionary()
    {
        reychesDict = new Dictionary<ReychType, Sprite>();

        foreach (var reych in reyches) reychesDict[reych.reychType] = reych.reychSprite;
    }

    public Sprite GetReychSprite(ReychType reychType) => reychesDict[reychType];
}
