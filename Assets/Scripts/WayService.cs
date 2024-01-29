using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayService : MonoBehaviour
{
    [SerializeField] WayGroupConfig[] wayGroups;
    Dictionary<string, WayConfig[]> waysDict;

    private void Awake()
    {
        InitializeWaysDictionary();
    }

    private void InitializeWaysDictionary()
    {
        waysDict = new Dictionary<string, WayConfig[]>();

        foreach (var wayGroup in wayGroups)
        {
            waysDict[wayGroup.groupIndex] = wayGroup.wayConfigs;
            foreach (var wayConfig in wayGroup.wayConfigs) wayConfig.way.index = wayConfig.index;
        }
    }

    private Way GetRandomWay(string index)
    {
        return RandomizerWithChances<WayConfig>.RandomWithChances(waysDict[index]).way;
    }

    private int GetWayPartIndex(Way way) => Random.Range(0, way.wayParts.Length);

    public (Way way, int wayPartIndex) GetWayAndWayPartIndex(string index)
    {
        var way = GetRandomWay(index);
        var wayPartIndex = GetWayPartIndex(way);
        return (way, wayPartIndex);
    }

    public void ClearAllWays()
    {
        foreach (var wayGroup in wayGroups)
        {
            foreach (var wayConfig in wayGroup.wayConfigs)
                wayConfig.way.ClearWay();
        }
    }

    public WayConfig[] GetWayGroup(string index) => waysDict[index];

    public Way GetClosestWay(Transform transform, string groupIndex)
    {
        float minDistance = float.MaxValue;
        Way closestWay = null;
        foreach (var wayConfig in waysDict[groupIndex])
        {
            var distance = Vector3.Distance(wayConfig.way.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestWay = wayConfig.way;
            }
        }

        return closestWay;
    }
}
