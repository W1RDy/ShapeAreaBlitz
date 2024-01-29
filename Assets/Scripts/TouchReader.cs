using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchReader : MonoBehaviour, IPointerDownHandler
{
    Image touchZone;
    bool isPCPlayer;

    private void Awake()
    {
        touchZone = GetComponent<Image>();
        isPCPlayer = SystemInfo.deviceType == DeviceType.Desktop;
    }

    private void Update()
    {
        if (isPCPlayer && touchZone.enabled && Input.anyKeyDown) DialogManager.instance.ShowNextMessage(); 
    }
        
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPCPlayer) DialogManager.instance.ShowNextMessage();
    }

    public void DisableTouchZone()
    {
        if (touchZone.enabled)
        {
            touchZone.enabled = false;
        }
    }

    public void EnableTouchZone()
    {
        if (!touchZone.enabled)
        {
            touchZone.enabled = true;
        }
    }
}
