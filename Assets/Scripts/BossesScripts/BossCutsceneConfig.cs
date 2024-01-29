using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class BossCutsceneConfig
{
    public bool isDefaultCutscene;
    [HideIf(nameof(isDefaultCutscene))] public BossType bossType;
    public PlayableDirector cutsceneDirector;
    public AnimationTrack[] tracks;
    public bool isAppearCutscene = true;

    public void InitializeTracks()
    {
        var animation = (TimelineAsset)cutsceneDirector.playableAsset;
        var offset = 0;
        foreach (var track in tracks)
        {
            if (animation.GetOutputTrack(track.index + offset).name == "Markers") offset++;
            track.trackAsset = animation.GetOutputTrack(track.index + offset);
        }
    }

    public void InitializeAnimation(Transform boss)
    {
        var bossView = boss.GetChild(0).GetComponent<Animator>();
        var bossTransform = boss.GetComponent<Animator>();
        foreach (var track in tracks)
        {
            if (track.isActivatorTrack) cutsceneDirector.SetGenericBinding(track.trackAsset, boss.gameObject);
            else if (track.isTransformTrack) cutsceneDirector.SetGenericBinding(track.trackAsset, bossTransform);
            else cutsceneDirector.SetGenericBinding(track.trackAsset, bossView);
        }
    }
}
