using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnersController : MonoBehaviour
{
    [SerializeField] GameObject[] spawnersObjects;
    ISpawnerable[] spawners;
    Dictionary<SpawnerType, List<ISpawnerable>> spawnersDictionary;

    private void Awake()
    {
        spawners = new ISpawnerable[spawnersObjects.Length];

        for (int i = 0; i < spawnersObjects.Length; i++)
        {
            var spawnerable = spawnersObjects[i].GetComponent<ISpawnerable>();
            if (spawnerable == null) throw new System.InvalidOperationException(spawnersObjects[i].name + "doesn't realize ISpawnerable interface");
            spawners[i] = spawnerable;
        }

        InitializeSpawnersDictionary();
    }

    private void InitializeSpawnersDictionary()
    {
        spawnersDictionary = new Dictionary<SpawnerType, List<ISpawnerable>>();

        foreach (var spawner in spawners)
        {
            var spawnerType = spawner.GetSpawnerType();
            if (!spawnersDictionary.ContainsKey(spawnerType)) spawnersDictionary[spawnerType] = new List<ISpawnerable>();
            spawnersDictionary[spawnerType].Add(spawner);
        }
    }

    private List<ISpawnerable> GetSpawnersList(SpawnerType spawnerType)
    {
        if (spawnerType == SpawnerType.AllSpawners) return spawners.ToList();
        return spawnersDictionary[spawnerType];
    }

    public void ActivateDeactivateSpawners(SpawnerType spawnerType, bool isActivate)
    {
        var spawners = GetSpawnersList(spawnerType);

        foreach (var spawner in spawners)
        {
            if (isActivate) spawner.ActivateSpawner();
            else spawner.DeactivateSpawner();
        }
    }

    public void ChangeSpawnerSettings(SpawnerType spawnerType, float spawnerCooldown, int? objectIndex)
    {
        ChangeSpawnerSettings(spawnerType, spawnerCooldown);
        ChangeSpawnerSettings(spawnerType, objectIndex);
    }

    public void ChangeSpawnerSettings(SpawnerType spawnerType, float spawnerCooldown)
    {
        var spawners = GetSpawnersList(spawnerType);

        foreach (var spawner in spawners) spawner.ChangeSpawnerSettings(spawnerCooldown);
    }

    public void ChangeSpawnerSettings(SpawnerType spawnerType, int? objectIndex)
    {
        var spawners = GetSpawnersList(spawnerType);

        foreach (var spawner in spawners) spawner.ChangeSpawnerSettings(objectIndex);
    }

    public void ChangeSpawnerSettings(SpawnerType spawnerType, DirectionType? direction)
    {
        var spawners = GetSpawnersList(spawnerType);

        foreach (var spawner in spawners) spawner.ChangeSpawnerSettings(direction);
    }
}
