using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectActivator : MonoBehaviour
{
    [SerializeField] EffectService effectService;

    public void ActivateEffect(EffectType effectType, bool withView)
    {
        ActivateEffect(effectType, withView, -1);
    }

    public void ActivateEffect(EffectType effectType, bool withView, float duration)
    {
        var effect = effectService.GetEffect(effectType);
        if (duration < 0) duration = effect.duration;
        StartCoroutine(UseEffect(effect, withView, duration));
    }

    private IEnumerator UseEffect(Effect effect, bool withView, float duration)
    {
        effect.action(true);
        if(withView)effect.view(true);
        effect.activatedEffectsActions++;
        if (withView) effect.activatedEffectsViews++;
        yield return new WaitForSeconds(duration);
        effect.activatedEffectsActions--;
        if (withView) effect.activatedEffectsViews--;
        if (effect.activatedEffectsActions == 0 || effect.isStack) effect.action(false);
        if (effect.activatedEffectsViews == 0) effect.view(false);
    }
}
