using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSpeedModeAttack : BaseBossAttack
{
    [SerializeField] float defaultAttackSpeed = 60;
    [SerializeField, ReadOnly] float attackSpeed;
    [SerializeField] float attackDelay = 2;
    IMovableBoss movableBoss;
    Transform boss;
    Transform player;
    Transform target;

    public override void Awake()
    {
        ChangeValueByDifficulty = () =>
        {
            attackSpeed = ChangerValueByDifficulty.instance.GetValueByDifficult(true, defaultAttackSpeed);
            if (movableBoss != null && isActivated) movableBoss.SetSpeed(attackSpeed);
        };
        base.Awake();
    }

    public override void InitializeAttack(Transform boss)
    {
        this.boss = boss;
        movableBoss = boss.GetComponent<IMovableBoss>();
        target = movableBoss.GetTarget();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void ActivateAttack()
    {
        isActivated = true;
        isFinishing = false;
        movableBoss.ChangeBossCollideWithWalls(false);
        movableBoss.SetSpeed(attackSpeed);
        target.localPosition = Vector2.right * 20f;
        movableBoss.ActivateDeactivateBossMovement(true);
        StartCoroutine(Attack());
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitUntil(BossOnTarget);

            if (!isFinishing) RandomizeStartPosition();
            else if (target.localPosition == Vector3.zero) break;
            else movableBoss.SetSpeed(movableBoss.GetStartSpeed());

            SetTargetPosition();
            TrajectoryShower.instance.ShowTrajectory(boss.position, target.position, attackDelay);
            yield return new WaitWhile(TrajectoryShower.instance.TrajectoryIsShowed);
            movableBoss.ActivateDeactivateBossMovement(true);
            yield return new WaitForSeconds(0.1f);
            AudioManager.instance.PlaySound("Sweep");
        }
        movableBoss.ChangeBossCollideWithWalls(false);
        isActivated = false;
    }

    private bool BossOnTarget() => boss.localPosition == target.localPosition;

    private void RandomizeStartPosition()
    {
        target.localPosition = new Vector2(RandomizeCoordinate(20, 30), RandomizeCoordinate(10, 20));
        boss.localPosition = target.localPosition;
    }

    private void SetTargetPosition()
    {
        if (!isFinishing)
            target.position = new Vector2(boss.position.x, boss.position.y) + DirectionService.GetDirectionToTarget(boss, player) * 50f;
        else target.localPosition = Vector3.zero;
    }

    private float RandomizeCoordinate(float minValue, float maxValue)
    {
        var coordinate = (Random.value < 0.5f ? 1 : -1) * Random.Range(minValue, maxValue);
        return coordinate;
    }
}
