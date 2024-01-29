using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionConfig
{
    public Way way;
    public int wayPartIndex;
    public Vector2 spawnPos;

    public SpawnPositionConfig(Way way, int wayPartIndex)
    {
        this.way = way;
        this.wayPartIndex = wayPartIndex;
        spawnPos = way.GetSpawnPoint(wayPartIndex).position;
    }

    public SpawnPositionConfig(Vector2 spawnPos)
    {
        this.spawnPos = spawnPos;
    }
}
