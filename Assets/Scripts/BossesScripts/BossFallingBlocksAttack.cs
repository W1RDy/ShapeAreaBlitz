using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BossFallingBlocksAttack : BaseBossAttack
{
    [SerializeField, FormerlySerializedAs("spawnCooldown")] float defaultSpawnCooldown;
    [SerializeField, ReadOnly] float spawnCooldown;
    [SerializeField] FallingBlock fallingBlock;
    [SerializeField] PointService pointService;
    AudioSource audioSource;
    IMovableBoss movableBoss;
    Transform target;
    ParticleSystem dust;

    public override void Awake()
    {
        ChangeValueByDifficulty = () => spawnCooldown = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultSpawnCooldown);
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public override void InitializeAttack(Transform boss)
    {
        movableBoss = boss.GetComponent<IMovableBoss>();
        target = movableBoss.GetTarget();
        dust = boss.GetComponentInChildren<ParticleSystem>();
        AudioManager.instance.AddLoopingSource(audioSource, name);
    }

    public override void ActivateAttack()
    {
        isActivated = true;
        isFinishing = false;
        target.localPosition = Vector2.down * 20f;
        movableBoss.ChangeBossCollideWithWalls(true);
        movableBoss.ActivateDeactivateBossMovement(true);
        StartCoroutine(Attack());
    }

    public override IEnumerator Attack()
    {
        yield return new WaitWhile(movableBoss.BossIsMoving);
        AudioManager.instance.PlayLoopingSound(name, "Jackhammer");
        dust.Play();
        while (true)
        {
            yield return new WaitForSeconds(spawnCooldown);
            if (isFinishing) break;
            SpawnBlock();
        }
        yield return new WaitWhile(movableBoss.BossIsMoving);
        isActivated = false;
    }

    private void SpawnBlock()
    {
        var spawnPos = pointService.GetRandomPoint().position;
        Destroy(Instantiate(fallingBlock.gameObject, spawnPos, Quaternion.identity), 6f);
    }

    public override void FinishAttack()
    {
        target.localPosition = Vector3.zero;
        movableBoss.ActivateDeactivateBossMovement(true);
        dust.Stop();
        isFinishing = true;
        AudioManager.instance.StopLoopingSound(name);
    }
}
