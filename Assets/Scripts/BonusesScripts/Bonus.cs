using System;
using System.Collections;
using UnityEngine;

public abstract class Bonus : MonoBehaviour, IType
{
    [HideInInspector] public EffectType type;
    EffectActivator effectActivator;
    EventService eventService;
    Action DestroyBonus;

    public virtual void Start()
    {
        effectActivator = GameObject.Find("EffectActivator").GetComponent<EffectActivator>();
        eventService = GameObject.Find("EventService").GetComponent <EventService>();
        DestroyBonus = () =>
        {
            if (gameObject.activeInHierarchy) StartDestroying(0);
        };

        eventService.DestroyAllBonuses += DestroyBonus;
    }

    private void OnEnable()
    {
        StartDestroying(4f);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            effectActivator.ActivateEffect(type, true);
            StartDestroying(0);
        }
    }

    private void StartDestroying(float time) => StartCoroutine(DestroyingCoroutine(time));

   private IEnumerator DestroyingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    Enum IType.GetType()
    {
        return type;
    }

    private void OnDestroy()
    {
        if (eventService != null) eventService.DestroyAllBonuses -= DestroyBonus;
    }
}
