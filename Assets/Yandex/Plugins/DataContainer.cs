using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public int coins = 0;
    public int score = 0;
    public int lastActivatedLevel = 1;
    public List<PlayerSkin> purchasedSkins;
    public bool isNeedTutorial = true;
    public bool isCompletedAllLevels = false;
    public bool isMusicOn = true;
    public bool isSoundsOn = true;
}

public class DataContainer : MonoBehaviour
{
    public PlayerData playerData;
    public static DataContainer Instance;
    private bool dataIsLoading = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        StartCoroutine(WaitWhileInitialized());
#endif
#if UNITY_EDITOR
        Debug.Log("Load");
        dataIsLoading = true;
#endif
    }

    private IEnumerator WaitWhileInitialized()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if (InteractorWithBrowser.IsInitialized())
            {
                InteractorWithBrowser.LoadData();
                break;
            }
        }
    }

    public void SaveDataOnServer()
    {
        InteractorWithBrowser.SaveDataOnServer(playerData);
    }

    public void LoadDataFromServer(string value)
    {
        var data = JsonUtility.FromJson<PlayerData>(value);
        if (!data.isNeedTutorial) playerData = data;
        dataIsLoading = true;
    }

    public bool DataIsLoading() => dataIsLoading;
}
