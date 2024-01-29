using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEnemy : Enemy
{
    EffectActivator effectActivator;

    public override void Awake()
    {
        base.Awake();
        effectActivator = GameObject.Find("EffectActivator").GetComponent<EffectActivator>();
        var snowParticles = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
        snowParticles.sortingOrder = index + 1;
    }

    public override void InitializeEnemyVariant()
    {
        base.InitializeEnemyVariant();
        effectActivator.ActivateEffect(EffectType.Freeze, true);
    }

    public override void SetEnemyType()
    {
        type = EnemyType.FreezeEnemy;
    }
}
