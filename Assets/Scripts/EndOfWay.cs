using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class EndOfWay : MonoBehaviour
{
    int wayIndex;
    Way way;

    private void Awake()
    {
        way = GetComponentInParent<Way>();
    }

    public void SetIndex(int index) => wayIndex = index;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            var enemy = collision.GetComponentInParent<MovableEnemy>();
            if (enemy && enemy.isWayEnemy && (way.GetIndex() == enemy.positionConfig.way.GetIndex() && wayIndex == enemy.positionConfig.wayPartIndex))
            {
                enemy.StartDestroying(1f);
                if (!enemy.isVulnerable) way.RemoveEnemy();
            }
        }

    }
}
