using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsActivator : MonoBehaviour
{
    [SerializeField] WindowsService windowsService;

    public void ActivateDeactivateWindow(WindowType windowType, bool isActivate)
    {
        Window window;
        if (isActivate && (windowType == WindowType.SkinWindow || windowType == WindowType.SkinWindowWithADV)) window = windowsService.GetSkinWindow(windowType);
        else window = windowsService.GetWindow(windowType);
        if (window != null) window.ActivateDeactivateWindow(isActivate);
    }
}
