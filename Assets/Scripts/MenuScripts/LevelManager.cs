using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelState[] levels;
    Dictionary<int, LevelState> levelsDict;

    public static LevelManager instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        levelsDict = new Dictionary<int, LevelState>();

        //foreach (var level in levels)
        //{
        //    var defaultValue = level.index > 1 ? "false" : "true";
        //    var isActive = Convert.ToBoolean(PlayerPrefs.GetString(level.index + "LevelIsActive", defaultValue));
        //    levelsDict[level.index] = level;

        //    if (isActive) level.ChangeState();
        //}
        foreach (var levelState in levels)
        {
            if (levelState.index <= DataContainer.Instance.playerData.lastActivatedLevel)
                levelState.isActive = true;
        }

        foreach (var level in levels) levelsDict[level.index] = level;
    }

    public LevelState GetLevelState(int index) => levelsDict[index];

    public void ActivateNextLevel(float nextLevelDialogDelay, int currentLevelIndex)
    {
        var levelIndex = currentLevelIndex + 1;
        if (levelsDict.ContainsKey(levelIndex))
        {
            var level = levelsDict[levelIndex];
            if (!level.isActivated())
            {
                levelsDict[levelIndex].ChangeState();
                DataContainer.Instance.playerData.lastActivatedLevel = levelIndex;
                StartCoroutine(DialogDelay(nextLevelDialogDelay, levelIndex));
            }
        }
        //else if (levelIndex == levels.Length && !Convert.ToBoolean(PlayerPrefs.GetString("FinishAllLevels", "false")))
        //{
        //    PlayerPrefs.SetString("FinishAllLevels", "true");
        //    StartCoroutine(DialogDelay(nextLevelDialogDelay, levelIndex));
        //}
        else if (levelIndex == levels.Length && !DataContainer.Instance.playerData.isCompletedAllLevels)
        {
            ScoreCounter.instance.AddScore(5000);
            DataContainer.Instance.playerData.isCompletedAllLevels = true;
            StartCoroutine(DialogDelay(nextLevelDialogDelay, levelIndex));
        }
        DataContainer.Instance.SaveDataOnServer();
    }

    private IEnumerator DialogDelay(float delay, int levelIndex)
    {
        yield return new WaitForSeconds(delay);
        if (levelIndex < levels.Length) DialogManager.instance.StartDialogBranch("Level" + levelIndex);
        else DialogManager.instance.StartDialogBranch("Congratulations");
        if (levelIndex == 2)
        {
            yield return new WaitUntil(DialogManager.instance.DialogIsFinished);
            InteractorWithBrowser.SuggestRateGame();
        }
    }
}
