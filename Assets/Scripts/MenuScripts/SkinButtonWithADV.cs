using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButtonWithADV : MonoBehaviour
{
    int changedSkinCost;
    CustomText buyButtonText;

    private void Awake()
    {
        buyButtonText = GetComponentInChildren<CustomText>();
    }

    public void SetNewCost(int cost)
    {
        changedSkinCost = cost;
        SetBuyText();
    }

    private void SetBuyText()
    {
        if (buyButtonText == null) buyButtonText = GetComponentInChildren<CustomText>();
        buyButtonText.SetMessage("BuyFor", changedSkinCost.ToString());
    }
}
