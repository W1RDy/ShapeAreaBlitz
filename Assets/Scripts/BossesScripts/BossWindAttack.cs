using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWindAttack : BaseBossAttack
{
    [SerializeField] float windForce;
    [SerializeField] BossSummonAttack summonAttack;
    AudioSource audioSource;
    PlayerController controller;
    GameService gameService;
    IMovableBoss movableBoss;
    Transform boss;
    Rigidbody2D player;
    ParticleSystem wind;
    Transform target;

    public override void Awake() 
    {
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
        controller = GameObject.Find("Canvas").GetComponent<PlayerController>(); 
        audioSource = GetComponent<AudioSource>();
    }

    public override void InitializeAttack(Transform boss)
    {
        this.boss = boss;
        movableBoss = boss.GetComponent<IMovableBoss>();
        wind = boss.GetComponentInChildren<ParticleSystem>();
        target = boss.GetComponent<BossLevel6>().target;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        AudioManager.instance.AddLoopingSource(audioSource, name);
    }

    public override void ActivateAttack()
    {
        isActivated = true;
        wind.Play();
        MoveBoss();
        StartCoroutine(WaitAttack());
        controller.isCanMove = false;
        AudioManager.instance.PlayLoopingSound(name, "Blizzard");
    }

    private void ActivateWindPhysic()
    {
        controller.isCanMoveHorizontal = false;
        gameService.StopChangeGameSpeed();
        gameService.generalGameSpeed += windForce;
        player.AddForce(Vector2.right * 50, ForceMode2D.Impulse);
    }

    private void MoveBoss()
    {
        if (target.localPosition == Vector3.zero) target.position = Vector3.left * 18;
        else target.localPosition = Vector3.zero;
        movableBoss.ActivateDeactivateBossMovement(true);
    }

    public override void FinishAttack()
    {
        wind.Stop();
        summonAttack.FinishAttack();
        gameService.generalGameSpeed -= windForce;
        gameService.StartChangeGameSpeed();
        controller.isCanMoveHorizontal = true;
        MoveBoss();
        StartCoroutine(WaitWhileReturn());
    }

    private IEnumerator WaitAttack()
    {
        yield return new WaitUntil(WindCollideWithPlayer);
        ActivateWindPhysic();
        yield return new WaitUntil(() => UnavailableDirectionsManager.instance.IsUnavailableDirection(DirectionType.Right));
        controller.isCanMove = true;
        yield return new WaitWhile(movableBoss.BossIsMoving);
        summonAttack.ActivateAttack();
    }

    private IEnumerator WaitWhileReturn()
    {
        yield return new WaitForSeconds(1.35f);
        AudioManager.instance.StopLoopingSound(name);
        yield return new WaitWhile(movableBoss.BossIsMoving);
        isActivated = false;
    }

    private bool WindCollideWithPlayer() => boss.transform.position.x < player.transform.position.x;
}
