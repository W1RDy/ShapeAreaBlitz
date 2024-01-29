using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossCutscenesActivator : MonoBehaviour
{
    [SerializeField] BossCutscenesService cutscenesService;
    PlayableDirector currentDirector;

    public void ActivateCutscene(Boss boss, bool isAppear)
    {
        ActivateCutscene(boss, isAppear, false);
    }

    public void ActivateCutscene(Boss boss, bool isAppear, bool isDefault)
    {
        var currentAnimation = isDefault ? cutscenesService.GetDefaultCutscene() : cutscenesService.GetCutscene(boss.type, isAppear);
        currentDirector = currentAnimation.cutsceneDirector;
        currentDirector.Play();
        Destroy(currentDirector, GetCutsceneDuration() + 0.1f);
    }

    public float GetCutsceneDuration()
    {
        if (currentDirector != null) return (float) currentDirector.duration;
        return 0f;
    }
}
