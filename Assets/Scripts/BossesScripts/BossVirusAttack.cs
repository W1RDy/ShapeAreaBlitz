using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BossVirusAttack : BaseBossAttack
{
    [SerializeField, FormerlySerializedAs("moveCooldown")] float defaultMoveCooldown;
    [SerializeField, ReadOnly] float moveCooldown;
    [SerializeField, FormerlySerializedAs("spawnCooldown")] float defaultSpawnCooldown;
    [SerializeField, ReadOnly] float spawnCooldown;
    [SerializeField] float virusSpeedDifference;
    [SerializeField] Pool pool;
    GameService gameService;
    IMovableBoss movableBoss;
    Transform boss;
    Transform target;


    public override void Awake()
    {
        ChangeValueByDifficulty = () => moveCooldown = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultMoveCooldown);
        base.Awake();
        ChangeValueByDifficulty = () => spawnCooldown = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultSpawnCooldown);
        base.Awake();
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
    }

    public override void InitializeAttack(Transform boss)
    {
        this.boss = boss;
        movableBoss = boss.GetComponent<IMovableBoss>();
        target = movableBoss.GetTarget();
    }

    public override void ActivateAttack()
    {
        gameService.generalGameSpeed /= virusSpeedDifference;
        base.ActivateAttack();
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            target.position = GetTargetPoint();
            TrajectoryShower.instance.ShowTrajectory(boss.position, target.position, moveCooldown);
            yield return new WaitForSeconds(moveCooldown);
            movableBoss.ActivateDeactivateBossMovement(true);
            StartCoroutine(SpawnViruses());
            yield return new WaitWhile(movableBoss.BossIsMoving);
            if (isFinishing) break;
        }
        target.localPosition = Vector3.zero;
        movableBoss.ActivateDeactivateBossMovement(true);
        yield return new WaitWhile(movableBoss.BossIsMoving);
        yield return new WaitForSeconds(0.1f);
        gameService.generalGameSpeed *= virusSpeedDifference;
        isActivated = false;
    }

    private IEnumerator SpawnViruses()
    {
        while (movableBoss.BossIsMoving())
        {
            yield return new WaitForSeconds(spawnCooldown);
            var virus = pool.GetPool(EnemyType.Virus).GetFreeElement();
            virus.transform.position = boss.position;
            virus.GetComponent<Enemy>().InitializeEnemyVariant();
        }
    }

    private Vector2 GetTargetPoint()
    {
        var newPos = new Vector2(Random.Range(-8, 8), Random.Range(-6, 6));
        var distance = Vector2.Distance(boss.position, newPos);
        if (distance < 4f) return GetTargetPoint();
        return newPos;
    }
}
