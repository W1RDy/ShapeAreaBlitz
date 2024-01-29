using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IMovable
{
    [SerializeField] bool isMove;
    Player player;
    public Vector2 direction;
    float speed;
    Rigidbody2D rb;
    Action<Vector3> callback;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
        player = GetComponent<Player>();
        callback = position => transform.position = position;
    }

    private void FixedUpdate()
    {
        if (isMove) Move();
    }

    public void Move()
    {
        rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            UnavailableDirectionsManager.instance.SetCollision(collision);
            SetDirection(Vector2.zero);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
            UnavailableDirectionsManager.instance.RemoveCollision(collision);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetSpeed() => speed;

    public Vector2 GetMovableDirection() => direction;

    public void SetDirection(Vector2 direction)
    {
        if (IsCanMove(direction))
        {
            this.direction = direction;
            SetMovableState(true);
            AudioManager.instance.PlaySound("Move");
            StartCoroutine(TestMoving());
        }
        else if (direction == Vector2.zero)
        {
            this.direction = Vector2.zero;
            rb.velocity = Vector2.zero;
            SetMovableState(false);
        }
    }

    public void ChangeDirectionByRotation(bool isStartRotation)
    {
        if (isStartRotation) direction = transform.InverseTransformDirection(direction);
        else direction = transform.TransformDirection(direction);
    }

    private bool IsCanMove(Vector2 direction)
    {
        DirectionType? directionType = DirectionService.GetDirectionType(direction);
        if (directionType == null || UnavailableDirectionsManager.instance.IsUnavailableDirection(directionType.Value)) return false;
        return true;
    }

    public void MoveToTarget(Transform target)
    {
        UnavailableDirectionsManager.instance.ClearAllUnavailableDirections();
        StartCoroutine(SmoothChanger<SmoothableVector3, Vector3>.SmoothChange(new SmoothableVector3(transform.position), target.position, 2f, callback));
    }

    public void SetMovableState(bool isMove)
    {
        this.isMove = isMove;
        player.ChangeAnimatorState(isMove);
    }

    public bool IsMoving()
    {
        return isMove;
    }

    public void ChangeCollideSetting(bool isActivate) { }

    private IEnumerator TestMoving()
    {
        var startPos = transform.position;
        yield return new WaitForSeconds(0.5f);
        if (startPos == transform.position) SetDirection(Vector2.zero);
    }

    public bool PlayerIsCame(Transform[] camePlaces)
    {
        foreach (var camePlace in camePlaces)
        {
            if (Vector3.Distance(transform.position, camePlace.position) < 2f) return true;
        }
        return false;
    }
}
