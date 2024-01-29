using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, ISpawnerable
{
    [SerializeField] EnemyService enemyService;
    [SerializeField] Pool enemiesPool;
    [SerializeField] SpawnPositionsService spawnPositionsService;
    [SerializeField] float spawnCooldown;
    [SerializeField] List<string> spawnerPlaceIndex;
    [SerializeField] public SpawnerType type;
    [SerializeField] bool isActivated;
    [SerializeField] EnemyType? enemyType = null;
    bool isFinishingSpawn = false;
    private int? spawnerDirectionIndex = null;
 
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

    IEnumerator Spawn()
    {
        EnemyConfig enemyConfig;
        while (true)
        {
            yield return new WaitForSeconds(spawnCooldown);
            if (isFinishingSpawn)
            {
                isActivated = false;
                isFinishingSpawn = false;
                break;
            }

            if (enemyType == null) enemyConfig = enemyService.GetRandomEnemyConfig(spawnerPlaceIndex);
            else enemyConfig = enemyService.GetEnemy(enemyType.Value);

            if (enemyConfig != null)
            {
                var spawnPosConfig = spawnPositionsService.GetSpawnPosition(GetSameIndex(enemyConfig), spawnerDirectionIndex);
                SpawnEnemy(spawnPosConfig, enemyConfig);
            }
        }
    }

    private Enemy SpawnEnemy(SpawnPositionConfig spawnPositionConfig, EnemyConfig enemyConfig)
    {
        var enemy = enemyConfig.enemy;
        var enemyObj = enemiesPool.GetPool(enemy.type).GetFreeElement();
        var currentEnemy = enemyObj.GetComponent<Enemy>();
        currentEnemy.transform.position = spawnPositionConfig.spawnPos;
        currentEnemy.positionConfig = spawnPositionConfig;
        currentEnemy.InitializeEnemyVariant();
        if (enemyConfig.isDestroyableForTime) currentEnemy.StartDestroying(enemyConfig.destroyingTime);
        return currentEnemy;
    }

    private string GetSameIndex(EnemyConfig enemyConfig)
    {
        foreach (var index in enemyConfig.spawnerPlaceIndex) 
            if (spawnerPlaceIndex.Contains(index)) return index;
        throw new System.InvalidOperationException("Chosen incorrect enemy!");
    }

    public SpawnerType GetSpawnerType()
    {
        return type;
    }

    public void DeactivateSpawner()
    {
        isFinishingSpawn = true;
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
        if (objectIndex == null) enemyType = null;
        else enemyType = (EnemyType)objectIndex;
    }

    public void ChangeSpawnerSettings(DirectionType? spawnerDirectionIndex)
    {
        if (spawnerDirectionIndex == null) this.spawnerDirectionIndex = null;
        else
            this.spawnerDirectionIndex = spawnerDirectionIndex == DirectionType.Right || spawnerDirectionIndex == DirectionType.Up ? 0 : 1;
    }

    public MovableEnemy SpawnEnemyOnSameWay(Transform transform, EnemyType enemyType, string spawnPlaceIndex)
    {
        if (spawnerPlaceIndex.Contains(spawnPlaceIndex))
        {
            var enemy = enemyService.GetEnemy(enemyType);
            var positionConfig = spawnPositionsService.GetSpawnPositionByTransform(spawnPlaceIndex, transform);
            return SpawnEnemy(positionConfig, enemy) as MovableEnemy;
        }
        return null;
    }
}
