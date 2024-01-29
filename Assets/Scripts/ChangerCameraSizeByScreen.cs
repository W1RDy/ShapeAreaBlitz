using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ChangerCameraSizeByScreen : MonoBehaviour
{
    [SerializeField] float defaultCameraSize = 7.3f;
    Camera _camera;
    Vector2 currentSize = new Vector2(1920, 1080);

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _camera.orthographicSize = defaultCameraSize;
    }

    private void Update()
    {
        if (currentSize != new Vector2(Screen.width, Screen.height))
        {
            var difference = Mathf.Min(Screen.width / currentSize.x, Screen.height / currentSize.y);
            _camera.orthographicSize /= difference;
            currentSize = new Vector2(Screen.width, Screen.height);
        }
    }
}
