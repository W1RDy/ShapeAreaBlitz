using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Bonus
{
    public override void Start()
    {
        base.Start();
        type = EffectType.Shield;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.tag == "Player") AudioManager.instance.PlaySound("Shield");
    }
}
