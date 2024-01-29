using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] Cloud clouds;
    [SerializeField] Pool cloudPool;
    [SerializeField] Transform defaultCloudsPos;
    [SerializeField] float[] yChangeInterval;
    [SerializeField] float cooldown;

    void Start() => StartCoroutine(SpawnerClouds());

    IEnumerator SpawnerClouds()
    {
        while (true)
        {
            yield return new WaitForSeconds(15F);
            var spawnPos = GetSpawnPos();
            var cloud = cloudPool.GetPool(clouds.GetType()).GetFreeElement();
            cloud.transform.position = spawnPos;
            StartCoroutine(CloudDestroying(70f, cloud.gameObject));
        }
    }

    IEnumerator CloudDestroying(float time, GameObject cloud)
    {
        yield return new WaitForSeconds(time);
        cloud.SetActive(false);
    }

    private Vector3 GetSpawnPos()
    {
        var yChange = Random.Range(yChangeInterval[0], yChangeInterval[1]);
        var defaultPos = defaultCloudsPos.position;
        return new Vector3(defaultPos.x, defaultPos.y + yChange, defaultPos.z);
    }
}
