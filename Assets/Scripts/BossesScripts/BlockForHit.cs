using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockForHit : MonoBehaviour, IRetractable
{
    [SerializeField] Transform target;
    [SerializeField] float speed = 2f;
    [SerializeField] bool isMovableBlock = true;
    bool isInitialized = false;
    TargetMove movable;
    Vector3 startTargetPos;

    private void InitializeBlockForHit()
    {
        if (isMovableBlock && !isInitialized)
        {
            isInitialized = true;
            movable = GetComponent<TargetMove>();
            movable.target = target;
            movable.SetSpeed(speed);
        }
    }

    public void ActivateDeactivateObject()
    {
        InitializeBlockForHit();
        if (isMovableBlock) ActivateBlockMovement();
        else transform.position = target.position;
    }

    private void ActivateBlockMovement()
    {
        if (startTargetPos == null) startTargetPos = target.localPosition;
        if (transform.position == target.position) ChangeTargetPosition();
        movable.SetMovableState(true);
    }

    public void ChangeTargetPosition()
    {
        if (target.localPosition == startTargetPos) target.localPosition = Vector3.zero;
        else target.localPosition = startTargetPos;
    }

    public bool IsOnTargetPosition()
    {
        if (gameObject == null) return true;
        return transform.position == target.position;
    }

    public Vector2 GetTargetPos()
    {
        return target.position;
    }
}
