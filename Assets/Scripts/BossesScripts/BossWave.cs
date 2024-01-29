using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWave : MonoBehaviour
{
    DeathZone deathZone;

    void Start()
    {
        if (transform.position.y > 1) transform.eulerAngles = new Vector3(0, 0, 180);
        deathZone = GetComponent<DeathZone>();
        Invoke(nameof(DeactivateDeathZone), 0.1f);
    }

    private void DeactivateDeathZone()
    {
        deathZone.shield = false;
    }
}
