using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BossActivator : MonoBehaviour
{
    [SerializeField] public float appearingDelay = 1;
    [SerializeField] BossBattleService battleService;
    [SerializeField] BossService bossService;
    [SerializeField] Pool bossPool;
    [SerializeField] BossCutscenesActivator cutsceneActivator;
    [SerializeField] GameService gameService;

    public void ActivateBoss(BossType bossType)
    {
        var boss = bossService.GetBoss(bossType);
        StartCoroutine(BossAppearing(boss));
    }

    public Boss ActivatePhantomBoss()
    {
        var randomBoss = bossService.GetRandomBoss();
        var boss = Instantiate(randomBoss.gameObject).GetComponent<Boss>();
        boss.isPhantom = true;
        return boss;
    }

    private IEnumerator BossAppearing(Boss boss)
    {
        try { AudioManager.instance.PlaySound(boss.gameObject.name); }
        catch { }

        yield return new WaitForSeconds(appearingDelay);
        Boss currentBoss;
        try
        {
            currentBoss = bossPool.GetPool(boss.type).GetFreeElement() as Boss;
        }
        catch
        {
            currentBoss = Instantiate(boss).GetComponent<Boss>();
        }
        if (currentBoss.type == BossType.Tutorial) cutsceneActivator.ActivateCutscene(currentBoss, true, true);
        else cutsceneActivator.ActivateCutscene(currentBoss, true);
        Invoke(nameof(ActivateBossBattle), cutsceneActivator.GetCutsceneDuration());
    }

    private void ActivateBossBattle()
    {
        gameService.isBossStage = true;
        if (!gameService.isTutorial) battleService.ActivateBossBattle();
    }
}
