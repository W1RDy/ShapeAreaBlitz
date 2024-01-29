using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);
    }

    public void AddScore(int value)
    {
        DataContainer.Instance.playerData.score += value;
        DataContainer.Instance.SaveDataOnServer();
        InteractorWithBrowser.SaveScoreInLeaderboard(DataContainer.Instance.playerData.score);
    }
}
