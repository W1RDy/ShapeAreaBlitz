using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PhraseDictionary : MonoBehaviour
{
    public static PhraseDictionary Instance;

    [SerializeField] PhraseConfig[] phrases;
    Dictionary<string, PhraseConfig> phrasesDictionary;
    bool isRussianLanguage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeDictionary();
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        SetPhrasesLanguage();
    }

    private void InitializeDictionary()
    {
        phrasesDictionary = new Dictionary<string, PhraseConfig>();

        foreach (var phrase in phrases) phrasesDictionary[phrase.index] = phrase;
    }

    private void SetPhrasesLanguage()
    {
        var language = InteractorWithBrowser.GetLanguage();
        isRussianLanguage = language == "ru";
    }

    public string GetMessage(string index)
    {
        var phrase = phrasesDictionary[index];
        if (isRussianLanguage) return phrase.messageOnRu;
        return phrase.messageOnEn;
    }
}

[Serializable]
public class PhraseConfig
{
    public string index;
    public string messageOnRu;
    public string messageOnEn;
}
