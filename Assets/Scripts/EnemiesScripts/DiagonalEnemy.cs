using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalEnemy : MovableEnemy
{
    public override void SetEnemyType()
    {
        type = EnemyType.DiagonalEnemy;
    }

    protected override void ChangeEnemyParent(bool isSpawn)
    {
        if (isSpawn) transform.SetParent(positionConfig.way.transform);
        else
        {
            transform.SetParent(enemyContainer);
            transform.localScale = startScale;
            transform.rotation = Quaternion.identity;
        }
    }
}
