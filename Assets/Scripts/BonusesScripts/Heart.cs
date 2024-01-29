using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Bonus
{
    public override void Start()
    {
        base.Start();
        type = EffectType.Heart;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.tag == "Player") AudioManager.instance.PlaySound("Heal");
    }
}
