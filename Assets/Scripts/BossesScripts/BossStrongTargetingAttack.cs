using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStrongTargetingAttack : BaseBossAttack
{
    [SerializeField] BossSummonAttack summonAttack;
    IMovableBoss movableBoss;
    Transform boss;
    Transform target;
    Transform player;

    public override void Awake() { }

    public override void InitializeAttack(Transform boss)
    {
        this.boss = boss;
        movableBoss = boss.GetComponent<IMovableBoss>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = boss.GetComponent<BossLevel5>().target;
    }

    public override void ActivateAttack()
    {
        isActivated = true;
        isFinishing = false;
        movableBoss.ChangeBossCollideWithWalls(false);
        summonAttack.ActivateAttack();
    }

    private void Update()
    {
        if (!isFinishing)
        {
            target.position = player.position;
            if (!movableBoss.BossIsMoving()) movableBoss.ActivateDeactivateBossMovement(true);
        }
    }

    public override void FinishAttack()
    {
        isFinishing = true;
        target.localPosition = Vector3.zero;
        movableBoss.ActivateDeactivateBossMovement(true);
        summonAttack.FinishAttack();
        StartCoroutine(WaitWhileReturnToZero());
    }

    private IEnumerator WaitWhileReturnToZero()
    {
        yield return new WaitUntil(IsOnZeroPos);
        isActivated = false;
    }

    private bool IsOnZeroPos() => boss.localPosition == Vector3.zero;
}
