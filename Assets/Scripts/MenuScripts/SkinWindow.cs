using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinWindow : Window
{
    [SerializeField] SkinFrame skinFrame;
    [SerializeField] CustomText windowText;
    [SerializeField] Button[] buttons;
    Action<int> ChangeByBuySkin;

    protected override void Awake()
    {
        base.Awake();
        ChangeByBuySkin = cost => ChangeWindowByBuySkin();
        SkinManager.instance.SkinIsBought += ChangeByBuySkin;
    }

    public override void ActivateDeactivateWindow(bool isActivate)
    {
        base.ActivateDeactivateWindow(isActivate);
        if (isActivate) transform.SetSiblingIndex(transform.parent.childCount - 5);
        else UIPanel.SetAsFirstSibling();
    }

    public void SetRecommendedSkin(RecommendedSkin recommendedSkin)
    {
        skinFrame.SetSkinCost(recommendedSkin.skin.cost);
        skinFrame.SetSkinImage(recommendedSkin.skin.skinImage);
        var ADVButton = buttons[1].GetComponent<SkinButtonWithADV>();
        if (ADVButton != null) ADVButton.SetNewCost(recommendedSkin.recommendedCost);
    }

    public void ChangeWindowByBuySkin()
    {
        windowText.SetMessage("SkinIsBought");
        buttons[0].GetComponentInChildren<CustomText>().SetMessage("Ok");
        buttons[1].gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        SkinManager.instance.SkinIsBought -= ChangeByBuySkin;
    }
}
