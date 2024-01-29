using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reborner : MonoBehaviour
{
    [SerializeField] EffectActivator effectActivator;

    public void Reborn()
    {
        for (int i = 0; i < 2; i++) effectActivator.ActivateEffect(EffectType.Heart, true);
    }
}
