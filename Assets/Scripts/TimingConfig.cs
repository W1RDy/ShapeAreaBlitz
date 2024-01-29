using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimingConfig
{
    public int enemyIndex;
    public float[] timings;

    public TimingConfig(int enemyIndex, (int wayPart, float time)[] timings)
    {
        this.enemyIndex = enemyIndex;
        this.timings = new float[2];
        foreach (var timing in timings) this.timings[timing.wayPart] = timing.time;
    }

    public bool IsGoodTimings(TimingConfig timingConfig, float goodDifference)
    {
        for (int wayPart = 0; wayPart < 2; wayPart++)
        {
            if (Math.Abs(timingConfig.timings[wayPart] - timings[wayPart]) <= goodDifference)
                return false;
        }
        return true;
    }
}
