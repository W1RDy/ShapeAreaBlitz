using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinIndicator : MonoBehaviour
{
    [SerializeField] bool isChangeTextBlock = false;
    Text text;
    RectTransform rectTransform;
    bool isInitialized;

    private void InitializeIndicator()
    {
        isInitialized = true;
        text = GetComponentInChildren<Text>();
        rectTransform = text.GetComponent<RectTransform>();
    }

    public void ChangeCoinIndicator(int coins)
    {
        if (!isInitialized) InitializeIndicator();
        var stringCoins = coins.ToString();
        if (isChangeTextBlock)
        {
            var textLength = stringCoins.Length < 5 ? stringCoins.Length : 4;
            rectTransform.sizeDelta = new Vector2(37 * textLength, rectTransform.sizeDelta.y);
        }
        text.text = stringCoins;
    }
}
