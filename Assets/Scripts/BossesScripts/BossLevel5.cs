using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BossLevel5 : Boss, IMovableBoss
{
    Collider2D _collider;

    public override void InitializeBoss()
    {
        base.InitializeBoss();
        _collider = GetComponent<Collider2D>();
    }

    public void ActivateDeactivateBossMovement(bool isActivate)
    {
        movable.SetMovableState(isActivate);
    }

    public bool BossIsMoving() => movable.IsMoving();

    public void ChangeBossCollideWithWalls(bool isCollideWithWalls)
    {
        movable.ChangeCollideSetting(isCollideWithWalls);
    }

    public override void Death()
    {
        if (_collider && _collider.enabled) _collider.enabled = false;
        cutscenesActivator.ActivateCutscene(this, false);
        animator.SetTrigger("Kill");
        base.Death();
    }

    public float GetStartSpeed() => speed;

    public Transform GetTarget() => target;

    public void SetSpeed(float speed) => this.speed = speed;
}
