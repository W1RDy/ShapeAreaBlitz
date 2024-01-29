using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;

public class BossBugsAttack : BaseBossAttack
{
    [SerializeField, FormerlySerializedAs("waitDuration")] float defaultWaitDuration;
    [SerializeField, ReadOnly] float waitDuration;
    [SerializeField] float hitDuration;
    [SerializeField] PointService bugsPointsService;
    [SerializeField] GameObject bug;
    [SerializeField] float newPlayerSpeed;
    float oldPlayerSpeed;
    Vector2 previousDirection;
    PlayerMove playerMove;
    PlayerController playerController;

    public override void Awake()
    {
        ChangeValueByDifficulty = () => waitDuration = (float)Math.Round(ChangerValueByDifficulty.instance.GetValueByDifficult(false, defaultWaitDuration));
        base.Awake();
    }

    public override void InitializeAttack(Transform boss)
    {
        base.InitializeAttack(boss);
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        playerController = GameObject.Find("Canvas").GetComponent<PlayerController>();
        oldPlayerSpeed = playerMove.GetSpeed();
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            if (isFinishing) break;
            var spawnPoints = new Transform[3];
            StartCoroutine(GetSpawnPos(spawnPoints));
            TimeCounter.instance.ActivateCounter((int)Math.Round(waitDuration, MidpointRounding.AwayFromZero));
            yield return new WaitForSeconds(waitDuration - 1);
            playerMove.SetSpeed(newPlayerSpeed);
            yield return new WaitForSeconds(1);
            SpawnBugs(spawnPoints);
            FreezeUnfreezePlayer(true);
            yield return new WaitForSeconds(hitDuration);
            FreezeUnfreezePlayer(false);
            playerMove.SetSpeed(oldPlayerSpeed);
        }
        isActivated = false;
    }

    private void FreezeUnfreezePlayer(bool isFreeze)
    {
        playerController.isCanMove = !isFreeze;
        if (isFreeze)
        {
            previousDirection = playerMove.direction;
            playerMove.SetDirection(Vector2.zero);
        }
        else playerMove.SetDirection(previousDirection);
    }

    private IEnumerator GetSpawnPos(Transform[] spawnPoints)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            var spawnPos = bugsPointsService.GetRandomPoint();
            Vector3 offset = spawnPos.TransformDirection(Vector3.right) * 10;
            TrajectoryShower.instance.ShowTrajectory(spawnPos.position + offset, spawnPos.position - offset, waitDuration);
            spawnPoints[i] = spawnPos;
            yield return null;
        }
        bugsPointsService.GetRandomPoint();
    }

    private void SpawnBugs(Transform[] spawnPositions)
    {
        AudioManager.instance.PlaySound("Bug");
        foreach (var spawnPosition in spawnPositions)
            Destroy(Instantiate(bug, spawnPosition.position, spawnPosition.localRotation), hitDuration);
    }
}
