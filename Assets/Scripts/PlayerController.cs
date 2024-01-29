using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    PlayerMove plMv;
    public bool isCanMove = true, isCanMoveHorizontal = true;
    bool isPCPlayer;
    DirectionType? lastHistoryDirection = null;

    void Start()
    {
        plMv = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        isPCPlayer = SystemInfo.deviceType == DeviceType.Desktop;
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        if (isCanMove)
        {
            DirectionType? directionType = null;
            if (Mathf.Abs(eventData.delta.x) > 10 || Mathf.Abs(eventData.delta.y) > 10)
            {
                if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) && isCanMoveHorizontal)
                {
                    if (eventData.delta.x > 10f && CanSetDirectionType(DirectionType.Right)) directionType = DirectionType.Right;
                    else if (eventData.delta.x < -10f && CanSetDirectionType(DirectionType.Left)) directionType = DirectionType.Left;
                }
                else
                {
                    if (eventData.delta.y > 10f && CanSetDirectionType(DirectionType.Up)) directionType = DirectionType.Up;
                    else if (eventData.delta.y < -10f && CanSetDirectionType(DirectionType.Down)) directionType = DirectionType.Down;
                }
            }
            SetDirection(directionType);
        }
    }

    private void Update()
    {
        if (isPCPlayer && isCanMove)
        {
            DirectionType? directionType = null;
            if (isCanMoveHorizontal)
            {
                if (Input.GetKeyDown(KeyCode.D) && CanSetDirectionType(DirectionType.Right)) directionType = DirectionType.Right;
                else if (Input.GetKeyDown(KeyCode.A) && CanSetDirectionType(DirectionType.Left)) directionType = DirectionType.Left;
            }
            if (Input.GetKeyDown(KeyCode.W) && CanSetDirectionType(DirectionType.Up)) directionType = DirectionType.Up;
            else if (Input.GetKeyDown(KeyCode.S) && CanSetDirectionType(DirectionType.Down)) directionType = DirectionType.Down;

            SetDirection(directionType);
        }
    }

    private void SetDirection(DirectionType? directionType)
    {
        if (directionType != null)
        {
            var direction = DirectionService.GetDirection(directionType);
            plMv.SetDirection(direction);
        }
    }

    private void SetMoveHistory(DirectionType _newDirectionType)
    {
        if (lastHistoryDirection == null || lastHistoryDirection.Value != _newDirectionType)
        {
            lastHistoryDirection = _newDirectionType;
            StartCoroutine(WaitWhileMoving(_newDirectionType));
            StartCoroutine(WaitWhileClearHistory());
        }
    }

    private IEnumerator WaitWhileMoving(DirectionType _newDirectionType)
    {
        yield return new WaitWhile(plMv.IsMoving);
        if (lastHistoryDirection == null || lastHistoryDirection.Value != _newDirectionType) yield break;
        SetDirection(_newDirectionType);
    }

    private IEnumerator WaitWhileClearHistory()
    {
        yield return new WaitForSeconds(0.4f);
        lastHistoryDirection = null;
    }

    private bool CanSetDirectionType(DirectionType directionType)
    {
        var canMoveImmediately = !plMv.IsMoving() || plMv.direction == -1 * DirectionService.GetDirection(directionType);
        if (!canMoveImmediately) SetMoveHistory(directionType);
        return canMoveImmediately;
    }
}
