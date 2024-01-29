using UnityEngine;

public interface ISpawnerable
{
    public void ActivateSpawner();

    public SpawnerType GetSpawnerType();

    public void DeactivateSpawner();

    public void ChangeSpawnerSettings(float spawnerCooldown, int? objectIndex);

    public void ChangeSpawnerSettings(float spawnerCooldown);

    public void ChangeSpawnerSettings(int? objectIndex);
    public void ChangeSpawnerSettings(DirectionType? spawnerDirectionIndex);
}
