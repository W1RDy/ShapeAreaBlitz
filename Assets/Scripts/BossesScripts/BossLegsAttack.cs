using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossLegsAttack : BaseBossAttack
{
    [SerializeField, FormerlySerializedAs("spawnCooldown")] float defaultSpawnCooldown;
    [SerializeField] float spawnCooldown;
    [SerializeField] int legsCount;
    [SerializeField] RetractableObjActivator legsActivator;
    Boss boss;
    Transform player;
    Transform legsSpawnPoint;
    int counter;

    public override void Awake()
    {
        ChangeValueByDifficulty = () => spawnCooldown = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultSpawnCooldown);
        base.Awake();
    }

    public override void InitializeAttack(Transform boss) 
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        legsSpawnPoint = boss.GetComponent<BossLevel7>().legsSpawnPoint;
        legsActivator.spawnPoint = legsSpawnPoint;
        this.boss = boss.GetComponent<Boss>();
    }

    public override void ActivateAttack()
    {
        var waitingTime = boss.animator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(WaitBeforeAttack(waitingTime));
    }

    private IEnumerator WaitBeforeAttack(float time)
    {
        yield return new WaitForSeconds(time);
        isActivated = true;
        isFinishing = false;
        StartCoroutine (Attack());
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            legsSpawnPoint.rotation = AngleService.GetAngleByTarget(legsSpawnPoint, player);
            yield return new WaitForSeconds(spawnCooldown);
            legsActivator.ActivateRetractableObj(spawnCooldown * (legsCount - counter));
            counter++;
            if (counter == legsCount)
            {
                counter = 0;
                yield return new WaitWhile(legsActivator.IsActivated);
                if (isFinishing) break;
            }
        }
        isActivated = false;
    }
}
