using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel7 : Boss, IMovableBoss
{
    [SerializeField] public Transform legsSpawnPoint;
    private float startSpeed;
    BossActivator bossActivator;
    RetractableObjActivator blockForHitActivator;

    protected override void InitializeBossVariant()
    {
        base.InitializeBossVariant();
        startSpeed = speed;
        bossActivator = GameObject.Find("BossServices").GetComponent<BossActivator>();
        blockForHitActivator = GameObject.Find("Objects/Room/BlocksForHitsActivator").GetComponent<RetractableObjActivator>();
    }

    public override void SetValueChanger()
    {
        if (stateType != BossStateType.Attacking) base.SetValueChanger();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
        movable.SetSpeed(speed);
    }

    public void ActivateDeactivateBossMovement(bool isActivate)
    {
        movable.SetMovableState(isActivate);
    }

    public bool BossIsMoving() => movable.IsMoving();

    public override void Death()
    {
        battleService.FinishBossBattle(true);
        bossActivator.appearingDelay = 0;
        blockForHitActivator.ChangeCurrentBlockForHit(BossType.VirusPhantom);
        bossActivator.ActivateBoss(BossType.VirusPhantom);
        Destroy(gameObject);
    }

    public float GetStartSpeed()
    {
        return startSpeed;
    }

    public Transform GetTarget()
    {
        return target;
    }
    public void ChangeBossCollideWithWalls(bool isCollideWithWalls)
    {
        movable.ChangeCollideSetting(isCollideWithWalls);
    }
}
