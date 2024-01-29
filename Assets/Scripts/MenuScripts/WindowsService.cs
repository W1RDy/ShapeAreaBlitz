using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsService : MonoBehaviour
{
    [SerializeField] WindowConfig[] windowConfigs;
    [SerializeField] Rewarder rewarder;
    [SerializeField] CoinCounter coinsCounter;
    Dictionary<WindowType, Window> windowsDict;

    private void Awake()
    {
        InitializeWindowsDictionary();
    }

    private void InitializeWindowsDictionary()
    {
        windowsDict = new Dictionary<WindowType, Window>();

        foreach (var windowConfig in windowConfigs)
        {
            windowConfig.type = windowConfig.window.type;
            windowsDict[windowConfig.type] = windowConfig.window;
        }
    }

    public Window GetWindow(WindowType type) 
    {
        return windowsDict[type];
    }

    public SkinWindow GetSkinWindow(WindowType type)
    {
        SkinWindow window = null;
        var isSkinWithADV = type == WindowType.SkinWindowWithADV;
        var recommendedSkin = SkinManager.instance.GetRecommendedSkin(isSkinWithADV, true);
        if (recommendedSkin != null)
        {
            if (recommendedSkin.isRecommendedWithADV) window = windowsDict[WindowType.SkinWindowWithADV] as SkinWindow;
            else window = windowsDict[WindowType.SkinWindow] as SkinWindow;
            window.SetRecommendedSkin(recommendedSkin);
        }
        return window;
    }
}
