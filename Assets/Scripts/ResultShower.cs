using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultShower : MonoBehaviour
{
    [SerializeField] Timer timer;
    [SerializeField] Rewarder rewarder;
    [SerializeField] CustomText timeScore;
    [SerializeField] Text rewardCoins;

    private void Start()
    {
        var isWin = GetComponent<Window>().type == WindowType.WinWindow ? true : false;
        ShowResults(isWin);        
    }

    public void ShowResults(bool isWin)
    {
        if (timeScore) ShowTimer();
        if (rewardCoins) ShowReward(isWin);
    }

    private void ShowTimer() => timeScore.SetMessage("Your time:", " " + timer.GetTime());

    private void ShowReward(bool isWin) => rewardCoins.text = "+" + rewarder.GetReward(isWin);

    public void ChangeReward(int sumReward)
    {
        rewardCoins.text = "+" + sumReward;
    }
}
