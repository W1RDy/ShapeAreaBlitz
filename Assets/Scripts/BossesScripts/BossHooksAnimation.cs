using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHooksAnimation : MonoBehaviour
{
    [SerializeField] GameObject hookPrefab;
    BossLevel2 boss;
    Animator animator;
    bool isFinished = true;
    Combustion combustion;
    GameObject hooksObj;

    public void SetBoss(BossLevel2 boss) => this.boss = boss;

    public void ActivateHooksAnimation()
    {
        isFinished = false;
        AudioManager.instance.PlaySound("Whistle");
        boss.ChangeBossCollideWithWalls(true);
        boss.ActivateDeactivateBossMovement(true);
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        yield return new WaitWhile(boss.BossIsMoving);
        SpawnHooks();
        yield return new WaitUntil(combustion.ObjIsAppeared);
        animator.SetTrigger("Activate");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        combustion.AppearDisappearByCombustion(false);
        boss.AppearDisappearBoss(false);
        yield return new WaitUntil(combustion.ObjIsDisappeared);
        boss.ChangeBossCollideWithWalls(false);
        isFinished = true;
        Destroy(hooksObj);
    }

    public bool HooksAnimationIsFinished() => isFinished;

    private void SpawnHooks()
    {
        hooksObj = Instantiate(hookPrefab, boss.GetCollisionPoint(), Quaternion.identity);
        hooksObj.transform.SetParent(transform);

        combustion = GetComponentInChildren<Combustion>();
        combustion.InitializeCombustionMaterial(boss.isPhantom);
        combustion.AppearDisappearByCombustion(true);

        animator = GetComponentInChildren<Animator>();
    }
}
