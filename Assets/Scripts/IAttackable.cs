using System.Collections;
using UnityEngine;

public interface IAttackable
{
    public void ActivateAttack();
    public void FinishAttack();
    public bool AttackIsActivated();
    public void InitializeAttack(Transform boss);
}
