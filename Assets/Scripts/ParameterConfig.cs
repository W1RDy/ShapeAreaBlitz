using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParameterConfig
{
    public string index;
    public float value;

    public ParameterConfig(string index, float value)
    {
        this.index = index;
        this.value = value;
    }
}
