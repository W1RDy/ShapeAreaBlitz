using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    int coins;
    [SerializeField] CoinIndicator[] coinIndicators;
    Action<int> UpdateByBuyingSkin;

    private void Awake()
    {
        //coins = PlayerPrefs.GetInt("Coins", 0);
        coins = DataContainer.Instance.playerData.coins;
    }

    private void Start()
    {
        ChangeCoinIndicators();
        UpdateByBuyingSkin = skinCost => ChangeCoins(-skinCost);
        SkinManager.instance.SkinIsBought += UpdateByBuyingSkin;
    }

    public int GetCoins() => coins;

    public void ChangeCoins(int value)
    {
        coins += value;
        DataContainer.Instance.playerData.coins = coins;
        DataContainer.Instance.SaveDataOnServer();
        //PlayerPrefs.SetInt("Coins", coins);
        ChangeCoinIndicators();
    }

    private void ChangeCoinIndicators()
    {
        foreach (var coinIndicator in coinIndicators)
            coinIndicator.ChangeCoinIndicator(coins);
    }

    private void OnDestroy()
    {
        SkinManager.instance.SkinIsBought -= UpdateByBuyingSkin;
    }
}
