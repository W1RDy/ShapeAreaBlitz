using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossService : MonoBehaviour
{
    [SerializeField] BossConfig[] bossRandomizedConfigs;
    [SerializeField] BossConfig[] bossConfigs;
    Dictionary<BossType, BossConfig> bossDict;

    private void Awake()
    {
        InitializeBossDictionary();
    }

    private void InitializeBossDictionary()
    {
        bossDict = new Dictionary<BossType, BossConfig>();
        
        foreach (var bossConfig in bossConfigs) bossDict[bossConfig.bossType] = bossConfig;
        foreach (var bossConfig in bossRandomizedConfigs) bossDict[bossConfig.bossType] = bossConfig;
    }

    public Boss GetBoss(BossType bossType) => bossDict[bossType].boss;

    public Boss GetRandomBoss()
    {
        return RandomizerWithChances<BossConfig>.RandomWithChances(bossRandomizedConfigs).boss;
    }
}
