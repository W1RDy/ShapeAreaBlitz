using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinFrame : MonoBehaviour
{
    [SerializeField] Text skinCost;
    [SerializeField] CustomText skinState;
    [SerializeField] Image skinImage;

    public void SetSkinCost(int cost)
    {
        skinCost.text = cost.ToString();
    }

    public void SetSkinState(string state)
    {
        skinState.SetMessage(state);
    }

    public void SetSkinImage(Sprite sprite)
    {
        var spriteSize = sprite.bounds.size * sprite.pixelsPerUnit;
        var maxSideDifference = Mathf.Min(150 / spriteSize.x, 115 / spriteSize.y);
        skinImage.sprite = sprite;
        skinImage.rectTransform.sizeDelta = new Vector3(spriteSize.x * maxSideDifference, spriteSize.y * maxSideDifference);
    }
}
