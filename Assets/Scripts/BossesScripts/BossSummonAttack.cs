using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BossSummonAttack : BaseBossAttack
{
    [SerializeField] List<SpawnerType> spawners;
    [SerializeField] EnemyType enemyType;
    [SerializeField, FormerlySerializedAs("spawnCooldown")] float defaultSpawnCooldown;
    [SerializeField, ReadOnly] float spawnCooldown;
    SpawnersController spawnersController;

    [SerializeField] bool isChangeEnemySpawnPlace;
    [SerializeField, FoldoutGroup("SpawnPlacesParameters"), ShowIf(nameof(isChangeEnemySpawnPlace))] string newSpawnPlace;
    [FoldoutGroup("SpawnPlacesParameters")] EnemyService enemyService;

    [SerializeField] bool isChangeSpawnerDirection;
    [SerializeField, ShowIf(nameof(isChangeSpawnerDirection))] DirectionType direction;
    string defaultSpawnPlace;

    public override void Awake()
    {
        spawnersController = GameObject.Find("SpawnersController").GetComponent<SpawnersController>();
        enemyService = GameObject.Find("EnemyService").GetComponent<EnemyService>();
        ChangeValueByDifficulty = () => spawnCooldown = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultSpawnCooldown);
        base.Awake();
    }

    public override void InitializeAttack(Transform boss)
    {
        if (isChangeEnemySpawnPlace)
            defaultSpawnPlace = enemyService.GetEnemy(enemyType).spawnerPlaceIndex[0];
    }

    public override void ActivateAttack()
    {
        isActivated = true;
        ChangeSpawnerSettings(true);
    }

    private void ChangeSpawnerSettings(bool isActivate)
    {
        int? enemyIndex = null;
        float spawnCooldown = 3f;
        DirectionType? direction = null;
        if (isActivate)
        {
            enemyIndex = (int)enemyType;
            spawnCooldown = this.spawnCooldown;
            if (isChangeSpawnerDirection) direction = this.direction;
        }

        if (isChangeEnemySpawnPlace)
        {
            enemyService.ChangeSpawnerPlaceIndexes(EnemyType.EnemyShooter, newSpawnPlace, isActivate);
            enemyService.ChangeSpawnerPlaceIndexes(EnemyType.EnemyShooter, defaultSpawnPlace, !isActivate);
        }

        foreach (var spawner in spawners)
        {
            spawnersController.ChangeSpawnerSettings(spawner, enemyIndex);
            spawnersController.ChangeSpawnerSettings(spawner, spawnCooldown);

            if (isChangeSpawnerDirection)
                spawnersController.ChangeSpawnerSettings(spawner, direction);

            spawnersController.ActivateDeactivateSpawners(spawner, true);
        }
    }

    public override void FinishAttack()
    {
        ChangeSpawnerSettings(false);
        isActivated = false;
    }
}
