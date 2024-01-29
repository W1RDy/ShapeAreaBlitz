using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerInvisibleObstacles : MonoBehaviour
{
    public static ShowerInvisibleObstacles instance;

    [SerializeField] float borderOffsetX = 4, borderOffsetY = 2;
    CameraArea cameraArea;
    Transform player;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);

        instance.player = GameObject.FindGameObjectWithTag("Player").transform;
        instance.cameraArea = GameObject.Find("Canvas/CameraArea").GetComponent<CameraArea>();
    }

    public void ShowSign(Transform obstacle, int obstacleIndex)
    {
        if (!cameraArea.ObjectInArea(obstacle))
        {
            var movable = obstacle.GetComponent<TargetMove>();
            var index = obstacle.gameObject.name + obstacleIndex;
            if (movable != null && movable.target == player) StartCoroutine(ActivateAndControlSign(obstacle, true, index));
            else StartCoroutine(ActivateAndControlSign(obstacle, false, index));
        }
    }

    private IEnumerator ActivateAndControlSign(Transform obstacle, bool isDynamicChanging, string index)
    {
        WarningSignActivator.instance.ActivateWarningSign(GetSignPoint(obstacle), index);
        while (obstacle.gameObject.activeInHierarchy && !cameraArea.ObjectInArea(obstacle))
        {
            //if (isDynamicChanging) WarningSignActivator.instance.ChangeWarningSignPos(index, GetSignPoint(obstacle));
            yield return null;
        }
        WarningSignActivator.instance.DeactivateWarningSign(index);
    }

    private Vector2 GetSignPoint(Transform obstacle)
    {
        var movableObject = obstacle.GetComponent<IMovable>();
        if (movableObject != null)
        {
            var direction = movableObject.GetMovableDirection();
            direction = obstacle.TransformDirection(direction);
            var borderPoint = cameraArea.GetAreaPointOnWay(direction, obstacle);
            var maxPoint = cameraArea.GetMaxAreaPoint();
            var offsetScale = borderOffsetY / direction.y;

            if (maxPoint.y - Math.Abs(borderPoint.y) < 0.01f)
                offsetScale = Math.Abs(borderOffsetY / direction.y);
            else if (maxPoint.x - Math.Abs(borderPoint.x) < 0.01f)
                offsetScale = Math.Abs(borderOffsetX / direction.x);
            return borderPoint + direction * offsetScale;
        }
        else throw new NullReferenceException(obstacle.gameObject.name + " doesn't realize IMovable interface!");
    }
}
