using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour, IMovable
{
    [SerializeField] bool isEnemy;
    [SerializeField] bool isMove = true;
    [SerializeField] bool isCollideWithWalls;
    [SerializeField] public Transform target;
    [SerializeField] string collideSoundIndex;
    [SerializeField] bool isMoveToLastTargetPos;
    [SerializeField] bool moveWithoutStops;
    Vector3 targetLastPos;
    float speed;

    private void Start()
    {
        if (isEnemy)
        {
            var enemy = GetComponent<Enemy>();
            if (enemy.type == EnemyType.Virus) target = GameObject.FindGameObjectWithTag("Player").transform;
            SetLastTargetPos();
        }
    }

    private void Update()
    {
        if (isMove && target) Move();
    }

    public void Move()
    {
        if (!isMoveToLastTargetPos) targetLastPos = target.position;
        transform.position = Vector3.MoveTowards(transform.position, targetLastPos, speed * Time.deltaTime);
        if (transform.position == targetLastPos && !moveWithoutStops) SetMovableState(false);
        else if (transform.position == targetLastPos && moveWithoutStops) SetLastTargetPos();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public Vector2 GetMovableDirection()
    {
        return DirectionService.GetDirectionToTarget(transform, target);
    }

    public void SetMovableState(bool isMove)
    {
        this.isMove = isMove;
        if (isMove && target) SetLastTargetPos();
    }

    private void SetLastTargetPos()
    {
        if (isMoveToLastTargetPos && isMove) targetLastPos = target.position;
    }

    public bool IsMoving()
    {
        return isMove;
    }

    public void ChangeCollideSetting(bool isActivate)
    {
        isCollideWithWalls = isActivate;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6 && isCollideWithWalls)
        {
            SetMovableState(false);
            if (collideSoundIndex != null && collideSoundIndex != "") AudioManager.instance.PlaySound(collideSoundIndex);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && isCollideWithWalls)
        {
            SetMovableState(false);
            if (collideSoundIndex != null) AudioManager.instance.PlaySound(collideSoundIndex);
        }
    }
}
