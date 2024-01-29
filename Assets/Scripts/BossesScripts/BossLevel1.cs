using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;

public class BossLevel1 : Boss, IMovableBoss
{
    Vector2 collidePoint;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag != "Player") collidePoint = other.ClosestPoint(transform.position);
    }

    public Transform GetTarget() => target;

    public void ActivateDeactivateBossMovement(bool isActivate)
    {
        movable.SetMovableState(isActivate);
    }

    public bool BossIsMoving() => movable.IsMoving();

    public Vector2 GetColliderPoint()
    {
        return collidePoint;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetStartSpeed() => speed;

    public void ChangeBossCollideWithWalls(bool isCollideWithWalls)
    {
        movable.ChangeCollideSetting(isCollideWithWalls);
    }

    public override void Death()
    {
        animator.SetTrigger("Kill");
        base.Death();
    }
}
