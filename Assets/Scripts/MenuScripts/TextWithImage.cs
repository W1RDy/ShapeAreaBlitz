using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWithImage : CustomText
{
    [SerializeField] Image image;
    [SerializeField] float imageOffset;
    [SerializeField] Canvas canvas;

    public override void SetMessage(string message)
    {
        base.SetMessage(message);
    }

    public override void SetMessage(string index, string dynamicString)
    {
        if (dynamicString.Length == 3) dynamicString += "  ";
        base.SetMessage(index, dynamicString);
    }
}
