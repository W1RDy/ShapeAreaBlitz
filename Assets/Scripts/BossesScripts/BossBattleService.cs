using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleService : MonoBehaviour
{
    [SerializeField] public bool isActivated;
    [SerializeField] float waitTime;
    [SerializeField] float hitTime;
    [SerializeField] EventActivator eventActivator;
    [SerializeField] BossAttackActivator attackActivator;
    [SerializeField] RetractableObjActivator blockForHitActivator;
    GameService gameService;
    Boss boss;
    Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
    }

    public void SetBoss(Boss boss) => this.boss = boss;

    public void ActivateBossBattle()
    {
        if (!isActivated)
        {
            isActivated = true;
            ActivateDeactivateSpawners(true);
            StartCoroutine(Battle());
        }
    }

    private IEnumerator Battle()
    {
        boss.ReturnToIdle();
        while (true)
        {
            yield return new WaitForSeconds(hitTime);
            boss.SetVulnerable(false); 

            yield return new WaitForSeconds(waitTime - hitTime);
            ActivateDeactivateSpawners(false);
            eventActivator.ActivateEvent(EventType.DestroyAllEnemies);
            attackActivator.ActivateAttack();

            yield return new WaitUntil(boss.BossIsWaiting);
            ActivateDeactivateSpawners(true);

            if (gameService.isTutorial)
            {
                UnavailableDirectionsManager.instance.AddAllDirectionsExcept(null);
                yield return new WaitWhile(player.GetComponent<IMovable>().IsMoving);
                blockForHitActivator.ActivateClosestRetractableObj(hitTime);
            }
            else blockForHitActivator.ActivateRetractableObj(hitTime);

            boss.SetVulnerable(true);
            if (gameService.isTutorial) FinishBossBattle(true);
        }
    }

    public void BossTakeHit()
    {
        player.ActivateShieldForTime(0.2f);
        blockForHitActivator.DestroyRetractableObj();
        boss.SetVulnerable(false);
    }

    private void ActivateDeactivateSpawners(bool isActivate)
    {
        ActivateDeactivateSpawner(SpawnerType.MainEnemySpawner, isActivate);
        ActivateDeactivateSpawner(SpawnerType.AdditiveEnemySpawner, isActivate);
        if (isActivate && !gameService.isTutorial) ActivateDeactivateSpawner(SpawnerType.BonusSpawner, isActivate);
    }

    private void ActivateDeactivateSpawner(SpawnerType spawnerType, bool isActivate)
    {
        if (isActivate) eventActivator.ActivateEvent(EventType.ActivateSpawner, (int)spawnerType);
        else eventActivator.DeactivateEvent(EventType.ActivateSpawner, (int)spawnerType);
    }

    public bool IsActivated() => isActivated;

    public void FinishBossBattle(bool isDisable)
    {
        isActivated = false;
        StopAllCoroutines();
        ActivateDeactivateSpawners(false);
        eventActivator.ActivateEvent(EventType.DestroyAllEnemies);
        if (isDisable) attackActivator.DisableAttack();
        else attackActivator.PauseAttack();
    }
}
