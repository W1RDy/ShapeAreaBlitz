using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HidingUIComponent : MonoBehaviour
{
    Image image;
    Text text;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponent<Text>();
    }

    private void Start()
    {
        transform.GetComponentInParent<HiderUI>().HideObject += alphaValue =>
        {
            if (image)
                image.color = new Color(image.color.r, image.color.g, image.color.b, alphaValue);
            if (text)
                text.color = new Color(text.color.r, text.color.g, text.color.b, alphaValue);
        };
    }
}
