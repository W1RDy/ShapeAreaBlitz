using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseBossAttack : MonoBehaviour, IAttackable
{
    [HideInInspector] public bool isActivated;
    [HideInInspector] public bool isFinishing = true;
    public Action ChangeValueByDifficulty;

    public virtual void Awake()
    {
        GameObject.Find("GameService").GetComponent<GameService>().SetLevelDifficulty += ChangeValueByDifficulty;
        isFinishing = true;
    }

    public virtual void ActivateAttack()
    {
        isActivated = true;
        isFinishing = false;
        StartCoroutine(Attack());
    }

    public virtual bool AttackIsActivated() => isActivated;

    public virtual void FinishAttack() => isFinishing = true;

    public virtual void InitializeAttack(Transform boss) { }

    public virtual IEnumerator Attack()
    {
        yield return null;
    }
}
