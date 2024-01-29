using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjectsChanger : MonoBehaviour
{
    [SerializeField] SkinMenuConfig[] skinConfigs;
    [SerializeField] LevelMenuConfig[] levelConfigs;
    Dictionary<int, SkinMenuConfig> skinConfigsDict;
    Action<(Skin oldSkin, Skin newSkin)> ChangeStatesByEquip;

    private void Start()
    {
        InitializeSkinDictionary();
        InitializeLevels();
        ChangeStatesByEquip = skins =>
        {
            ChangeSkinState(skins.oldSkin, skinConfigsDict[skins.oldSkin.index]);
            ChangeSkinState(skins.newSkin, skinConfigsDict[skins.newSkin.index]);
        };
        SkinManager.instance.SkinIsEquipped += ChangeStatesByEquip;
    }

    private void InitializeSkinDictionary()
    {
        skinConfigsDict = new Dictionary<int, SkinMenuConfig>();

        foreach (var skinConfig in skinConfigs)
        {
            var skin = SkinManager.instance.GetSkin(skinConfig.index);
            skinConfig.skinFrame.SetSkinCost(skin.cost);
            skinConfig.skinFrame.SetSkinImage(skin.skinImage);
            skinConfigsDict[skinConfig.index] = skinConfig;
            ChangeSkinState(skin, skinConfig);
        }
    }

    private void InitializeLevels()
    {
        foreach (var levelConfig in levelConfigs)
        {
            var levelState = LevelManager.instance.GetLevelState(levelConfig.index);
            if (levelState.isActive) levelConfig.levelIcon.ActivateIcon();
        }
    }

    public void ChangeSkinState(Skin skin, SkinMenuConfig skinConfig)
    {
        if (skin.isEnabled) skinConfig.skinFrame.SetSkinState("Equipped");
        else if (skin.isPurchased) skinConfig.skinFrame.SetSkinState("Purchased");
        else skinConfig.skinFrame.SetSkinState("Buy");
    }

    public void ChangeSkinState(int skinIndex)
    {
        var skin = SkinManager.instance.GetSkin(skinIndex);
        ChangeSkinState(skin, skinConfigsDict[skinIndex]);
    }

    private void OnDestroy()
    {
        SkinManager.instance.SkinIsEquipped -= ChangeStatesByEquip;
    }
}
