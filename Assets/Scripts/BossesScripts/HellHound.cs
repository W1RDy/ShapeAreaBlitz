using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HellHound : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("speed")] float defaultSpeed;
    [SerializeField, ReadOnly] float speed;
    [SerializeField] public Transform target;
    AudioSource audioSource;
    GameService gameService;
    Vector3 startPos, startTargetPos;
    TargetMove movable;
    bool isInitialized;
    Action ChangeValueByDifficulty;

    private void InitializeHellHound()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            startPos = transform.position;
            startTargetPos = new Vector2(0, target.position.y);
            target.position = startTargetPos;
            if (transform.position.x < 0) transform.localScale = new Vector3(-6, 6, 1);
            SetValueChanger();
            ChangeValueByDifficulty();

            movable = GetComponent<TargetMove>();
            movable.SetMovableState(false);
            movable.target = target;
            movable.SetSpeed(speed);

            audioSource = GetComponent<AudioSource>();
            AudioManager.instance.AddLoopingSource(audioSource, name);
            AudioManager.instance.PlayLoopingSound(name, "Bark");

            if (gameService.currentBoss == BossType.VirusPhantom)
            {
                foreach (var view in transform.GetComponentsInChildren<SpriteRenderer>())  
                    view.material = MaterialManager.instance.GetMaterial("Phantom").material;
            }
        }
    }

    private void SetValueChanger()
    {
        ChangeValueByDifficulty = () =>
        {
            speed = ChangerValueByDifficulty.instance.GetValueByDifficult(true, defaultSpeed);
            if (movable) movable.SetSpeed(speed);
        };
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
        gameService.SetLevelDifficulty += ChangeValueByDifficulty;
    }

    public void ActivateHellHoundMove()
    {
        InitializeHellHound();
        if (transform.position == target.position) ChangeTargetPos();
        movable.SetMovableState(true);
    }

    private void ChangeTargetPos()
    {
        if (target.position == startPos) target.position = startTargetPos;
        else target.position = startPos;
    }

    public bool IsMoving() => movable.IsMoving();

    private void OnDestroy()
    {
        gameService.SetLevelDifficulty -= ChangeValueByDifficulty;
        AudioManager.instance.RemoveLoopingSource(name);
    }
}
