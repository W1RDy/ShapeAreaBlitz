using UnityEngine;

public interface IMovableBoss
{
    public void SetSpeed(float speed);
    public float GetStartSpeed();
    public Transform GetTarget();
    public bool BossIsMoving();
    public void ActivateDeactivateBossMovement(bool isActivate);
    public void ChangeBossCollideWithWalls(bool isCollideWithWalls);
}
