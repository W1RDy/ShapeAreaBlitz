using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MovableEnemy
{
    [SerializeField] public bool isDestroyableForTime = true;

    public override void InitializeEnemyVariant()
    {
        base.InitializeEnemyVariant();
        if (isDestroyableForTime) StartDestroying(4f);
    }

    public override void SetEnemyType()
    {
        type = EnemyType.Virus;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") StartDestroying(0f);  
    }
}
