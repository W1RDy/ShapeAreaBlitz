using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public abstract class Boss : MonoBehaviour, IHealthable, IType
{
    [Header("BossSettings")]
    [SerializeField] public BossType type;
    [SerializeField] int hp = 3;
    [SerializeField] public BossStateType stateType;
    [SerializeField] public BossAttack[] attacks;
    [SerializeField] public bool isPhantom;
    [SerializeField] bool isMovableBoss;
    public bool shield = true;
    [HideInInspector] public bool isInitialized = false;

    [Header("AdditiveScripts")]
    protected GameObject bossView;
    [HideInInspector] public BossBattleService battleService;
    [HideInInspector] public Animator animator;
    [HideInInspector] public BossCutscenesActivator cutscenesActivator;
    [HideInInspector] public GameService gameService;
    DeathZone deathZone;
    Action ChangeSpeedByDifficulty;

    [Header("Movable")]
    [SerializeField, ShowIf(nameof(isMovableBoss)), FoldoutGroup("Movable")] float defaultSpeed;
    [ShowIf(nameof(isMovableBoss)), ReadOnly, FoldoutGroup("Movable")] public float speed;
    [HideInInspector, FoldoutGroup("Movable")] public Transform target;
    [HideInInspector, FoldoutGroup("Movable")] public TargetMove movable;

    protected virtual void Awake()
    {
        if (!isInitialized) InitializeBoss();
    }

    private void Start()
    {
        InitializeBossVariant();
    }

    public virtual void InitializeBoss()
    {
        deathZone = GetComponent<DeathZone>();
        bossView = transform.Find("BossView").gameObject;
        animator = bossView.GetComponent<Animator>();
        battleService = GameObject.Find("BossServices").GetComponent<BossBattleService>();
        battleService.SetBoss(this);
        GameObject.Find("BossAttackActivator").GetComponent<BossAttackActivator>().SetBoss(this);
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
        cutscenesActivator = GameObject.Find("CutsceneActivator").GetComponent<BossCutscenesActivator>();
        GameObject.Find("CutsceneService").GetComponent<BossCutscenesService>().InitializeCutsceneTracks(this, isPhantom);
        isInitialized = true;
    }

    protected virtual void InitializeBossVariant()
    {
        if (isMovableBoss)
        {
            target = GameObject.Find("BossAttacks/BossTarget").transform;
            target.position = Vector2.zero;
            movable = GetComponent<TargetMove>();
            movable.target = target;
            movable.SetMovableState(false);

            SetValueChanger();
            ChangeSpeedByDifficulty();
        };

        InitializeAttacks();

        if (isPhantom)
        {
            hp = 1;
            var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.material = MaterialManager.instance.GetMaterial("Phantom").material;
        }
        else gameService.currentBoss = type;
    }

    public virtual void SetValueChanger()
    {
        ChangeSpeedByDifficulty = () =>
        {
            speed = ChangerValueByDifficulty.instance.GetValueByDifficult(true, defaultSpeed);
            movable.SetSpeed(speed);
        };
        gameService.SetLevelDifficulty += ChangeSpeedByDifficulty;
    }

    private void InitializeAttacks()
    {
        foreach (var attack in attacks)
        {
            attack.attackable = BossAttackService.instance.GetAttackable(attack.attackableIndex);
            attack.attackable.InitializeAttack(transform);
        }
    }

    public void SetVulnerable(bool isVulnerable)
    {
        ActivateDeactivateShield(!isVulnerable);
        ChangeWaitState(isVulnerable);
    }

    private void ActivateDeactivateShield(bool isActivate)
    {
        shield = isActivate;
        ActivateDeactivateDeathZoneShield(isActivate);
    }

    public void ActivateDeactivateDeathZoneShield(bool isActivate) => deathZone.shield = isActivate;

    public void ChangeWaitState(bool isWait)
    {
        if (isWait) stateType = BossStateType.Waiting;
        else stateType = BossStateType.Idle;
        animator.SetBool("Wait", isWait);
    }

    public void ReturnToIdle()
    {
        animator.SetBool("Wait", false);
        animator.SetInteger("Attack", 0);
        animator.Play("Boss_Idle");
    }

    public void ChangeAttackState(int attackIndex)
    {
        if (attackIndex == 0) stateType = BossStateType.FinishAttacking;
        else stateType = BossStateType.Attacking;
        animator.SetInteger("Attack", attackIndex);
    }

    public bool BossIsWaiting() => stateType == BossStateType.Waiting;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && shield == false) ChangeHp(-1);
    }

    public void ChangeHp(int changeValue)
    {
        if (changeValue < 0)
        {
            hp += changeValue;
            battleService.BossTakeHit();
            if (hp == 0) Death();
            else animator.SetTrigger("Hit");
        }
    }

    public int GetHp() => hp;

    public virtual void Death()
    {
        if (ChangeSpeedByDifficulty != null) gameService.SetLevelDifficulty -= ChangeSpeedByDifficulty;

        if (!isPhantom)
        {
            var cutsceneLength = cutscenesActivator.GetCutsceneDuration();
            gameService.FinishGame(true, cutsceneLength + 2);
        }
        else
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySound("KillEnemy");
        }
    }

    public BossAttack GetRandomAttack() => RandomizerWithChances<BossAttack>.RandomWithChances(attacks);

    Enum IType.GetType()
    {
        return type;
    }
}
