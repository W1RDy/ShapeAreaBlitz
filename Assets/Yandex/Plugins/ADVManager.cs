using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADVManager : MonoBehaviour
{
    public static ADVManager Instance;
    private int advertiseCooldown = 3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }

    public void ShowADV()
    {
        if (advertiseCooldown <= 1)
        {
            InteractorWithBrowser.ShowAdversity();
            advertiseCooldown = 3;
        }
        else advertiseCooldown--;
    }

    public void ShowADVForCoins(float value)
    {
        InteractorWithBrowser.MultiplyCoinsForAdv(value);
    }

    public void ShowADVForContinue()
    {
        InteractorWithBrowser.AddSecondLife();
    }

    public void ShowADVForRecommendedSkin()
    {
        InteractorWithBrowser.PurchaseRecommendedSkinForADV();
    }

    public bool ADVIsShowed => advertiseCooldown == 3;
}
