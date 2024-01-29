using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class PointService : MonoBehaviour
{
    [SerializeField] List<PointConfig> pointConfigs;
    PointConfig[] pointConfigsCopy;

    private void Awake()
    {
        try
        {
            var pointsInRoom = transform.GetChild(0).GetComponent<CopyrighterInRoom>().GetCopy();
            for (int i = 0; i < pointConfigs.Count(); i++)
            {
                pointConfigs[i].point = pointsInRoom.transform.GetChild(i);
            }
        }
        catch { }

        UpdateChances();
        pointConfigsCopy = pointConfigs.ToArray();
    }

    private void UpdateChances()
    {
        var step = 100 / pointConfigs.Count;
        var sum = 0;

        foreach (var pointConfig in pointConfigs)
        {
            pointConfig.chance = step;
            pointConfig.chanceStep = step;
            sum += step;
        }
        if (sum != 100) pointConfigs[0].chance += 100 - sum;
    }

    public Transform GetRandomPoint()
    {
        return RandomizerWithChances<PointConfig>.RandomWithChances(pointConfigs.ToArray()).point;
    }

    public Transform GetClosestPoint(Transform player)
    {
        float minDistance = float.MaxValue;
        Transform closestPoint = null;
        foreach (var point in pointConfigs)
        {
            var distance = Vector3.Distance(player.position, point.point.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = point.point;
            }
        }
        return closestPoint;
    }

    public void DestroyPointConfig(Transform point)
    {
        if (pointConfigs.Count > 1) DestroyByPoint(point);
        else UpdatePointsConfigs();
        UpdateChances();
    }

    private void DestroyByPoint(Transform point)
    {
        foreach (var pointConfig in pointConfigs)
        {
            if (pointConfig.point == point)
            {
                pointConfigs.Remove(pointConfig);
                break;
            }
        }
    }

    public void UpdatePointsConfigs()
    {
        pointConfigs = pointConfigsCopy.ToList();
    }
}
