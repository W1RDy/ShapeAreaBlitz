using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

[Serializable]
public class Event
{
    public EventType type;
    public bool isEventWithParameters;

    [ShowIf(nameof(isEventWithParameters))]
    public ParametersConfigs parametersConfigs;

    public float duration;
    public EventService.EventAction action;
}
