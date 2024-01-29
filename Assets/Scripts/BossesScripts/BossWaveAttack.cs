using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWaveAttack : BaseBossAttack
{
    [SerializeField] float defaultCooldown = 1;
    [SerializeField, ReadOnly] float cooldown;
    [SerializeField] BossWave wave;
    Transform target;
    BossLevel1 boss;
    Transform player;
    Action<float> callback;

    public override void Awake()
    {
        ChangeValueByDifficulty = () => cooldown = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultCooldown);
        base.Awake();
    }

    public override void InitializeAttack(Transform boss)
    {
        var bossScript = boss.GetComponent<BossLevel1>();
        this.boss = bossScript;
        target = bossScript.GetTarget();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        callback = angle => boss.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            SetTarget();
            if (target.localPosition != Vector3.zero)
                yield return new WaitForSeconds(cooldown);
            boss.ActivateDeactivateBossMovement(true);
            yield return new WaitWhile(boss.BossIsMoving);
            if (target.localPosition == Vector3.zero && isFinishing) break;
            else if (target.localPosition != Vector3.zero) SpawnWave();
        }
        RotateForDirection(Vector2.down);
        yield return new WaitForSeconds(cooldown);
        isActivated = false;
    }

    private void SetTarget()
    {
        if (boss.transform.localPosition == Vector3.zero)
        {
            var direction = DirectionService.GetOneCoordDirectionToTarget(boss.transform, player);
            RotateForDirection(direction);
            target.localPosition = direction * 20f;
        }
        else target.localPosition = Vector3.zero;
    }

    private void SpawnWave()
    {
        var spawnPoint = boss.GetColliderPoint();
        Destroy(Instantiate(wave.gameObject, spawnPoint, boss.transform.rotation), 1f);
    }

    public void RotateForDirection(Vector2 direction)
    {
        var randAngle = AngleService.GetAngleByDirection(direction);
        if (randAngle == 180) randAngle = 0;
        StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(boss.transform.eulerAngles.z), randAngle, cooldown, callback));
    }
}
