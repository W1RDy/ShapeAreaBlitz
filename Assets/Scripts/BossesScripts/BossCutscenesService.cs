using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCutscenesService : MonoBehaviour
{
    [SerializeField] BossCutsceneConfig[] cutsceneConfigs;
    Dictionary<(BossType type, bool isAppearCutscene) , BossCutsceneConfig> cutsceneDict;
    BossCutsceneConfig defaultCutscene;

    private void Awake()
    {
        InitializeCutsceneDictionary();
    }

    private void InitializeCutsceneDictionary()
    {
        cutsceneDict = new Dictionary<(BossType type, bool isAppearCutscene), BossCutsceneConfig> ();

        foreach (var config in cutsceneConfigs)
        {
            if (config.isDefaultCutscene) defaultCutscene = config;
            else cutsceneDict[(config.bossType, config.isAppearCutscene)] = config;
            config.InitializeTracks();
        }
    }

    public void InitializeCutsceneTracks(Boss boss, bool isDefault)
    {
        if (isDefault || boss.type == BossType.Tutorial)
        {
            var cutsceneConfig = GetDefaultCutscene();
            cutsceneConfig.InitializeAnimation(boss.transform);
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                var cutsceneConfig = GetCutscene(boss.type, System.Convert.ToBoolean(i));
                if (cutsceneConfig != null) cutsceneConfig.InitializeAnimation(boss.transform);
            }
        }
    }

    public BossCutsceneConfig GetCutscene(BossType bossType, bool isAppear)
    {
        if (cutsceneDict.ContainsKey((bossType, isAppear))) return cutsceneDict[(bossType, isAppear)];
        return null;
    }

    public BossCutsceneConfig GetDefaultCutscene() => defaultCutscene;
}
