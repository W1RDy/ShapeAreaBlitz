using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireLaserAttack : BaseBossAttack
{
    [SerializeField] float defaultSpeed = 18;
    [SerializeField, ReadOnly] float speed;
    [SerializeField] Transform target;
    [SerializeField] float distance;
    Transform laserStartPoint;
    AudioSource audioSource;
    TargetMove targetMove;
    LineRenderer laser;
    RaycastHit2D hit;
    Player player;
    Action<float> callback;

    public override void Awake()
    {
        ChangeValueByDifficulty = () =>
        {
            speed = ChangerValueByDifficulty.instance.GetValueByDifficult(true, defaultSpeed);
            if (targetMove) targetMove.SetSpeed(speed);
        };
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public override void InitializeAttack(Transform boss)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        callback = distance => this.distance = distance;

        targetMove = target.GetComponent<TargetMove>();
        targetMove.SetMovableState(false);
        targetMove.target = player.transform;

        laserStartPoint = boss.GetComponent<BossLevel2>().laserStartPoint;
        laser = GetComponent<LineRenderer>();
        laser.SetPosition(0, laserStartPoint.position);
        AudioManager.instance.AddLoopingSource(audioSource, name);
    }

    public override void ActivateAttack()
    {
        isActivated = true;
        isFinishing = false;
        targetMove.SetSpeed(speed / 2);
        target.position = laserStartPoint.position;
        laser.SetPosition(1, laserStartPoint.position);
        laser.enabled = true;
        StartCoroutine(WaitBeforeIncreaseSpeed());
        AudioManager.instance.PlayLoopingSound(name, "FireLaser");
    }

    private void Update()
    {
        if (isActivated && laser.enabled)
        {
            if (!isFinishing)
            {
                if (!targetMove.IsMoving()) targetMove.SetMovableState(true);
                distance = Vector3.Distance(laserStartPoint.position, target.position);
            }
            var direction = DirectionService.GetDirectionToTarget(laserStartPoint, target);
            laser.SetPosition(1, new Vector2(laserStartPoint.position.x, laserStartPoint.position.y) + direction * distance);
            hit = Physics2D.Raycast(laserStartPoint.position, direction, distance, 1 << 7);
            if (hit.collider != null) 
                if (hit.collider.tag == "Player") player.ChangeHp(-1);
        }
    }

    public override void FinishAttack()
    {
        isFinishing = true;
        StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(distance), 0, 2f, callback));
        StartCoroutine(WaitBeforeFinish());
    }

    private bool LaserDistanceIsMin() => distance == 0;

    private IEnumerator WaitBeforeFinish()
    {
        yield return new WaitUntil(LaserDistanceIsMin);
        laser.enabled = false;
        targetMove.SetMovableState(false);
        isActivated = false;
        AudioManager.instance.StopLoopingSound(name);
    }

    private IEnumerator WaitBeforeIncreaseSpeed()
    {
        yield return new WaitForSeconds(1f);
        targetMove.SetSpeed(speed);
    }
}
