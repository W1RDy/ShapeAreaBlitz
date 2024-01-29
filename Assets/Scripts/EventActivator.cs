using System.Collections;
using UnityEngine;

public class EventActivator : MonoBehaviour
{
    //[SerializeField] float activateCooldown;
    [SerializeField] EventService eventService;
    //[SerializeField] bool isActivateByChance = true;
    [SerializeField] GameService gameService;

    //public void ActivateEventsByChanceActivator()
    //{
    //    StartCoroutine(ActivateEventByChance());
    //}

    //private IEnumerator ActivateEventByChance()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(activateCooldown);
    //        if (!isActivateByChance) break;
    //        var _event = eventService.GetRandomEvent();
    //        if (_event != null) ActivateEvent(_event, _event.parametersConfigs);
    //    }
    //}

    //public void StopActivateEventsByChance()
    //{
    //    isActivateByChance = false;
    //}

    public void ActivateBoss()
    {
        //StopActivateEventsByChance();
        ActivateEvent(EventType.Boss, 0, new ParametersConfigs(("boss", (float)gameService.bossOnLevel)));
        DeactivateEvent(EventType.ActivateSpawner, (int)SpawnerType.AllSpawners);
        ActivateEvent(EventType.DestroyAllEnemies);
    }

    public void ActivateEvent(EventType eventType)
    {
        ActivateEvent(eventType, 0, null);
    }

    public void ActivateEvent(EventType eventType, int actionTypeIndex)
    {
        var _event = eventService.GetEvent(eventType);
        ActivateEvent(_event, actionTypeIndex, null);
    }

    public void ActivateEvent(EventType eventType, int actionTypeIndex, ParametersConfigs parameters)
    {
        var _event = eventService.GetEvent(eventType);
        ActivateEvent(_event, actionTypeIndex, parameters);
    }

    public void DeactivateEvent(EventType eventType, int actionTypeIndex)
    {
        var _event = eventService.GetEvent(eventType);
        ActivateDeactivateEventAction(_event, actionTypeIndex, false, null);
    }

    public void DeactivateEvent(EventType eventType, int actionTypeIndex, ParametersConfigs parameters)
    {
        var _event = eventService.GetEvent(eventType);
        ActivateDeactivateEventAction(_event, actionTypeIndex, false, parameters);
    }

    //private void ActivateEvent(Event _event, ParametersConfigs parameters)
    //{
    //    ActivateEvent(_event, 0, parameters);
    //}

    private void ActivateEvent(Event _event, int actionTypeIndex, ParametersConfigs parameters)
    {
        StartCoroutine(StartEvent(_event, actionTypeIndex, parameters));
    }

    private IEnumerator StartEvent(Event _event, int actionTypeIndex, ParametersConfigs parameters)
    { 
        ActivateDeactivateEventAction(_event, actionTypeIndex, true, parameters);
        yield  return new WaitForSeconds(_event.duration);
        if (_event.duration > 0) ActivateDeactivateEventAction(_event, actionTypeIndex, false, parameters);
    }

    private void ActivateDeactivateEventAction(Event _event, int actionTypeIndex, bool isActivate, ParametersConfigs parameters)
    {
        _event.action(isActivate, actionTypeIndex, parameters);
    }
}
