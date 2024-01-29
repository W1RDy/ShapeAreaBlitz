using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeCamera : MonoBehaviour
{
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    public void ChangeSize(float size)
    {
        mainCamera.orthographicSize = size;
    }

    public Camera GetCamera() => mainCamera;
}
