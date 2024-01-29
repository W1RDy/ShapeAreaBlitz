using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[Serializable]
public class AnimationTrack
{
    public int index;
    [HideInInspector] public TrackAsset trackAsset;
    public bool isTransformTrack;
    public bool isActivatorTrack;
}
