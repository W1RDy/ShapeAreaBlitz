using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealthable
{
    [SerializeField] float speed = 50f;
    [SerializeField] int hp = 3;
    [SerializeField] bool isShield; 
    GameService gameService;
    HealthIndicator healthIndicator;
    EffectActivator effectActivator;
    Animator animator;
    IMovable movable;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movable = GetComponent<IMovable>();
        movable.SetSpeed(speed);
        healthIndicator = GameObject.Find("Canvas/UIPanel/HealthIndicator").GetComponent<HealthIndicator>();
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
        effectActivator = GameObject.Find("EffectActivator").GetComponent<EffectActivator>();
    }

    public void ChangeSpeed(float speed)
    {
        this.speed = speed;
        movable.SetSpeed(speed);
    }

    public void ChangeAnimatorState(bool isMove)
    {
        if (isMove) animator.SetBool("Run", true);
        else animator.SetBool("Run", false);
    }

    public void ChangeHp(int changeValue)
    {
        if ((!isShield && changeValue < 0) || changeValue > 0)
        {
            hp += changeValue;
            if (hp < 0) hp = 0;
            else if (hp > 3) hp = 3;
            healthIndicator.ChangeHealthIndicator(hp);

            if (changeValue < 0)
            {
                ActivateShieldForTime(1f);
                animator.SetTrigger("Damage");
                AudioManager.instance.PlaySound("TakeHit");
            }
            if (hp == 0) Death();
        }
    }

    public int GetHp()
    {
        return hp;
    }

    public void Death()
    {
        gameService.FinishGame(false, 0);
    }

    public void SetShield(bool value)
    {
         isShield = value;
    }

    public void ActivateShieldForTime(float time)
    {
        effectActivator.ActivateEffect(EffectType.Shield, false, time);
    }
}
