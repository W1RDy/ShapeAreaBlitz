using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour, ISpawnerable
{
    [SerializeField] BonusService service;
    [SerializeField] Pool bonusPool;
    [SerializeField] SpawnPositionsService spawnPositionsService;
    [SerializeField] float spawnCooldown;
    [SerializeField] SpawnerType type;
    [SerializeField] bool isActivated;
    [SerializeField] EffectType? bonusType = null;
    Transform spawnParent;
    bool isFinishingSpawn = false;

    private void Awake()
    {
        spawnParent = GameObject.Find("Objects").transform;
    }

    public void ActivateSpawner()
    {
        if (!isActivated)
        {
            isActivated = true;
            isFinishingSpawn = false;
            StartCoroutine(Spawn());
        }
        else if (isFinishingSpawn) isFinishingSpawn = false;
    }

    public void DeactivateSpawner()
    {
        isFinishingSpawn = true;
    }

    public SpawnerType GetSpawnerType()
    {
        return type;
    }

    private IEnumerator Spawn()
    {
        Bonus bonus;
        while (true)
        {
            yield return new WaitForSeconds(spawnCooldown);
            if (isFinishingSpawn)
            {
                isActivated = false;
                isFinishingSpawn = false;
                break;
            }

            if (bonusType == null) bonus = service.GetRandomBonus();
            else bonus = service.GetBonus(bonusType.Value);
            if (bonus == null) continue;
            var spawnPos = spawnPositionsService.GetSpawnPosition("InsideRoom");

            var bonusExemplar = bonusPool.GetPool(bonus.type).GetFreeElement();
            bonusExemplar.transform.position = spawnPos.spawnPos;
            bonusExemplar.transform.SetParent(spawnParent);
            bonusExemplar.transform.localScale = bonus.transform.localScale;
            AudioManager.instance.PlaySound("Bonus");
        }
    }

    public void ChangeSpawnerSettings(float spawnerCooldown, int? objectIndex)
    {
        ChangeSpawnerSettings(spawnerCooldown);
        ChangeSpawnerSettings(objectIndex);
    }

    public void ChangeSpawnerSettings(float spawnerCooldown)
    {
        spawnCooldown = spawnerCooldown;
    }

    public void ChangeSpawnerSettings(int? objectIndex)
    {
        if (objectIndex == null) bonusType = null;
        else bonusType = (EffectType)objectIndex;
    }

    public void ChangeSpawnerSettings(DirectionType? spawnerDirectionIndex) { }
}
