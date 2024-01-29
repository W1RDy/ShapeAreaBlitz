using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossSpikesAttack : BaseBossAttack
{
    [SerializeField, FormerlySerializedAs("cooldown")] float defaultCooldown;
    [SerializeField, ReadOnly] float cooldown;
    [SerializeField] float hitDuration;
    [SerializeField] RetractableObjActivator spikesActivator;

    public override void Awake()
    {
        ChangeValueByDifficulty = () => cooldown = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultCooldown);
        base.Awake();
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            if (isFinishing) break;
            spikesActivator.ActivateRetractableObj(hitDuration, cooldown);
            WarningSignActivator.instance.ActivateWarningSign(spikesActivator.GetTargetPos(), cooldown);
            yield return new WaitWhile(spikesActivator.IsActivated);
        }
        isActivated = false;
    }
}
