using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public abstract class MovableEnemy : Enemy
{ 
    public float speed { get; private set; }
    [SerializeField] float[] speedInterval;
    [SerializeField] public bool isWayEnemy = true;
    TimingsCalculator timingsCalculator;
    IMovable movable;
    DirectionalMove dirMovable;
    [HideInInspector] public Vector3 startScale;
    Transform room;

    public override void Awake()
    {
        base.Awake();
        timingsCalculator = GameObject.Find("TimingsCalculator").GetComponent<TimingsCalculator>();
        room = GameObject.Find("Objects/Room").transform;
        try
        {
            dirMovable = GetComponent<DirectionalMove>();
            movable = dirMovable;
            if (movable == null) throw new System.Exception();
        }
        catch { movable = GetComponent<IMovable>(); }
        startScale = transform.localScale;
    }

    public override void InitializeEnemyVariant()
    {
        base.InitializeEnemyVariant();
        if (isWayEnemy)
        {
            ChangeEnemyParent(true);
            Flip();

            if (enemyView.spriteRenderer.materials.Length < 2)
                enemyView.spriteRenderer.materials = new Material[2] { enemyView.spriteRenderer.material, MaterialManager.instance.GetMaterial("SpriteOutline").material };
            if (enemyView.spriteRenderer.materials.Length == 2)
            {
                if (enemyView.GetMaterial(MaterialType.OutlineMaterial).GetInt("_OutlineEnabled") == 1)
                    enemyView.SetMaterialInt(MaterialType.OutlineMaterial, "_OutlineEnabled", 0);
            }
        }
        ChangeMovingSettings();
        if (isWayEnemy)
        {
            if (!positionConfig.way.IsNeighborsWayEmpty() || !timingsCalculator.IsGoodTiming(this)) ActivateVulnerable();
            else
            {
                positionConfig.way.AddEnemy();
                timingsCalculator.AddTiming(this);
            }
        }
    }

    protected virtual void ChangeEnemyParent(bool isSpawn)
    {
        if (isSpawn)
        {
            transform.localRotation = positionConfig.way.transform.rotation;
            transform.SetParent(positionConfig.way.transform);
        }
        else
        {
            transform.SetParent(enemyContainer);
            transform.localScale = startScale;
        }
    }

    protected virtual void ChangeMovingSettings()
    {
        RandomizeSpeed();
        ChangeDirection();
        movable.SetSpeed(speed);
        movable.SetMovableState(true);
        Invoke(nameof(ShowInvisibleEnemies), 0.1f);
    }

    private void ChangeDirection()
    {
        if (isWayEnemy && dirMovable) dirMovable.SetDirectionByEnemy(this);
    }

    protected void Flip()
    {
        if (transform.localPosition.x > 0 && transform.localScale.x > 0 || transform.localPosition.x < 0 && transform.localScale.x < 0)
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    private void RandomizeSpeed()
    {
        speed = Random.Range(speedInterval[0] * gameService.generalGameSpeed, speedInterval[1] * gameService.generalGameSpeed);
    }

    public void StartStopEnemyMoving(bool isStart)
    {
        movable.SetMovableState(isStart);
    }

    private void ShowInvisibleEnemies() => ShowerInvisibleObstacles.instance.ShowSign(transform, index);

    protected override IEnumerator DestroyingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (gameObject.activeInHierarchy)
        {
            ChangeEnemyParent(false);
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnDisable()
    {
        if (isWayEnemy)
            timingsCalculator.RemoveTiming(this);
        if (isVulnerable) DeactivateVulnerable();
    }
}
