using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    public WindowType type;
    protected Transform UIPanel;

    protected virtual void Awake()
    {
        if (type == WindowType.SkinWindow || type == WindowType.SkinWindowWithADV)
        {
            UIPanel = GameObject.Find("Canvas/UIPanel").transform;
        }
    }

    public virtual void ActivateDeactivateWindow(bool isActivate)
    {
        gameObject.SetActive(isActivate);
        if (UIPanel && isActivate)
        {
            UIPanel.SetSiblingIndex(UIPanel.parent.childCount - 4);
        }
    }
}
