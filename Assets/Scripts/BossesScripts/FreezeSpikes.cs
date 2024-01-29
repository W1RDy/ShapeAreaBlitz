using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeSpikes : MonoBehaviour
{
    [SerializeField] float defaultSpeed = 10f;
    [SerializeField, ReadOnly] float speed;
    DirectionalMove movable;
    GameService gameService;
    Action ChangeValueByDifficulty;

    void Start()
    {
        movable = GetComponent<DirectionalMove>();
        SetValueChanger();
        ChangeValueByDifficulty();
    }

    private void SetValueChanger()
    {
        ChangeValueByDifficulty = () =>
        {
            speed = ChangerValueByDifficulty.instance.GetValueByDifficult(true, defaultSpeed);
            movable.SetSpeed(speed);
        };
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
        gameService.SetLevelDifficulty += ChangeValueByDifficulty;
    }

    private void OnDestroy()
    {
        gameService.SetLevelDifficulty -= ChangeValueByDifficulty;
    }
}
