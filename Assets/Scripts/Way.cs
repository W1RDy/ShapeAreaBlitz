using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour
{
    [SerializeField] public int index;
    [SerializeField] public WayPartsConfig[] wayParts;
    [SerializeField] bool isEmpty = true;
    [SerializeField] Way[] neighborWays;
    private int enemyCount;
    public Way oppositeWay;

    Dictionary<int, WayPartsConfig> wayPartsDictionary;

    private void Awake()
    {
        wayPartsDictionary = new Dictionary<int, WayPartsConfig>();

        foreach (var wayPart in wayParts)
        {
            wayPartsDictionary[wayPart.index] = wayPart;
            wayPart.endPoint.SetIndex(wayPart.index);
        }
    }

    public int GetIndex() => index;

    public Transform GetSpawnPoint(int index) => wayPartsDictionary[index].spawnPoint;

    public void AddEnemy()
    {
        enemyCount++;
        if (isEmpty) ChangeWayState();
    }

    public void RemoveEnemy()
    {
        enemyCount--;
        if (enemyCount == 0) ChangeWayState();
    }

    public void ClearWay()
    {
        if (enemyCount != 0)
        {
            enemyCount = 0;
            ChangeWayState();
        }
    }

    private void ChangeWayState() => isEmpty = !isEmpty;

    public bool IsNeighborsWayEmpty()
    {
        foreach (var neighborWay in neighborWays) 
            if (!neighborWay.isEmpty) return false;
        return true;
    }

    public int GetFarthestWayPartIndex(Transform transform)
    {
        var firstWayPart = GetDistance(transform, wayParts[0]);
        var secondWayPart = GetDistance(transform, wayParts[1]);
        return firstWayPart < secondWayPart ? 1 : 0;
    }

    private float GetDistance(Transform transform, WayPartsConfig wayPart) => Vector3.Distance(transform.position, wayPart.spawnPoint.position);

}
