using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy, IRotatable
{
    [SerializeField] GameObject enemyBullet;
    [SerializeField] Transform shootPoint;
    [SerializeField] float shootDelay = 0.2f;
    ComputerMouse computerMouse;
    bool isShooting;
    private Transform target;

    public override void Awake()
    {
        base.Awake();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void InitializeEnemyVariant()
    {
        base.InitializeEnemyVariant();
        Invoke(nameof(Shoot), 1f);

        try { computerMouse = GameObject.FindGameObjectWithTag("Mouse").GetComponent<ComputerMouse>(); }
        catch { }
    }

    private void Update()
    {
        if (!isShooting) RotateForTarget(target);
    }

    public override void SetEnemyType()
    {
        type = EnemyType.EnemyShooter;
    }

    public void RotateForTarget(Transform target)
    {
        transform.rotation = AngleService.GetAngleByTarget(transform, target);
    }

    private void Shoot()
    {
        if (gameObject.activeInHierarchy) StartCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        isShooting = true;
        yield return new WaitForSeconds(shootDelay);
        Destroy(Instantiate(enemyBullet, shootPoint.position, transform.rotation), 2.2f);
        if (computerMouse != null) computerMouse.ThrowOutEnemy(GetComponent<EnemyShooter>());
        AudioManager.instance.PlaySound("Shot");
    }

    protected override IEnumerator DestroyingCoroutine(float time)
    {
        if (computerMouse != null) computerMouse.RemoveEnemy(this);
        return base.DestroyingCoroutine(time);
    }

    private void OnDisable()
    {
        isShooting = false;
    }
}
