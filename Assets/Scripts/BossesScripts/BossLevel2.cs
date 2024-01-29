using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public class BossLevel2 : Boss, IMovableBoss
{
    [SerializeField] public Transform laserStartPoint;
    Vector3 startPos;
    Vector3 collisionPoint;
    Combustion combustion;

    public override void InitializeBoss()
    {
        base.InitializeBoss();
        combustion = GetComponentInChildren<Combustion>();
        combustion.InitializeCombustionMaterial(isPhantom);
    }

    protected override void InitializeBossVariant()
    {
        base.InitializeBossVariant();
        startPos = transform.localPosition;
        target.localPosition = startPos;
    }

    protected void OnEnable()
    {
        if (isInitialized) AppearDisappearBoss(true);
        isInitialized = true;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.layer == 6)
            collisionPoint = collision.ClosestPoint(transform.position); 
    }

    public void ActivateDeactivateBossMovement(bool isActivate)
    {
        if (isActivate) ChangeTargetPos();
        movable.SetMovableState(isActivate);
    }

    private void ChangeTargetPos()
    {
        if (target.localPosition == startPos) target.localPosition = Vector2.down * 20;
        else target.localPosition = startPos;
    }

    public void AppearDisappearBoss(bool isAppear)
    {
        if (!isAppear) ActivateDeactivateDeathZoneShield(false);
        if (!isPhantom) combustion.AppearDisappearByCombustion(isAppear);
        else gameObject.SetActive(isAppear);
    }

    public bool BossIsAppeared() => combustion.ObjIsAppeared();

    public bool BossIsMoving() => movable.IsMoving();

    public Vector3 GetCollisionPoint() => collisionPoint;

    public override void Death()
    {
        animator.SetTrigger("Kill");
        base.Death();
        AppearDisappearBoss(false);
    }

    public void SetSpeed(float speed) => this.speed = speed;

    public float GetStartSpeed() => speed;

    public Transform GetTarget() => target;

    public void ChangeBossCollideWithWalls(bool isCollideWithWalls)
    {
        movable.ChangeCollideSetting(isCollideWithWalls);
    }
}
