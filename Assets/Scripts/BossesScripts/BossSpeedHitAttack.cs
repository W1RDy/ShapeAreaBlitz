using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossSpeedHitAttack : BaseBossAttack
{
    [SerializeField, FormerlySerializedAs("cooldown")] float defaultCooldown;
    [SerializeField, ReadOnly] float cooldown;
    Transform boss;
    IMovableBoss movableBoss;
    Transform target;
    Transform player;

    public override void Awake()
    {
        ChangeValueByDifficulty = () => cooldown = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultCooldown);
        base.Awake();
    }

    public override void InitializeAttack(Transform boss)
    {
        this.boss = boss;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = boss.GetComponent<BossLevel3>().target;
        movableBoss = boss.GetComponent<IMovableBoss>();
    }

    public override void ActivateAttack()
    {
        isActivated = true;
        isFinishing = false;
        movableBoss.ChangeBossCollideWithWalls(true);
        target.localPosition = Vector3.zero;
        StartCoroutine(Attack());
    }

    public override IEnumerator Attack()
    {
        while(true)
        {
            if (boss.transform.localPosition == Vector3.zero) yield return new WaitForSeconds(cooldown);
            MoveBoss();
            yield return new WaitWhile(movableBoss.BossIsMoving);
            if (isFinishing && boss.transform.localPosition == Vector3.zero) break;
        }
        isActivated = false;
    }

    private void MoveBoss()
    {
        ChangeTargetPos();
        movableBoss.ActivateDeactivateBossMovement(true);
    }

    private void ChangeTargetPos()
    {
        if (target.localPosition == Vector3.zero) target.position = player.position;
        else target.localPosition = Vector3.zero;
    }
}
