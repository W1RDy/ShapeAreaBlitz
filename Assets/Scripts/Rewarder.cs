using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewarder : MonoBehaviour
{
    [SerializeField] ResultShower resultShower;
    [SerializeField] GameService gameService;
    [SerializeField] Timer timer;
    [SerializeField] float maxTimeForMultiplyReward;
    CoinCounter coinsCounter;
    int defaultReward;
    int reward;
    int defaultScore;

    private void Awake()
    {
        defaultReward = gameService.defaultRewardOnLevel;
        defaultScore = gameService.defaultScoreOnLevel;
        coinsCounter = GameObject.Find("CoinCounter").GetComponent<CoinCounter>();
    }

    public int GetReward(bool isWin)
    {
        if (isWin)
        {
            var value = maxTimeForMultiplyReward / timer.GetTime();
            ScoreCounter.instance.AddScore((int)Mathf.Ceil(defaultScore * value));
            reward = (int)Mathf.Ceil(defaultReward * value);
            return reward;
        }
        else return GetDefaultReward();
    }

    public int GetDefaultReward()
    {
        return defaultReward;
    }

    public void CollectReward()
    {
        coinsCounter.ChangeCoins(reward);
    }

    public void MultiplyReward(float value)
    {
        var sumReward = (int)Mathf.Ceil(reward * value);
        reward = sumReward - reward;
        resultShower.ChangeReward(sumReward);
        CollectReward();
    }
}
