using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockForHitService : MonoBehaviour
{
    [SerializeField] BlockForHitConfig[] blockConfigs;
    Dictionary<BossType, GameObject> blockDict;

    private void Awake()
    {
        InitializeBlockDictionary();
    }

    private void InitializeBlockDictionary()
    {
        blockDict = new Dictionary<BossType, GameObject>();

        foreach (var config in blockConfigs) blockDict[config.bossType] = config.blockForHitObj;
    }

    public GameObject GetBlockForHitObj(BossType bossType) => blockDict[bossType];
}
