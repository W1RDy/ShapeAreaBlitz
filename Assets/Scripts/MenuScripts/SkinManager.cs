using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField] Skin[] skinConfigs;
    [SerializeField] int skinRememberCooldown = 4;
    Dictionary<int, Skin> skinConfigsDict;
    //int indexSkinEquipped = 0;
    Skin equippedSkin;
    int skinRememberCdCount = 2;
    RecommendedSkin recommendedSkin;
    CoinCounter coinCounter;
    Rewarder rewarder;
    public event Action<int> SkinIsBought;
    public event Action<(Skin oldSkin, Skin newSkin)> SkinIsEquipped;

    public static SkinManager instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        InitializeSkinDictionary();
        instance.coinCounter = GameObject.Find("CoinCounter").GetComponent<CoinCounter>();
        try { instance.rewarder = GameObject.Find("Rewarder").GetComponent<Rewarder>(); }
        catch { }
    }

    private void InitializeSkinDictionary()
    {
        skinConfigsDict = new Dictionary<int, Skin>();
        ////indexSkinEquipped = PlayerPrefs.GetInt("EquippedSkin", 0);

        //foreach (var skinConfig in skinConfigs)
        //{
        //    string defaultValue = skinConfig.index > 0 ? "false" : "true";
        //    bool skinIsPurchased = System.Convert.ToBoolean(PlayerPrefs.GetString(skinConfig.index + "SkinPurchased", defaultValue));
        //    skinConfig.SetState(skinIsPurchased, false);

        //    if (indexSkinEquipped == skinConfig.index)
        //    {
        //        skinConfig.SetState(true, true);
        //        equippedSkin = skinConfig;
        //    }
        //    skinConfigsDict[skinConfig.index] = skinConfig;
        //}

        foreach (var skin in DataContainer.Instance.playerData.purchasedSkins)
        {
            foreach (var skinConfig in skinConfigs)
            {
                if (skinConfig.index == skin.skinIndex)
                {
                    skinConfig.isPurchased = true;
                    skinConfig.isEnabled = skin.isEnabled;
                    if (skinConfig.isEnabled) equippedSkin = skinConfig;
                    break;
                }
            }
        }

        foreach (var skinConfig in skinConfigs) skinConfigsDict[skinConfig.index] = skinConfig;
    }

    public Skin GetEquippedSkin() => equippedSkin;

    public void BuySkin(Skin skin)
    {
        BuySkinOnCost(skin, skin.cost);
    }

    public void BuySkinOnCost(Skin skin, int cost)
    {       
        if (coinCounter.GetCoins() >= cost)
        {
            //PlayerPrefs.SetString(skinIndex + "SkinPurchased", "true");
            ScoreCounter.instance.AddScore(skin.cost / 10);
            SkinIsBought?.Invoke(cost);
            DataContainer.Instance.playerData.purchasedSkins.Add(new PlayerSkin(skin.index));
            EquipSkin(skin);
        }
    }

    public void BuyRecommendedSkin()
    {
        skinRememberCdCount = 0;
        BuySkinOnCost(recommendedSkin.skin, recommendedSkin.recommendedCost);
    }

    public void EquipSkin(Skin skin)
    {
        equippedSkin.SetState(true, false);
        //PlayerPrefs.SetInt("EquippedSkin", skinIndex);
        skin.SetState(true, true);
        foreach (var skinInContainer in DataContainer.Instance.playerData.purchasedSkins)
        {
            if (skinInContainer.skinIndex == skin.index) skinInContainer.isEnabled = true;
            else skinInContainer.isEnabled = false;
        }
        SkinIsEquipped?.Invoke((equippedSkin, skin));
        equippedSkin = skin;
        DataContainer.Instance.SaveDataOnServer();
    }

    public Skin GetSkin(int index) => skinConfigsDict[index];

    private RecommendedSkin RecommendSkinInCostRange(int minCost, int maxCost, bool isSkinWithADV)
    {
        foreach (var skin in skinConfigsDict.Values)
        {
            if (minCost + 4 < skin.cost && skin.cost <= maxCost && !skin.isPurchased && SkinIsInteresting(isSkinWithADV, skin))
            {
                var recommendedCost = isSkinWithADV ? minCost : skin.cost;
                recommendedSkin = new RecommendedSkin(skin, isSkinWithADV, recommendedCost);
            }
            else if (skin.cost > maxCost)
            {
                return recommendedSkin;
            }
        }
        return recommendedSkin;
    }

    public RecommendedSkin GetRecommendedSkin(bool isSkinWithADV, bool withChecking)
    {
        if (withChecking || recommendedSkin == null)
        {
            recommendedSkin = null;
            var currentCoins = coinCounter.GetCoins();
            if (isSkinWithADV) recommendedSkin = RecommendSkinInCostRange(currentCoins, currentCoins + rewarder.GetDefaultReward(), true);
            if (recommendedSkin == null) recommendedSkin = RecommendSkinInCostRange(0, currentCoins, false);
        }
        if (recommendedSkin != null) skinRememberCdCount = 2;
        return recommendedSkin;
    }

    private bool SkinIsInteresting(bool isSkinWithADV, Skin skin)
    {
        if (isSkinWithADV) return skin.isInterestingToPurchaseWithADV;
        return skin.isInterestingToPurchase;
    }

    public bool CanRememberAboutSkin()
    {
        skinRememberCdCount++;
        return skinRememberCdCount >= skinRememberCooldown;
    }

    public void SetSkinIsntInterested()
    {
        if (recommendedSkin.isRecommendedWithADV) recommendedSkin.skin.isInterestingToPurchaseWithADV = false;
        else recommendedSkin.skin.isInterestingToPurchase = false;
    }
}

public class RecommendedSkin
{
    public Skin skin;
    public bool isRecommendedWithADV;
    public int recommendedCost;

    public RecommendedSkin(Skin _skin, bool _isRecommendedWithADV, int _recommendedCost)
    {
        skin = _skin;
        isRecommendedWithADV = _isRecommendedWithADV;
        recommendedCost = _recommendedCost;
    }
}
