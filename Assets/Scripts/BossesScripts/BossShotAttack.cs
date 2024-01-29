using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossShotAttack : BaseBossAttack
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float shootCooldown;
    [SerializeField] float disappearingDelay;
    [SerializeField] Transform target;
    Transform laserStartPoint;
    Vector3 laserStartPosition;
    float shootDuration;

    Player player;
    Vector3 direction;
    float distance;
    Vector3 endPosition;

    LineRenderer lineRenderer;
    bool isShoot;

    Action<float> callbackDistance;
    Action<Vector3> callbackDisappearing;

    public override void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        callbackDistance = distance => this.distance = distance;
        callbackDisappearing = position => laserStartPosition = position;
    }

    private void Update()
    {
        if (isShoot)
        {
            lineRenderer.SetPosition(0, laserStartPosition);
            lineRenderer.SetPosition(1, laserStartPoint.position + direction * distance);
            var hit = Physics2D.Raycast(laserStartPoint.position, direction, distance, 1 << 7);
            if (hit.collider != null)
                if (hit.collider.tag == "Player") player.ChangeHp(-1);
        }
    }

    public override void InitializeAttack(Transform boss)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        laserStartPoint = boss.GetComponent<BossTutorial>().laserStartPoint;
    }

    public override void ActivateAttack()
    {
        base.ActivateAttack();
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootCooldown);
            Shoot(true);
            yield return new WaitForSeconds(disappearingDelay);
            StartCoroutine(SmoothChanger<SmoothableVector3, Vector3>.SmoothChange(new SmoothableVector3(laserStartPoint.position), endPosition, shootDuration, callbackDisappearing));
            yield return new WaitForSeconds(shootDuration);
            Shoot(false);
            if (isFinishing) break;
        }
        isActivated = false;
    }

    private void Shoot(bool isActivate)
    {
        isShoot = isActivate;
        if (isActivate)
        {
            AudioManager.instance.PlaySound("TutorialLaser");
            laserStartPosition = laserStartPoint.position;
            lineRenderer.SetPosition(0, laserStartPosition);
            lineRenderer.SetPosition(1, laserStartPosition);

            endPosition = player.transform.position;
            direction = DirectionService.GetDirectionToTarget(laserStartPoint, endPosition);
            var finalDistance = Vector2.Distance(laserStartPoint.position, endPosition) * 1.1f;
            shootDuration = finalDistance / bulletSpeed;
            StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(0), finalDistance, shootDuration, callbackDistance));
        }
        lineRenderer.enabled = isActivate;
    }
}
