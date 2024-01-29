using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skin
{
    public int index;
    public int cost;
    public GameObject skinPrefab;
    public Sprite skinImage;
    public bool isPurchased;
    public bool isEnabled;
    public bool isInterestingToPurchaseWithADV = true;
    public bool isInterestingToPurchase = true;

    public void SetState(bool isPurchased, bool isEnabled)
    {
        this.isPurchased = isPurchased;
        this.isEnabled = isEnabled;
    }
}
