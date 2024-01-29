using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BossRoomAttack : BaseBossAttack
{
    [SerializeField, FormerlySerializedAs("bossSpeed")] float defaultBossSpeed;
    [SerializeField, ReadOnly] float bossSpeed;
    [SerializeField, FormerlySerializedAs("roomSpeed")] float defaultRoomSpeed;
    [SerializeField, ReadOnly] float roomSpeed;
    [SerializeField] float moveCooldown;
    [SerializeField] Transform roomTarget;
    IMovableBoss movableBoss;
    Transform objects;
    Transform boss;
    Transform bossTarget;
    TargetMove roomTargetMove;
    Vector3 defaultScale;
    Vector3 scale;
    Action<Vector3> callback;

    public override void Awake()
    {
        objects = GameObject.Find("Objects").transform;
        ChangeValueByDifficulty = () =>
        {
            bossSpeed = ChangerValueByDifficulty.instance.GetValueByDifficult(true, defaultBossSpeed);
            if (movableBoss != null && isActivated) movableBoss.SetSpeed(bossSpeed);
        };
        base.Awake();
        ChangeValueByDifficulty = () =>
        {
            roomSpeed = ChangerValueByDifficulty.instance.GetValueByDifficult(true, defaultRoomSpeed);
            if (roomTargetMove) roomTargetMove.SetSpeed(roomSpeed);
        };
        base.Awake();
    }

    public override void InitializeAttack(Transform boss)
    {
        roomTargetMove = objects.GetComponent<TargetMove>();
        roomTargetMove.target = roomTarget;
        roomTargetMove.SetMovableState(false);
        roomTargetMove.SetSpeed(roomSpeed);

        movableBoss = boss.GetComponent<IMovableBoss>();
        this.boss = boss;
        bossTarget = movableBoss.GetTarget();

        defaultScale = objects.localScale;
        callback = scale => objects.localScale = scale;
    }

    public override void ActivateAttack()
    {
        isActivated = true;
        isFinishing = false;
        scale = defaultScale;
        movableBoss.SetSpeed(bossSpeed);
        ChangeObjectsScale();
        StartCoroutine(Attack());
        StartCoroutine(RoomMovement());
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitUntil(ObjectsChangedScale);
            bossTarget.position = GetTargetPoint(bossTarget);
            TrajectoryShower.instance.ShowTrajectory(boss.position, bossTarget.position, moveCooldown);
            yield return new WaitForSeconds(moveCooldown);
            movableBoss.ActivateDeactivateBossMovement(true);
            yield return new WaitWhile(movableBoss.BossIsMoving);
            if (isFinishing) break;
        }
        bossTarget.position = Vector3.zero;
        TrajectoryShower.instance.ShowTrajectory(boss.position, bossTarget.position, moveCooldown);
        yield return new WaitForSeconds(moveCooldown);
        movableBoss.ActivateDeactivateBossMovement(true);
    }

    private IEnumerator RoomMovement()
    {
        while (true)
        {
            yield return new WaitUntil(ObjectsChangedScale);
            roomTarget.position = GetTargetPoint(roomTarget);
            roomTargetMove.SetMovableState(true);
            yield return new WaitWhile(roomTargetMove.IsMoving);
            if (isFinishing) break;
        }
        roomTarget.localPosition = Vector3.zero;
        roomTargetMove.SetMovableState(true);
        StartCoroutine(ReturnToWaitPose());
    }

    private IEnumerator ReturnToWaitPose()
    {
        yield return new WaitWhile(roomTargetMove.IsMoving);
        yield return new WaitUntil(BossOnZero);
        movableBoss.SetSpeed(movableBoss.GetStartSpeed());
        ChangeObjectsScale();
        yield return new WaitUntil(ObjectsChangedScale);
        isActivated = false;
    }

    private Vector2 GetTargetPoint(Transform transform)
    {
        var newPos = new Vector2(Random.Range(-12, 12), Random.Range(-7, 7));
        var distance = Vector2.Distance(transform.position, newPos);
        if (distance < 7f) return GetTargetPoint(transform);
        return newPos;
    }

    private void ChangeObjectsScale()
    {
        if (scale == defaultScale) scale = new Vector3(0.5f, 0.5f, 1);
        else scale = defaultScale;
        StartCoroutine(SmoothChanger<SmoothableVector3, Vector3>.SmoothChange(new SmoothableVector3(objects.localScale), scale, 2f, callback));
    }

    private bool ObjectsChangedScale() => objects.localScale == scale;

    private bool BossOnZero() => boss.localPosition == Vector3.zero;
}
