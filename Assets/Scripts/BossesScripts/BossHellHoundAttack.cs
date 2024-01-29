using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHellHoundAttack : BaseBossAttack
{
    [SerializeField] float defaultCooldown;
    [SerializeField, ReadOnly] float cooldown;
    [SerializeField] float defaultHitDuration;
    [SerializeField, ReadOnly] float hitDuration;
    [SerializeField] HellHound hellHoundPrefab;
    [SerializeField] PointService pointService;
    [SerializeField] Transform target;
    BossLevel2 boss;
    BossHooksAnimation hooksAnimation;
    HellHound hellHound;

    public override void Awake()
    {
        ChangeValueByDifficulty = () => cooldown = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultCooldown);
        base.Awake();
        ChangeValueByDifficulty = () => hitDuration = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultHitDuration);
        base.Awake();
    }

    public override void InitializeAttack(Transform boss)
    {
        this.boss = boss.GetComponent<BossLevel2>();
        hellHoundPrefab.target = target;
        hooksAnimation = GetComponentInChildren<BossHooksAnimation>();
        hooksAnimation.SetBoss(this.boss);
    }

    public override void ActivateAttack()
    {
        hooksAnimation.ActivateHooksAnimation();
        base.ActivateAttack();
    }

    public override IEnumerator Attack()
    {
        yield return new WaitUntil(hooksAnimation.HooksAnimationIsFinished);
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            SpawnHellHound();
            hellHound.ActivateHellHoundMove();
            yield return new WaitWhile(hellHound.IsMoving);
            yield return new WaitForSeconds(hitDuration);
            hellHound.ActivateHellHoundMove();
            yield return new WaitWhile(hellHound.IsMoving);
            Destroy(hellHound.gameObject);
            if (isFinishing) break;
        }
        boss.ActivateDeactivateBossMovement(true);
        boss.AppearDisappearBoss(true);
        yield return new WaitWhile(boss.BossIsMoving);
        yield return new WaitUntil(boss.BossIsAppeared);
        isActivated = false;
    }

    private void SpawnHellHound()
    {
        var spawnPoint = pointService.GetRandomPoint();
        hellHound = Instantiate(hellHoundPrefab, spawnPoint.position, Quaternion.identity).GetComponent<HellHound>();
        hellHound.transform.SetParent(boss.transform.parent);
    }
}
