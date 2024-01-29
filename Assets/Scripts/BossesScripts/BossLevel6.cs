using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossLevel6 : Boss, IMovableBoss
{
    float startSpeed;
    Material material;
    Action<float> callback;

    protected override void Awake()
    {
        base.Awake();
        material = bossView.GetComponent<SpriteRenderer>().material;
        callback = value => material.SetFloat("_Dissolve", value);
    }

    protected override void InitializeBossVariant()
    {
        base.InitializeBossVariant();
        startSpeed = speed;
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

    public float GetStartSpeed()
    {
        return startSpeed;
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

    private void StartMelting(float time)
    {
        StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(material.GetFloat("_Dissolve")), 1, time, callback));
    }

    public override void Death()
    {
        animator.SetTrigger("Kill");
        StartMelting(2f);
        base.Death();
    }
}
