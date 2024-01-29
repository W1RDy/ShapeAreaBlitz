using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEnemy : MovableEnemy
{
    public override void SetEnemyType()
    {
        type = EnemyType.SpeedEnemy;
    }
}
