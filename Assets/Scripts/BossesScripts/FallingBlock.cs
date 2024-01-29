using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    public int index;
    [SerializeField] float defaultSpeed = 6f;
    [SerializeField, ReadOnly] float speed;
    DirectionalMove movable;
    GameService gameService;
    Action ChangeValueByDifficulty;

    void Start()
    {
        index = IndexDistributor.instance.GetIndex();
        movable = GetComponentInChildren<DirectionalMove>();
        SetValueChanger();
        ChangeValueByDifficulty();
        Invoke(nameof(ShowInvisibleObstacle), 0.1f);
    }

    private void ShowInvisibleObstacle() => ShowerInvisibleObstacles.instance.ShowSign(transform.GetChild(0), index);

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
        IndexDistributor.instance.DestroyIndex(index);
    }
}
