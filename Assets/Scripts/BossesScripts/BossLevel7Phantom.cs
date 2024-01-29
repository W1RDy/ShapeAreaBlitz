using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel7Phantom : Boss
{
    [SerializeField] float reincarnationCooldown;
    [SerializeField] float reincarnationDelay;
    [SerializeField] ParticleSystem magicEffect;
    BossActivator bossActivator;
    Boss currentBoss;
    bool isReincarnating;

    protected override void InitializeBossVariant()
    {
        base.InitializeBossVariant();
        bossActivator = GameObject.Find("BossServices").GetComponent<BossActivator>();
        isReincarnating = true;
        StartCoroutine(Reincarnation());
    }

    private IEnumerator Reincarnation()
    {
        while (true)
        {
            yield return new WaitForSeconds(reincarnationCooldown);
            yield return StartCoroutine(StartReincarnation(reincarnationDelay));
            yield return new WaitWhile(IsReincarnated);
            Destroy(Instantiate(magicEffect.gameObject), 5f);
            battleService.SetBoss(this);
            ChangeHp(-1);
            if (!isReincarnating) break;
        }
    }

    private bool IsReincarnated() => currentBoss != null;

    private IEnumerator StartReincarnation(float reincarnationDelay)
    {
        ActivateDeactivateDeathZoneShield(false);
        animator.SetTrigger("Reincarnate");
        yield return new WaitForSeconds(reincarnationDelay);
        Reincarnate();
        yield return null;
    }
    
    public override void Death()
    {
        isReincarnating = false;
        cutscenesActivator.ActivateCutscene(this, false);
        base.Death();
    }

    private void Reincarnate()
    {
        AudioManager.instance.PlaySound("Magic");
        Destroy(Instantiate(magicEffect.gameObject), 5f);
        currentBoss = bossActivator.ActivatePhantomBoss();
    }
}
