using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackActivator : MonoBehaviour
{
    Boss boss;
    BossAttack currentAttack;

    public void SetBoss(Boss boss) => this.boss = boss;

    public void ActivateAttack()
    {
        if (currentAttack == null) currentAttack = boss.GetRandomAttack();
        currentAttack.attackable.ActivateAttack();
        StartCoroutine(AttackTimer());
    }

    private IEnumerator AttackTimer()
    {
        boss.ChangeAttackState(currentAttack.GetIndex());
        yield return new WaitForSeconds(currentAttack.duration);
        boss.ChangeAttackState(0);
        currentAttack.attackable.FinishAttack();
        boss.stateType = BossStateType.ReturningToWaitState;
        yield return new WaitWhile(currentAttack.attackable.AttackIsActivated);
        DisableAttack();
    }

    public void DisableAttack()
    {
        StopAllCoroutines();
        if (currentAttack != null && currentAttack.attackable.AttackIsActivated()) currentAttack.attackable.FinishAttack();
        boss.stateType = BossStateType.Waiting;
        currentAttack = null;
    }

    public void PauseAttack()
    {
        StopAllCoroutines();
        if (currentAttack != null && currentAttack.attackable.AttackIsActivated()) currentAttack.attackable.FinishAttack();
    }
}
