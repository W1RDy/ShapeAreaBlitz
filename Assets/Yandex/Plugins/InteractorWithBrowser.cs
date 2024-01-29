using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class InteractorWithBrowser
{
    //[DllImport("__Internal")]
    //private static extern void AuthorizeExtern();
    [DllImport("__Internal")]
    private static extern void RateGame();
    [DllImport("__Internal")]
    private static extern void SaveExtern(string data);
    [DllImport("__Internal")]
    private static extern void LoadExtern();
    [DllImport("__Internal")]
    private static extern void ShowAdv();
    [DllImport("__Internal")]
    private static extern void MultiplyCoinsForAdvExtern(float value);
    [DllImport("__Internal")]
    private static extern void AddSecondLifeExtern();
    [DllImport("__Internal")]
    private static extern void SaveScoreInLeaderboardExtern(int score);
    [DllImport("__Internal")]
    private static extern void PurchaseRecommendedSkinForADVExtern();
    [DllImport("__Internal")]
    private static extern string GetLanguageExtern();
    [DllImport("__Internal")]
    private static extern string PlayerIsInitialized();

    //    public static void Authorize()
    //    {
    //#if !UNITY_EDITOR && UNITY_WEBGL
    //        AuthorizeExtern();
    //#endif
    //    }
    public static void SuggestRateGame()
    {
        Debug.Log("Rate");
#if !UNITY_EDITOR && UNITY_WEBGL
        RateGame();
#endif
    }

    public static void SaveDataOnServer(PlayerData playerData)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        var jsonString = JsonUtility.ToJson(playerData);
        SaveExtern(jsonString);
#endif
    }

    public static void LoadData()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        LoadExtern();
#endif
    }

    public static void ShowAdversity()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        AudioManager.instance.PauseStartAudio("false");
        ShowAdv();
#endif
#if UNITY_EDITOR
        AudioManager.instance.PauseStartAudio("false");
        Debug.Log("ADV");
        AudioManager.instance.PauseStartAudio("true");
#endif
    }

    public static void MultiplyCoinsForAdv(float value)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        AudioManager.instance.PauseStartAudio("false");
        MultiplyCoinsForAdvExtern(value);
#endif
#if UNITY_EDITOR
        AudioManager.instance.PauseStartAudio("false");
        Debug.Log("ADV");
        AudioManager.instance.PauseStartAudio("true");
#endif
    }

    public static void AddSecondLife()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        AudioManager.instance.PauseStartAudio("false");
        AddSecondLifeExtern();
#endif
#if UNITY_EDITOR
        AudioManager.instance.PauseStartAudio("false");
        Debug.Log("ADV");
        GameObject.Find("Reborner").GetComponent<Reborner>().Reborn();
        AudioManager.instance.PauseStartAudio("true");
#endif
    }

    public static void SaveScoreInLeaderboard(int score)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        SaveScoreInLeaderboardExtern(score);
#endif
    }

    public static void PurchaseRecommendedSkinForADV()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        AudioManager.instance.PauseStartAudio("false");
        PurchaseRecommendedSkinForADVExtern();
#endif
#if UNITY_EDITOR
        AudioManager.instance.PauseStartAudio("false");
        Debug.Log("ADV");
        AudioManager.instance.PauseStartAudio("true");
#endif
    }

    public static string GetLanguage()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        return GetLanguageExtern();
#endif
#if UNITY_EDITOR
        return "ru";
#endif  
    }

    public static bool IsInitialized()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        var str = PlayerIsInitialized();
        return System.Convert.ToBoolean(str);
#endif
#if UNITY_EDITOR
        return true;
#endif  
    }
}
