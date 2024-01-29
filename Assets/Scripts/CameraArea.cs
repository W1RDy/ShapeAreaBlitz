using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    Collider2D area;

    private void Awake()
    {
        area = GetComponent<Collider2D>();    
    }

    public bool ObjectInArea(Transform obj)
    {
        return area.ClosestPoint(obj.position) == new Vector2 (obj.position.x, obj.position.y);
    }

    public Vector2 GetAreaPointOnWay(Vector3 direction, Transform obj)
    {
        var hit = Physics2D.Raycast(obj.position, direction, 20f, 1 << 5);
        if (hit.collider != null && hit.collider == area)
            return hit.point;
        else throw new System.InvalidOperationException(obj.gameObject.name + "doesn't move to the room!");
    }

    public Vector2 GetMaxAreaPoint() => area.bounds.max;
}
