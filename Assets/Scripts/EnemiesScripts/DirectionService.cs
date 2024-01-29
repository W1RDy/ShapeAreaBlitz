using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionService
{
    public static Vector2 GetDirection(Enemy enemy)
    {
        if (enemy.type == EnemyType.DiagonalEnemy)
            return GetDirectionToTarget(enemy.transform, Vector3.zero);
        else
        {
            if (enemy.transform.localPosition.x > 0) return GetDirection(DirectionType.Left);
            else return GetDirection(DirectionType.Right);
        }
    }

    public static DirectionType? GetDirectionType(Vector2 direction)
    {
        if (direction.x > 0.02) return DirectionType.Right;
        else if (direction.x < -0.02) return DirectionType.Left;
        else if (direction.y > 0.02) return DirectionType.Up;
        else if (direction.y < -0.02) return DirectionType.Down;
        else return null;
    }

    public static Vector2 GetDirection(DirectionType? directionType)
    {
        if (directionType == DirectionType.Right) return Vector2.right;
        else if (directionType == DirectionType.Down) return Vector2.down;
        else if (directionType == DirectionType.Left) return Vector2.left;
        else if (directionType == DirectionType.Up) return Vector2.up;
        throw new System.InvalidOperationException(directionType+ " is incorrect direction index!");
    }

    public static Vector2 GetOneCoordDirectionToTarget(Transform transform, Transform target)
    {
        var direction = GetDirectionToTarget(transform, target);
        (float value, string index) coord = (direction.x, "x");
        if (coord.value == 0 || direction.y != 0 && Random.value < 0.5f)
            coord = (direction.y, "y");
        return GetDirectionByCoord(coord);
    }

    public static Vector2 GetMaxCoordDirectionToTarget(Transform transform, Transform target)
    {
        var direction = GetDirectionToTarget(transform, target);
        if (System.Math.Abs(direction.x) > System.Math.Abs(direction.y)) return GetDirectionByCoord((direction.x, "x"));
        else return GetDirectionByCoord((direction.y, "y"));
    }

    public static DirectionType GetDirectionForDodge(DirectionType directionTypeToEnemy, Transform player)
    {
        var directionToEnemy = GetDirection(directionTypeToEnemy);
        var result = new Vector2(directionToEnemy.y, directionToEnemy.x);
        if ((player.localPosition.x > 0 && result.x > 0) || (player.localPosition.x < 0 && result.x < 0)) result = new Vector2(-result.x, result.y);
        if ((player.localPosition.y > 0 && result.y > 0) || (player.localPosition.y < 0 && result.y < 0)) result = new Vector2(result.x, -result.y);
        return GetDirectionType(result).Value;
    }

    public static Vector2 GetDirectionToTarget(Transform transform, Transform target)
    {
        return GetDirectionToTarget(transform.position, target.position);
    }

    public static Vector2 GetDirectionToTarget(Transform transform, Vector3 target)
    {
        return GetDirectionToTarget(transform.position, target);
    }

    public static Vector2 GetDirectionToTarget(Vector3 transform, Vector3 target)
    {
        return (target - transform).normalized;
    }

    private static Vector2 GetDirectionByCoord((float value, string index) coord)
    {
        if (coord.index == "x")
        {
            if (coord.value > 0) return Vector2.right;
            else return Vector2.left;
        }
        if (coord.index == "y")
        {
            if (coord.value > 0) return Vector2.up;
            else return Vector2.down;
        }
        return Vector2.zero;
    }

    public static DirectionType[] GetTutorialDirectionTypes(TutorialElementConfig tutorialElement, TutorialObjectsActivator tutorialObjectsActivator, Transform player)
    {
        if (tutorialElement.isPointerByDirection) return tutorialElement.directionTypes;
        else
        {
            var directionTypes = new DirectionType[tutorialElement.connectedObjectsIndexes.Length];
            for (int i = 0; i < directionTypes.Length; i++)
            {
                var connectedObject = tutorialObjectsActivator.GetConnectedObjectByIndex(tutorialElement.connectedObjectsIndexes[i]);
                directionTypes[i] = tutorialObjectsActivator.GetDirectionByObstacle(connectedObject);
                if (tutorialElement.isDodgeConnectedObject) directionTypes[i] = GetDirectionForDodge(directionTypes[i], player);
            }
            return directionTypes;
        }
    }
}
