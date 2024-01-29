using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingsCalculator : MonoBehaviour
{
    [SerializeField] Timer timer;
    [SerializeField] Way[] ways;
    [SerializeField] float timingDifference = 2;
    GameService gameService;
    Dictionary<Way, Dictionary<int, TimingConfig>> unavailableTimings;

    private void Awake()
    {
        InitializeDictionary();
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
    }

    private void InitializeDictionary()
    {
        unavailableTimings = new Dictionary<Way, Dictionary<int,TimingConfig>>();

        foreach (var way in ways) unavailableTimings.Add(way, new Dictionary<int, TimingConfig>());
    }

    public void AddTiming(MovableEnemy enemy)
    {
        if (gameService.isPlay)
        {
            var way = enemy.positionConfig.way;
            if (unavailableTimings.ContainsKey(way))
            {
                var timings = GetTimings(enemy);
                unavailableTimings[way].Add(enemy.index, new TimingConfig(enemy.index, timings));
            }
        }
    }

    public void RemoveTiming(MovableEnemy enemy)
    {
        var way = enemy.positionConfig != null ? enemy.positionConfig.way : null;
        if (way != null && unavailableTimings.ContainsKey(way)) unavailableTimings[way].Remove(enemy.index);
    }

    public bool IsGoodTiming(MovableEnemy enemy)
    {
        var oppositeWay = enemy.positionConfig.way.oppositeWay;
        if (oppositeWay == null) return true;
        var timings = GetTimings(enemy);
        timingDifference = timingDifference / gameService.generalGameSpeed;
        if (timingDifference < 0.5f) timingDifference = 0.5f;
        foreach (var unavailableTiming in unavailableTimings[oppositeWay].Values)
        {
            if (!unavailableTiming.IsGoodTimings(new TimingConfig(enemy.index, timings), timingDifference)) 
                return false;
        }
        return true;
    }

    private (int wayPart, float time)[] GetTimings(MovableEnemy enemy)
    {
        (int wayPart, float time)[] timings = new (int wayPart, float time)[2];
        for (int i = 0; i < 2; i++)
        {
            var endPoint = enemy.positionConfig.way.wayParts[i].endPoint.transform;
            var offset = endPoint.localPosition.x > 0 ? -1.5f : 1.5f;
            var playerZone = new Vector2(endPoint.localPosition.x + offset, endPoint.localPosition.y);
            timings[i] = (i, CalculateTime(enemy, playerZone));
        }
        return timings;
    }

    private float CalculateTime(MovableEnemy enemy, Vector2 endPoint)
    {
        var distance = Vector3.Distance(enemy.transform.position, endPoint);
        return (distance / enemy.speed) + timer.GetTime();
    }
}
