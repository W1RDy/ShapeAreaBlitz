using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTutorial : Boss
{
    public Transform laserStartPoint;

    protected override void InitializeBossVariant()
    {
        base.InitializeBossVariant();
        var tutorialObj = GameObject.Find("Tutorial");
        tutorialObj.GetComponent<TutorialObjectsActivator>().InitializeBoss(transform);
    }

    public override void Death()
    {
        AudioManager.instance.PlaySound("TurnOff");
        animator.SetTrigger("Kill");
    }
}
