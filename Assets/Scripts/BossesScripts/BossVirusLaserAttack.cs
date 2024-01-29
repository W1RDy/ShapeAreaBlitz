using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class BossVirusLaserAttack : BaseBossAttack, IRotatable
{
    [SerializeField, FormerlySerializedAs("laserSpeed")] float defaultLaserSpeed;
    [SerializeField, ReadOnly] float laserSpeed;
    [SerializeField] Transform target;
    [SerializeField] BossSummonAttack summonAttack;
    [SerializeField] float interruptTime;
    [SerializeField] float interruptDuration;
    AudioSource audioSource;
    EventActivator eventActivator;
    Transform boss;
    Transform startLaserPoint;
    Player player;
    TargetMove targetMove;
    LineRenderer laser;
    RaycastHit2D hit;
    Action<float> callback;

    public override void Awake()
    {
        eventActivator = GameObject.Find("EventsActivator").GetComponent<EventActivator>();
        ChangeValueByDifficulty = () =>
        {
            laserSpeed = ChangerValueByDifficulty.instance.GetValueByDifficult(true, defaultLaserSpeed);
            if (targetMove) targetMove.SetSpeed(laserSpeed);
        };
        base.Awake();
    }

    public override void InitializeAttack(Transform boss)
    {
        this.boss = boss;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        laser = GetComponent<LineRenderer>();
        startLaserPoint = boss.GetComponent<BossLevel4>().laserStartPoint;

        targetMove = target.GetComponent<TargetMove>();
        targetMove.target = player.transform;
        targetMove.SetMovableState(false);
        ChangeValueByDifficulty();

        callback = angle => boss.transform.eulerAngles = new Vector3(0, 0, angle);

        audioSource = GetComponent<AudioSource>();
        AudioManager.instance.AddLoopingSource(audioSource, name);
    }

    public void RotateForTarget(Transform target)
    {
        boss.rotation = AngleService.GetAngleByTarget(transform, target);
    }

    public override void ActivateAttack()
    {
        isActivated = true;
        isFinishing = false;
        summonAttack.ActivateAttack();
        StartCoroutine(Attack());
    }
    
    private void Update()
    {
        if (isActivated) 
        {
            if (!targetMove.IsMoving()) targetMove.SetMovableState(true);
            RotateForTarget(target);
            if (laser.enabled)
            {
                laser.SetPosition(0, startLaserPoint.position);
                laser.SetPosition(1, target.position);

                var distance = Vector3.Distance(Vector3.zero, target.position);
                var direction = DirectionService.GetDirectionToTarget(Vector3.zero, target.position);
                hit = Physics2D.Raycast(Vector3.zero, direction, distance, 1 << 7 | 1 << 3);

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Player") player.ChangeHp(-1);
                    else if (hit.collider.tag == "Enemy")
                    {
                        var enemy = hit.collider.GetComponent<Enemy>();
                        enemy.StartDestroying(0);
                    }
                }
            }
        }
    }

    private void StopLaser()
    {
        laser.enabled = false;
        AudioManager.instance.StopLoopingSound(name);
    }

    private void StartLaser()
    {
        target.position = startLaserPoint.position;
        laser.SetPosition(0, startLaserPoint.position);
        laser.SetPosition(1, target.position);
        laser.enabled = true;
        AudioManager.instance.PlayLoopingSound(name, "Laser");
    }

    public override void FinishAttack()
    {
        isFinishing = true;
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(interruptDuration);
            StartLaser();
            yield return new WaitForSeconds(interruptTime);
            StopLaser();
            if (isFinishing) break;
        }
        eventActivator.ActivateEvent(EventType.DestroyAllEnemies);
        summonAttack.FinishAttack();
        StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(boss.eulerAngles.z), 0, 1f, callback));
        StartCoroutine(WaitWhileRotate());
    }

    IEnumerator WaitWhileRotate()
    {
        yield return new WaitUntil(IsRotationToZero);
        isActivated = false;
    }

    private bool IsRotationToZero() => boss.eulerAngles.z == 0;
}
