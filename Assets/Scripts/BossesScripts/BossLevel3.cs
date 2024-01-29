using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BossLevel3 : Boss, IMovableBoss
{
    float startSpeed;

    protected override void InitializeBossVariant()
    {
        base.InitializeBossVariant();
        startSpeed = speed;
        target.localPosition = Vector3.zero;
    }

    public override void SetValueChanger()
    {
        if (stateType != BossStateType.Attacking) base.SetValueChanger();
    }

    public float GetStartSpeed() => startSpeed;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
        movable.SetSpeed(speed);
    }

    public Transform GetTarget()
    {
        return target;
    }

    public bool BossIsMoving() => movable.IsMoving();

    public void ActivateDeactivateBossMovement(bool isActivate)
    {
        movable.SetMovableState(isActivate);
    }

    public void ChangeBossCollideWithWalls(bool isCollideWithWalls)
    {
        movable.ChangeCollideSetting(isCollideWithWalls);
    }

    public override void Death()
    {
        cutscenesActivator.ActivateCutscene(this, false);
        base.Death();
    }
}
