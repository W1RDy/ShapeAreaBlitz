using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossLevel4 : Boss
{
    public Transform laserStartPoint;

    public override void Death()
    {
        cutscenesActivator.ActivateCutscene(this, false);
        animator.SetTrigger("Kill");
        base.Death();
    }
}
