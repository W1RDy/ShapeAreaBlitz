using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothableVector3 : ISmoothable<Vector3>
{
    Vector3 value;

    public SmoothableVector3(Vector3 value)
    {
        this.value = value;
    }

    public Vector3 ChangeValue(Vector3 firstValue, Vector3 secondValue, float step)
    {
        return value = Vector3.Lerp(firstValue, secondValue, step);
    }

    public Vector3 GetValue()
    {
        return value;
    }
}
