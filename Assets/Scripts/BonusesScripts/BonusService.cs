using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusService : MonoBehaviour
{
    [SerializeField] BonusConfig[] bonusConfigs;
    Dictionary<EffectType, Bonus> bonusDictionary;

    private void Awake()
    {
        InitializeBonusDictionary();
    }

    private void InitializeBonusDictionary()
    {
        bonusDictionary = new Dictionary<EffectType, Bonus>();

        foreach (var config in bonusConfigs) bonusDictionary[config.type] = config.bonus;
    }

    public Bonus GetRandomBonus()
    {
        var bonusConfig = RandomizerWithChances<BonusConfig>.RandomWithChances(bonusConfigs);
        if (bonusConfig == null) return null;
        return bonusConfig.bonus;
    }

    public Bonus GetBonus(EffectType effectType)
    {
        return bonusDictionary[effectType];
    }
}
