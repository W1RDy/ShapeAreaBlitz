using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorVulnerable : MonoBehaviour
{
    Renderer cracks;
    DeathZone deathZone;

    public void MakeEnemyVulnerable()
    {
        var view = transform.GetChild(0);
        try {
            cracks = view.GetChild(0).gameObject.GetComponent<Renderer>();
            cracks.enabled = true;
        }
        catch { }
        deathZone = GetComponent<DeathZone>();

        OutlineManager.instance.OutlineObject(new GameObject[1] { view.gameObject }, true);
        deathZone.shield = false;
    }

    public void MakeEnemyUnvulnerable()
    {
        if (cracks) cracks.enabled = false;
        deathZone.shield = true;
    }
}
