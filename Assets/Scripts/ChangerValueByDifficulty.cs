using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerValueByDifficulty : MonoBehaviour
{
    public static ChangerValueByDifficulty instance;
    private GameService gameService;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            instance.gameService = GameObject.Find("GameService").GetComponent<GameService>();
            Destroy(gameObject);
        }

        DontDestroyOnLoad(instance);

        instance.gameService = GameObject.Find("GameService").GetComponent<GameService>();
    }

    public float GetValueByDifficult(bool isStraightConnections, float defaultValue)
    {
        if (isStraightConnections) return defaultValue * gameService.levelDifficulty;
        else return defaultValue / gameService.levelDifficulty;
    }
}
