using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongEnemy : MovableEnemy
{
    public override void SetEnemyType()
    {
        type = EnemyType.StrongEnemy;
    }
}
