using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossDigitAttack : BaseBossAttack
{
    [SerializeField, FormerlySerializedAs("waitDuration")] float defaultWaitDuration;
    [SerializeField, ReadOnly] float waitDuration;
    [SerializeField] float hitDuration;
    [SerializeField] GameObject ones, nulls;
    [SerializeField] PointService cornerService;

    public override void Awake()
    {
        ChangeValueByDifficulty = () => waitDuration = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultWaitDuration);
        base.Awake();
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            if (isFinishing) break;
            var spawnPositions = new (string index, Vector2 position)[4];
            StartCoroutine(GetSpawnPos(spawnPositions));
            yield return new WaitForSeconds(waitDuration);
            SpawnDigits(spawnPositions);
            yield return new WaitForSeconds(hitDuration);
        }
        isActivated = false;
    }

    private IEnumerator GetSpawnPos((string index, Vector2 position)[] spawnPositions)
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            var spawnPoint = cornerService.GetRandomPoint();
            var index = i < spawnPositions.Length - 1 ? "one" : "null";
            if (index == "one") WarningSignActivator.instance.ActivateWarningSign(spawnPoint.position, waitDuration);
            spawnPositions[i] = (index, spawnPoint.position);
            yield return null;
        }
    }

    private void SpawnDigits((string index, Vector2 position)[] spawnPositions)
    {
        AudioManager.instance.PlaySound("Digit");
        foreach (var spawnPosition in spawnPositions)
        {
            var digit = spawnPosition.index == "one" ? ones : nulls;
            Destroy(Instantiate(digit, spawnPosition.position, Quaternion.identity), hitDuration);
        }
    }
}
