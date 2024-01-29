using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IType
{
    [SerializeField] public int index;
    [SerializeField] public EnemyType type;
    [SerializeField] public bool isVulnerable = false;
    [SerializeField] int coinsForDestroying;
    [HideInInspector] public SpawnPositionConfig positionConfig { get; set; }
    [HideInInspector] public GameService gameService { get; private set; }
    [HideInInspector] public CustomSpriteRenderer enemyView { get; private set; }
    [HideInInspector] public ActivatorVulnerable activatorVulnerable;
    CoinsCollector coinsCollector;
    protected Transform enemyContainer;
    EventService eventService;
    Action DestroyEnemy;
    protected bool isInitialized = false;

    public virtual void Awake()
    {
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
        eventService = GameObject.Find("EventService").GetComponent<EventService>();
        coinsCollector = GameObject.Find("CoinsCollector").GetComponent<CoinsCollector>();
        enemyContainer = GameObject.Find("Pool/EnemyContainer").transform;
        DestroyEnemy = () =>
        {
            if (gameObject.activeInHierarchy) StartDestroying(0f);
        };
        activatorVulnerable = GetComponent<ActivatorVulnerable>();

        eventService.DestroyAllEnemies += DestroyEnemy;
        index = IndexDistributor.instance.GetIndex();
    }

    protected void InitializeEnemy()
    {
        enemyView = transform.GetChild(0).GetComponent<CustomSpriteRenderer>();

        enemyView.spriteRenderer.sortingOrder = index;
        try
        {
            enemyView.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = index+1;
        }
        catch { }

        isInitialized = true;
    }

    public virtual void InitializeEnemyVariant()
    {
        if (!isInitialized) InitializeEnemy();
   
        if (gameService.IsBossStage() && gameService.currentBoss == BossType.VirusPhantom)
            enemyView.SetMaterial(MaterialManager.instance.GetMaterial("Phantom"));
    }

    public void ActivateVulnerable()
    {
        isVulnerable = true;
        activatorVulnerable.MakeEnemyVulnerable();
    }
    
    public void DeactivateVulnerable()
    {
        isVulnerable = false;
        activatorVulnerable.MakeEnemyUnvulnerable();
    }

    public virtual void SetEnemyType ()
    {
        type = EnemyType.DefaultEnemy;
    }

    public void StartDestroying(float time)
    {
        if (gameObject.activeInHierarchy) StartCoroutine(DestroyingCoroutine(time));
    }

    protected virtual IEnumerator DestroyingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isVulnerable)
        {
            AudioManager.instance.PlaySound("KillEnemy");
            coinsCollector.CollectCoins(coinsForDestroying, transform.position);
            StartDestroying(0);
        }
    }

    private void OnDestroy()
    {
        eventService.DestroyAllEnemies -= DestroyEnemy;
        IndexDistributor.instance.DestroyIndex(index);
    }

    public bool IsCloseToPlayer(Transform player, float suitableDistance)
    {
        return Vector3.Distance(player.position, transform.position) <= suitableDistance;
    }

    Enum IType.GetType()
    {
        return type;
    }
}
