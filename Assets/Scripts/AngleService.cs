using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AngleService
{
    public static float GetAngleByDirection(Vector2 direction)
    {
        if (direction.x > 0) return 90;
        else if (direction.x < 0) return -90;
        else if (direction.y > 0) return 180;
        else if (direction.y < 0) return 0;
        throw new System.InvalidOperationException("is incorrect direction!");
    }

    public static Quaternion GetAngleByTarget(Transform transform, Transform target)
    {
        Vector3 relative = target.InverseTransformPoint(transform.position);
        return Quaternion.AngleAxis(Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg + 90 + target.rotation.eulerAngles.z, Vector3.forward);
    }
}
