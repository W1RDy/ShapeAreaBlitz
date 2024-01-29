using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireSplashAttack : BaseBossAttack
{
    [SerializeField] float defaultCooldown = 1;
    [SerializeField, ReadOnly] float cooldown;
    [SerializeField] FireSplash fireSplash;
    [SerializeField] PointService cornerService;
    GameObject fireSplashObj;

    public override void Awake()
    {
        ChangeValueByDifficulty = () => cooldown = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultCooldown);
        base.Awake();
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            StartCoroutine(SpawnFireSplashes());
            yield return new WaitWhile(FireSplashIsActivated);
            if (isFinishing) break;
        }
        isActivated = false;
    }

    private IEnumerator SpawnFireSplashes()
    {
        AudioManager.instance.PlaySound("Fire");
        for (var i = 0; i < 3; i++)
        {
            var corner = cornerService.GetRandomPoint();
            fireSplash.SetTarget(corner);
            fireSplashObj = Instantiate(fireSplash.gameObject, Vector2.zero, Quaternion.identity);
            yield return null;
        }
        cornerService.GetRandomPoint();
    }

    private bool FireSplashIsActivated() => fireSplashObj != null;
}
