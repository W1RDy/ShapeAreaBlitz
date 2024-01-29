using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParametersConfigs
{
    public ParameterConfig[] parameters;

    public ParametersConfigs(params (string index, float value)[] _params)
    {
        parameters = new ParameterConfig[_params.Length];
        
        for (int i = 0; i < parameters.Length; i++)
        {
            parameters[i] = new ParameterConfig(_params[i].index, _params[i].value);
        }
    }

    public float this[string index]
    {
        get
        {
            foreach (var parameter in parameters)
                if (parameter.index == index) return parameter.value;
            throw new InvalidOperationException("parameter with index " + index + " is incorrect parameter!");
        }
    }
}
