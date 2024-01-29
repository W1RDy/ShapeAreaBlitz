using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ButtonService : MonoBehaviour
{
    [SerializeField] MenuObjectsChanger changer;
    [SerializeField] ChooserLevel chooserLevel;
    [SerializeField] WindowsActivator windowsActivator;
    GameService gameService;
    PlayerController playerController;
    GameObject buttons;

    private void Start()
    {   
        playerController = GetComponentInParent<PlayerController>();
        buttons = GameObject.Find("Canvas/Buttons");

        try { gameService = GameObject.Find("GameService").GetComponent<GameService>(); }
        catch { }
    }

    public void ClickOnSkin(int skinIndex)
    {
        ActivateSound();
        var skin = SkinManager.instance.GetSkin(skinIndex);
        if (skin.isPurchased) SkinManager.instance.EquipSkin(skin);
        else SkinManager.instance.BuySkin(skin);
    }

    public void Play()
    {
        ActivateSound();
        var index = chooserLevel.levelIndex;
        if (index > 0) LoadSceneManager.instance.LoadScene(index);
    }

    public void Menu()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            gameService.FinishGameWithoutWindow(false);
        }

        ActivateSound();
        LoadSceneManager.instance.LoadScene(1);
    }

    public void Restart()
    {
        ActivateSound();
        LoadSceneManager.instance.ReloadCurrentScene();
    }

    public void NextLevel()
    {
        ActivateSound();
        LoadSceneManager.instance.LoadNextScene();
    }

    public void ActivateDeactivateLevelsChoosingWindow(bool isActivate)
    {
        ActivateSound();
        windowsActivator.ActivateDeactivateWindow(WindowType.LevelsChoosingWindow, isActivate);
    }

    public void ActivateDeactivateStoreWindow(bool isActivate)
    {
        ActivateSound();
        windowsActivator.ActivateDeactivateWindow(WindowType.StoreWindow, isActivate);
    }

    public void ActivateDeactivateSettingsWindow(bool isActivate)
    {
        ActivateSound();
        windowsActivator.ActivateDeactivateWindow(WindowType.SettingsWindow, isActivate);
    }

    public void ActivateDeactivatePauseWindow (bool isActivate)
    {
        ActivateSound();
        playerController.isCanMove = !isActivate;
        buttons.SetActive(!isActivate);
        Time.timeScale = isActivate ? 0f : 1f;
        windowsActivator.ActivateDeactivateWindow(WindowType.PauseWindow, isActivate);
    }

    public void DeactivateContinueWindow()
    {
        ActivateSound();
        windowsActivator.ActivateDeactivateWindow(WindowType.ContinueWindow, false);
        windowsActivator.ActivateDeactivateWindow(WindowType.LoseWindow, true);
    }

    public void DeactivateSkinWindow()
    {
        windowsActivator.ActivateDeactivateWindow(WindowType.SkinWindow, false);
        SkinManager.instance.SetSkinIsntInterested();
    }

    public void DeactivateSkinWindowWithADV()
    {
        windowsActivator.ActivateDeactivateWindow(WindowType.SkinWindowWithADV, false);
        SkinManager.instance.SetSkinIsntInterested();
    }

    public void ShowADVForCoins(float value)
    {
        ADVManager.Instance.ShowADVForCoins(value);
#if UNITY_EDITOR
        GameObject.Find("Rewarder").GetComponent<Rewarder>().MultiplyReward(value);
#endif
    }

    public void ShowADVForContinue()
    {
        ADVManager.Instance.ShowADVForContinue();
#if UNITY_EDITOR
        GameObject.Find("GameService").GetComponent<GameService>().ContinueGame();
#endif
    }

    public void ShowADVForRecommendedSkin()
    {
        ADVManager.Instance.ShowADVForRecommendedSkin();
#if UNITY_EDITOR
        BuyRecommendedSkin();
#endif
    }

    public void BuyRecommendedSkin()
    {
        SkinManager.instance.BuyRecommendedSkin();
    }

    private void ActivateSound()
    {
        AudioManager.instance.PlaySound("Click");
    }
}
