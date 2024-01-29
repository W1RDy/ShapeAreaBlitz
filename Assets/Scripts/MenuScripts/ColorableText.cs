using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorableText : Text, IColorable
{
    public void SetColor(Color color)
    {
        this.color = color;
    }

    public Color GetColor() => color;
}
