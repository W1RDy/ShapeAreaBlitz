using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BossSlideAttack : BaseBossAttack
{
    [SerializeField, FormerlySerializedAs("newSpeed")] float defaultNewSpeed;
    [SerializeField, ReadOnly] float newSpeed;
    [SerializeField] float hitCooldown;
    [SerializeField] bool isCountAttack;
    [SerializeField] bool isSlide;
    [SerializeField, ShowIf(nameof(isCountAttack))] int[] movementCountInterval;
    IMovableBoss movableBoss;
    Transform boss;
    int movementCount;
    int counter;
    Transform target;
    float angleOffset;
    Action<float> callback;

    public override void Awake()
    {
        ChangeValueByDifficulty = () =>
        {
            newSpeed = ChangerValueByDifficulty.instance.GetValueByDifficult(true, defaultNewSpeed);
        };
        base.Awake();
    }

    public override void InitializeAttack(Transform boss)
    {
        movableBoss = boss.GetComponent<IMovableBoss>();
        target = movableBoss.GetTarget();
        callback = angle => boss.transform.eulerAngles = new Vector3(0, 0, angle);
        this.boss = boss;
    }

    public override void ActivateAttack()
    {
        isActivated = true;
        isFinishing = false;
        movableBoss.ChangeBossCollideWithWalls(true);
        angleOffset = 90;
        target.position = Vector2.down * 20;
        movableBoss.SetSpeed(newSpeed);
        movableBoss.ActivateDeactivateBossMovement(true);
        if (isCountAttack) RandomizeMovementCount();
        StartCoroutine(Attack());
    }

    public override IEnumerator Attack()
    {
        yield return new WaitWhile(movableBoss.BossIsMoving);
        while (true)
        {
            yield return new WaitForSeconds(hitCooldown);
            MoveBoss();
            yield return new WaitWhile(movableBoss.BossIsMoving);
            if (isFinishing) break;
            if (isCountAttack && counter == movementCount) Flip();
            else RotateBoss();
            movableBoss.SetSpeed(newSpeed);
        }
        movableBoss.ChangeBossCollideWithWalls(false);
        RotateBoss();
        target.localPosition = Vector3.zero;
        movableBoss.SetSpeed(movableBoss.GetStartSpeed());
        movableBoss.ActivateDeactivateBossMovement(true);
        yield return new WaitWhile(movableBoss.BossIsMoving);
        if (boss.localScale.x < 0) Flip();
        isActivated = false;
    }

    private void RotateBoss()
    {
        var newAngle = boss.eulerAngles.z + angleOffset;
        if (isFinishing) newAngle = 0;
        StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(boss.eulerAngles.z), newAngle, hitCooldown, callback));
    }

    private void MoveBoss()
    {
        var direction = boss.localScale.x > 0 ? Vector2.right : Vector2.left;
        var offset = boss.TransformDirection(direction) * 20;
        target.position = boss.position + offset;
        counter++;
        movableBoss.ActivateDeactivateBossMovement(true);

        if (!isSlide) AudioManager.instance.PlaySound("Wheel");
        else AudioManager.instance.PlaySound("Slide");
    }

    private void Flip()
    {
        boss.localScale = new Vector2 (-1 * boss.localScale.x, boss.localScale.y);
        angleOffset = -1 * angleOffset;
        RandomizeMovementCount();
    }

    private void RandomizeMovementCount()
    {
        movementCount = Random.Range(movementCountInterval[0], movementCountInterval[1] + 1);
        counter = 0;
    }
}
