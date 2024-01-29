using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnPositionsService : MonoBehaviour
{
    [SerializeField] WayService wayService;
    [SerializeField] MeshCollider spawnPlace;
    [SerializeField] Vector2 marginSize;
    Vector2 borderPos;

    public SpawnPositionConfig GetSpawnPosition(string spawnerPlaceIndex)
    {
        return GetSpawnPosition(spawnerPlaceIndex, null);
    }

    public SpawnPositionConfig GetSpawnPosition(string spawnerPlaceIndex, int? wayPartIndex)
    {
        if (spawnerPlaceIndex == "Horizontal" || spawnerPlaceIndex == "Vertical" || spawnerPlaceIndex == "Diagonal")
        {
            var wayAndPartIndex = GetWayAndPartIndex(spawnerPlaceIndex);
            if (wayPartIndex == null) wayPartIndex = wayAndPartIndex.wayPartIndex;
            return new SpawnPositionConfig(wayAndPartIndex.way, wayPartIndex.Value);
        }
        else
        {
            var spawnPos = GetSpawnPos(spawnerPlaceIndex);
            return new SpawnPositionConfig(spawnPos);
        }
    }

    public SpawnPositionConfig GetSpawnPositionByTransform(string spawnerPlaceIndex, Transform transform)
    {
        var wayAndPartIndex = GetClosestWayAndFarthestPartIndex(transform, spawnerPlaceIndex);
        return new SpawnPositionConfig(wayAndPartIndex.way, wayAndPartIndex.wayPartIndex);
    }

    private (Way way, int wayPartIndex) GetWayAndPartIndex(string spawnerPlaceIndex)
    {
        return wayService.GetWayAndWayPartIndex(spawnerPlaceIndex);
    }

    private (Way way, int wayPartIndex) GetClosestWayAndFarthestPartIndex(Transform transform, string groupIndex)
    {
        var way = wayService.GetClosestWay(transform, groupIndex);
        var wayPartIndex = way.GetFarthestWayPartIndex(transform);
        return (way, wayPartIndex);
    }

    private Vector2 GetSpawnPos(string spawnerPlaceIndex)
    {
        if (spawnerPlaceIndex == "Center") return Vector2.zero;
        else if (spawnerPlaceIndex == "Anywhere") return new Vector2((Random.value < 0.5f ? -1 : 1) * Random.Range(13, 20), (Random.value < 0.5f ? -1 : 1) * Random.Range(8, 12));
        else if (spawnerPlaceIndex == "InsideRoom") return RandomizePosInsideRoom();
        else if (spawnerPlaceIndex == "OutsideRoom") return new Vector2((Random.value < 0.5f ? -1 : 1) * Random.Range(9.8f, 11.5f), Random.Range(-5.8f, 5.8f));
        throw new System.InvalidOperationException("Index " + spawnerPlaceIndex + " is not correct index!");
    }

    private Vector2 RandomizePosInsideRoom()
    {
        borderPos = spawnPlace.bounds.max;
        Vector2 spawnPos;
        if (Random.value < 0.5)
            spawnPos = new Vector2(Random.Range(-borderPos.x, borderPos.x), Random.value < 0.5 ? -borderPos.y : borderPos.y);
        else
            spawnPos = new Vector2(Random.value < 0.5 ? -borderPos.x : borderPos.x, Random.Range(-borderPos.y, borderPos.y));
        if (CheckSpawnPos(spawnPos)) return spawnPos;
        else return RandomizePosInsideRoom();
    }

    private bool CheckSpawnPos(Vector2 spawnPos)
    {
        var objects = Physics2D.OverlapBoxAll(spawnPos, marginSize, 0, 1 << 7 | 1 << 9);
        return objects.Length == 0;
    }
}
