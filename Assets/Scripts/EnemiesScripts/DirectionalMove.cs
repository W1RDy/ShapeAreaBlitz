using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DirectionalMove : MonoBehaviour, IMovable
{
    [SerializeField] float speed;
    [SerializeField] bool isMove = true;
    [SerializeField] bool isCollideWithWalls;
    [SerializeField] bool isEnemy;
    [SerializeField] DirectionType directionType;
    [SerializeField] string collideSoundIndex;
    Vector2 direction;

    private void Awake()
    {
        direction = DirectionService.GetDirection(directionType);
    }

    private void Update()
    {
        if (isMove) Move();
    }

    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public Vector2 GetMovableDirection() => direction;

    public void SetMovableState(bool isMove)
    {
        this.isMove = isMove;
    }

    public void SetDirection(DirectionType directionType)
    {
        this.directionType = directionType;
        direction = DirectionService.GetDirection(directionType);
    }

    public void SetDirectionByEnemy(MovableEnemy enemy)
    {
        direction = DirectionService.GetDirection(enemy);
    }

    public bool IsMoving()
    {
        return isMove;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && isCollideWithWalls)
        {
            SetMovableState(false);
            if (collideSoundIndex != null) AudioManager.instance.PlaySound(collideSoundIndex);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetMovableState(false);
        if (collideSoundIndex != null) AudioManager.instance.PlaySound(collideSoundIndex);
    }

    public void ChangeCollideSetting(bool isActivate)
    {
        isCollideWithWalls = isActivate;
    }
}
