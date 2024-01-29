using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Spikes : MonoBehaviour, IRetractable
{
    [SerializeField] float defaultSpeed = 10;
    [SerializeField, ReadOnly] float speed;
    [SerializeField] Transform target;
    [SerializeField] ParticleSystem dust;
    bool isInitialized = false;
    TargetMove movable;
    Vector3 startTargetPos;
    Action ChangeValueByDifficulty;
    GameService gameService;

    private void Start()
    {
        Destroy(Instantiate(dust.gameObject, target.position, transform.rotation), 5);
        if (transform.parent.eulerAngles.z % 180 == 0) transform.parent.localScale = new Vector3(1.3f, 1, 1);
    }

    private void InitializeSpikes()
    {
        if (!isInitialized)
        {
            isInitialized = true;

            SetValueChanger();
            ChangeValueByDifficulty();

            movable = GetComponent<TargetMove>();
            movable.target = target;
            movable.SetSpeed(speed);
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

    public void ActivateDeactivateObject()
    {
        InitializeSpikes();
        if (startTargetPos == null) startTargetPos = target.localPosition;
        if (transform.position == target.position) ChangeTargetPosition();

        if (transform.localPosition == Vector3.zero) AudioManager.instance.PlaySound("Spikes");
        movable.SetMovableState(true);
    }

    public void ChangeTargetPosition()
    {
        if (target.localPosition == startTargetPos) target.localPosition = Vector3.zero;
        else target.localPosition = startTargetPos;
    }

    public bool IsOnTargetPosition()
    {
        return transform.position == target.position;
    }

    public Vector2 GetTargetPos()
    {
        return target.position;
    }

    private void OnDestroy()
    {
        gameService.SetLevelDifficulty -= ChangeValueByDifficulty;
    }
}
