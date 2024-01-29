using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Effect
{
    public EffectType type;
    public Action<bool> action;
    public Action<bool> view;
    public float duration;
    public bool isStack;
    [HideInInspector] public int activatedEffectsActions;
    [HideInInspector] public int activatedEffectsViews;

    public void SetEffect(Action<bool> effectAction, Action<bool> effectView)
    {
        action = effectAction;
        view = effectView;
    }
}
