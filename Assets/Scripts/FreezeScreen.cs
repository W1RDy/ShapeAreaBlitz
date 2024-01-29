using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeScreen : MonoBehaviour
{
    public float duration;
    public float minSize, maxSize;
    Image freeze;
    Action<float> callback;

    public void SetParameters(float minSize, float maxSize, float duration)
    {
        this.minSize = minSize;
        this.maxSize = maxSize;
        this.duration = duration;
    }

    private void Start()
    {
        freeze = GetComponent<Image>();
        callback = size => freeze.material.SetFloat("Size", size);
        ActivateFreezeScreen();
    }

    public void ActivateFreezeScreen()
    {
        AudioManager.instance.PlaySound("Freeze");
        callback(minSize);
        StartCoroutine(FreezeScreenAppearing());
    }

    private IEnumerator FreezeScreenAppearing()
    {
        StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(maxSize), minSize, duration/2, callback));
        yield return new WaitForSeconds(duration/2);
        StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(minSize), maxSize, duration / 2, callback));
    }
}
