using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossFreezeSpikesAttack : BaseBossAttack
{
    [SerializeField, FormerlySerializedAs("cooldown")] float defaultCooldown;
    [SerializeField, ReadOnly] float cooldown;
    [SerializeField] float spawnDelay;
    [SerializeField] FreezeSpikes freezeSpikes;
    WayService wayService;

    public override void Awake()
    {
        wayService = GameObject.Find("WayService").GetComponent<WayService>();
        ChangeValueByDifficulty = () => cooldown = ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultCooldown);
        base.Awake();
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            if (isFinishing) break;
            var spawnPairPoints = GetPairSpawnPoints();
            ShowTrajectory(spawnPairPoints.point1);
            ShowTrajectory(spawnPairPoints.point2);
            yield return new WaitForSeconds(spawnDelay);
            SpawnSpikes(spawnPairPoints.point1);
            SpawnSpikes(spawnPairPoints.point2);
        }
        isActivated = false;
    }

    private (Transform point1, Transform point2) GetPairSpawnPoints()
    {
        var wayConfig = GetWayConfig();
        var point = GetWayConfig(wayConfig.directionIndex, wayConfig.sideIndex).point;
        return (wayConfig.point, point);
    }

    private (Transform point, string directionIndex, int sideIndex) GetWayConfig(string previousDirectionIndex, int previousPartIndex)
    {
        var wayDirectionIndex = Random.value < 0.5f ? "Horizontal" : "Vertical";
        var wayConfig = wayService.GetWayAndWayPartIndex(wayDirectionIndex);
        if (wayDirectionIndex == previousDirectionIndex && previousPartIndex == wayConfig.wayPartIndex)
        {
            var partIndex = previousPartIndex == 0 ? 1 : 0;
            return (wayConfig.way.GetSpawnPoint(partIndex), wayDirectionIndex, partIndex);
        }
        return (wayConfig.way.GetSpawnPoint(wayConfig.wayPartIndex), wayDirectionIndex, wayConfig.wayPartIndex);
    }

    private (Transform point, string directionIndex, int sideIndex) GetWayConfig()
    {
        return GetWayConfig("", -2);
    }

    private void SpawnSpikes(Transform spawnPoint)
    {
        var spikes = Instantiate(freezeSpikes.gameObject, spawnPoint.position, Quaternion.identity);
        spikes.transform.eulerAngles = new Vector3(0, 0, spawnPoint.eulerAngles.z - 90);
        if (spawnPoint.localPosition.x > 0) spikes.transform.eulerAngles = new Vector3(0,0, spikes.transform.eulerAngles.z + 180);
        Destroy(spikes, 6f);
        AudioManager.instance.PlaySound("SnowSpikes");
    }

    private void ShowTrajectory(Transform startPoint)
    {
        var direction = startPoint.localPosition.x > 0 ? Vector2.left : Vector2.right;
        var offset = startPoint.TransformDirection(direction) * 40f;
        TrajectoryShower.instance.ShowTrajectory(startPoint.position, startPoint.position + offset, spawnDelay);
    }
}
