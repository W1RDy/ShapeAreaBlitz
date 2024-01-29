using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossLeg : MonoBehaviour, IRetractable
{
    [SerializeField, FormerlySerializedAs("speed")] float defaultSpeed;
    [SerializeField, ReadOnly] float speed;
    [SerializeField] Transform updatedLeg;
    [SerializeField] Transform target;
    Transform endLegPoint;
    GameService gameService;
    Player player;
    RaycastHit2D hit;
    TargetMove targetMove;
    LineRenderer leg;
    Vector3 startTargetPos;
    bool isFirstMoving;
    bool isInitialized;
    Action ChangeValueByDifficulty;

    private void InitializeLeg()
    {
        if (!isInitialized)
        {
            isFirstMoving = true;
            isInitialized = true;
            leg = GetComponent<LineRenderer>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            targetMove = updatedLeg.GetComponent<TargetMove>();
            targetMove.target = target;
            targetMove.ChangeCollideSetting(true);
            SetValueChanger();
            ChangeValueByDifficulty();

            startTargetPos = target.position;
            endLegPoint = updatedLeg.GetChild(1);
        }
    }

    private void SetValueChanger()
    {
        ChangeValueByDifficulty = () =>
        {
            speed = ChangerValueByDifficulty.instance.GetValueByDifficult(true, defaultSpeed);
            targetMove.SetSpeed(speed);
        };
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
        gameService.SetLevelDifficulty += ChangeValueByDifficulty;
    }

    private void Update()
    {
        if (targetMove != null && targetMove.IsMoving())
        {
            leg.SetPosition(1, updatedLeg.position);
            var distance = Vector3.Distance(Vector3.zero, endLegPoint.position);
            var direction = transform.TransformDirection(Vector2.down);
            hit = Physics2D.Raycast(Vector3.zero, direction, distance, 1 << 7);
            if (hit.collider != null) player.ChangeHp(-1);
        }
    }

    public void ActivateDeactivateObject()
    {
        InitializeLeg();
        if (!isFirstMoving) ChangeTargetPosition();
        else isFirstMoving = false;
        targetMove.SetMovableState(true);
    }

    public void ChangeTargetPosition()
    {
        if (target.localPosition == Vector3.zero) target.position = startTargetPos;
        else target.localPosition = Vector3.zero; 
    }

    public Vector2 GetTargetPos()
    {
        return target.position;
    }

    public bool IsOnTargetPosition()
    {
        return !targetMove.IsMoving();
    }

    private void OnDestroy()
    {
        gameService.SetLevelDifficulty -= ChangeValueByDifficulty;
    }
}
