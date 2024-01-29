using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectService : MonoBehaviour
{
    [SerializeField] Effect[] effects;
    [SerializeField] FreezeScreenConfig freezeScreen;
    [SerializeField] EffectIndicator[] effectIndicators;
    [SerializeField] CoinsCollector coinsCollector;
    [SerializeField] GameObject shieldView;
    [SerializeField] Transform canvas;
    Dictionary<EffectType, Effect> effectsDict;
    Dictionary<EffectType, EffectIndicator> indicatorDict;
    Player player;
    GameObject currentFreezeScreen;
    GameObject currentShieldView;

    private void Awake()
    {
        InitializeIndicatorsDictionary();
        InitializeEffectsDictionary();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();    
    }

    private void InitializeIndicatorsDictionary()
    {
        indicatorDict = new Dictionary<EffectType, EffectIndicator>();

        foreach (var indicator in effectIndicators) indicatorDict[indicator.effectType] = indicator;
    }

    private void InitializeEffectsDictionary()
    {
        effectsDict = new Dictionary<EffectType, Effect>();

        foreach (var effect in effects)
        {
            if (effect.type == EffectType.Freeze) freezeScreen.duration = effect.duration;
            else if (effect.type == EffectType.Shield || effect.type == EffectType.CoinDoubler) 
                indicatorDict[effect.type].duration = effect.duration;

            var actionAndView = GetEffectActionAndView(effect.type);
            effect.action = actionAndView.action;
            effect.view = actionAndView.view;
            effectsDict[effect.type] = effect;
        }
    }

    private (Action<bool> action, Action<bool> view) GetEffectActionAndView(EffectType effectType)
    {
        Action<bool> Action = (isActivate) => { };
        Action<bool> View = (isActivate) => { };
        switch (effectType)
        {
            case EffectType.Freeze:
                View = (isActivate) =>
                {
                    if (isActivate)
                    {
                        freezeScreen.SetScreenParameters();
                        if (currentFreezeScreen != null) Destroy(currentFreezeScreen);
                        currentFreezeScreen = Instantiate(freezeScreen.freezeScreen.gameObject, canvas);
                        currentFreezeScreen.transform.SetSiblingIndex(0);
                    }
                    else Destroy(currentFreezeScreen);
                };
                Action = (isActivate) =>
                {
                    if (isActivate) player.ChangeSpeed(10);
                    else player.ChangeSpeed(50);
                };
                break;
            case EffectType.Shield:
                View = (isActivate) =>
                {
                    if (isActivate)
                    {
                        if (currentShieldView != null) Destroy(currentShieldView);
                        currentShieldView = Instantiate(shieldView, player.transform);
                    }
                    else Destroy(currentShieldView);
                    indicatorDict[EffectType.Shield].ActivateDeactivateIndicator(isActivate);
                };
                Action = (isActivate) => { player.SetShield(isActivate); };
                break;
            case EffectType.CoinDoubler:
                View = (isActivate) => indicatorDict[EffectType.CoinDoubler].ActivateDeactivateIndicator(isActivate);
                Action = (isActivate) =>
                {
                    if (isActivate) coinsCollector.MultiplyCoins(2);
                    else coinsCollector.MultiplyCoins(0.5f);
                };
                break;
            case EffectType.Heart:
                Action = (isActivate) => { if (isActivate) player.ChangeHp(1); };
                break;
        }
        return (Action, View);
        throw new InvalidOperationException(effectType + " is not correct effect type!");
    }

    public Effect GetEffect(EffectType type) => effectsDict[type];
}
