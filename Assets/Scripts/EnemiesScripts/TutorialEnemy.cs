using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : MovableEnemy
{
    public override void SetEnemyType() => type = EnemyType.TutorialEnemy;
}
