using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsCollector : MonoBehaviour
{
    [SerializeField] CoinCounter coinsCounter;
    [SerializeField] CoinsEffect coinsEffect;
    [SerializeField] Pool coinsEffectPool;
    GameService gameService;
    int coinsMultiplier = 1;
    int collectedCoins;

    private void Awake()
    {
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
    }

    public void CollectCoins(int coins, Vector3 collectPoint)
    {
        coins *= coinsMultiplier;
        var collectEffect = coinsEffectPool.GetPool(coinsEffect.GetType()).GetFreeElement() as CoinsEffect;
        collectEffect.transform.position = collectPoint;
        if (collectedCoins < gameService.maxCoinsInLevel)
        {
            ScoreCounter.instance.AddScore(coins * 10 / coinsMultiplier);
            collectEffect.ChangeCoinsText(coins);
            coinsCounter.ChangeCoins(coins);
            collectedCoins += coins;
        }
        else collectEffect.SetLimitText();
    }

    public void MultiplyCoins(float value)
    {
        coinsMultiplier = (int)(coinsMultiplier * value);
    }
}
