using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyService : MonoBehaviour
{
    [SerializeField] EnemyConfig[] configs;
    [SerializeField] EnemyConfig[] notRandomizableEnemies;
    Dictionary<EnemyType, EnemyConfig> enemiesDictionary;

    private void Awake()
    {
        InitializeEnemiesDictionary();
    }

    private void InitializeEnemiesDictionary()
    {
        enemiesDictionary = new Dictionary<EnemyType, EnemyConfig>();

        foreach (var enemyConfig in configs) enemiesDictionary[enemyConfig.type] = enemyConfig;

        foreach (var enemy in notRandomizableEnemies) enemiesDictionary[enemy.type] = enemy;
    }

    public EnemyConfig GetRandomEnemyConfig(List<string> groupIndex)
    {
        return RandomizerWithChances<EnemyConfig>.RandomWithChances(configs, groupIndex);
    }

    public EnemyConfig GetEnemy(EnemyType enemyType) => enemiesDictionary[enemyType];

    public void ChangeSpawnerPlaceIndexes(EnemyType enemyType, string index, bool isAdd)
    {
        var enemy = GetEnemy(enemyType);
        if (isAdd) enemy.spawnerPlaceIndex.Add(index);
        else enemy.spawnerPlaceIndex.Remove(index);
    }

    public void ChangeDestroyingEnemy(EnemyType enemyType, bool isDestroyByTime, float destroyingTime)
    {
        var enemy = GetEnemy(enemyType);
        enemy.isDestroyableForTime = isDestroyByTime;
        enemy.destroyingTime = destroyingTime;
    }
}
