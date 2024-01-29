using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRetractable 
{
    public void ActivateDeactivateObject();
    public void ChangeTargetPosition();
    public bool IsOnTargetPosition();
    public Vector2 GetTargetPos();
}
