using UnityEngine;
using UnityEngine.Playables;

public class FakeBossLevel4 : Boss
{
    EnemyService enemyService;
    BossActivator bossActivator;
    RetractableObjActivator blockForHitActivator;

    protected override void InitializeBossVariant()
    {
        base.InitializeBossVariant();
        enemyService = GameObject.Find("EnemyService").GetComponent<EnemyService>();
        bossActivator = GameObject.Find("BossServices").GetComponent<BossActivator>();
        blockForHitActivator = GameObject.Find("Objects/Room/BlocksForHitsActivator").GetComponent<RetractableObjActivator>();
    }

    public override void Death()
    {
        battleService.FinishBossBattle(true);
        bossActivator.appearingDelay = 0;
        enemyService.ChangeDestroyingEnemy(EnemyType.EnemyShooter, true, 3.2f);
        blockForHitActivator.ChangeCurrentBlockForHit(BossType.Processor);
        bossActivator.ActivateBoss(BossType.Processor);
        Destroy(gameObject);
    }
}
