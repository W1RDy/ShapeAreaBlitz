using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combustion : MonoBehaviour
{
    [SerializeField] public float appearingDuration;
    Material material;
    Action<float> callback;
    float startValue, endValue;
    bool isPhantom;
    SpriteRenderer spriteRenderer;

    [ColorUsage(true, true)]
    [SerializeField] Color color;

    public void InitializeCombustionMaterial(bool isPhantom)
    {
        this.isPhantom = isPhantom;
        if (!isPhantom)
        {
            material = GetComponent<SpriteRenderer>().material;
            material.SetColor("_Color", color);
        }
        else spriteRenderer = GetComponent<SpriteRenderer>();

        callback = value =>
        {
            if (!isPhantom) material.SetFloat("_Fade", value);
            else spriteRenderer.color = new Color(1, 1, 1, value);
        };
        callback(0);
    }

    public void AppearDisappearByCombustion(bool isAppear)
    {
        if (isAppear) (startValue, endValue) = (0, 1);
        else (startValue, endValue) = (1, 0);
        StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(startValue), endValue, appearingDuration, callback));
    }

    public bool ObjIsDisappeared()
    {
        if (!isPhantom) return CombustionIsFinish() && material.GetFloat("_Fade") == 0;
        return spriteRenderer.color.a == 0;
    }

    public bool ObjIsAppeared()
    {
        if (!isPhantom) return CombustionIsFinish() && material.GetFloat("_Fade") == 1;
        return spriteRenderer.color.a == 1;
    }

    private bool CombustionIsFinish() => material.GetFloat("_Fade") == endValue;
}
