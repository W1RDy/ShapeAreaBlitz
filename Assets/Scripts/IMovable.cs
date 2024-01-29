using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    public void Move();
    public void SetSpeed(float speed);
    public Vector2 GetMovableDirection();
    public void SetMovableState(bool isMove);

    public bool IsMoving();

    public void ChangeCollideSetting(bool isActivate);
}
