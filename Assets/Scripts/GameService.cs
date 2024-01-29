using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameService : MonoBehaviour
{
    [Header("LevelSettings")]
    [SerializeField] int levelIndex;
    public bool isPlay;
    public bool isTutorial = false;
    public BossType bossOnLevel;
    public bool isBossStage;
    public int maxCoinsInLevel;
    [HideInInspector] public BossType currentBoss;
    [HideIf(nameof(isTutorial)), SerializeField] public float generalGameSpeed;
    [HideIf(nameof(isTutorial)), SerializeField, FormerlySerializedAs("maxGameSpeed")] float defaultMaxGameSpeed;
    [HideIf(nameof(isTutorial)), SerializeField, ReadOnly] float maxGameSpeed;
    [HideIf(nameof(isTutorial)), SerializeField, FormerlySerializedAs("speedChangeCooldown")] float defaultSpeedChangeCooldown = 6f;
    [HideIf(nameof(isTutorial)), SerializeField, ReadOnly] float speedChangeCooldown;
    [HideIf(nameof(isTutorial)), SerializeField] public int defaultRewardOnLevel;
    [HideIf(nameof(isTutorial)), SerializeField] public int defaultScoreOnLevel;
    [Range(0.1f, 2)] public float levelDifficulty;
    [HideInInspector] public bool isCanContinue = false;
    bool isChangeGameSpeed;
    bool isFirstChance = true;
    public int LevelIndex { get => levelIndex; }

    [Header("AdditiveComponents")]
    [SerializeField] Timer timer;
    [SerializeField] EventActivator events;
    [SerializeField] WindowsActivator windowsActivator;
    [SerializeField] BossBattleService battleService;
    [SerializeField] Rewarder rewarder;
    GameObject buttons;
    PlayerController controller;
    Action ChangeMaxGameSpeedByDifficulty;
    Action ChangeSpeedCooldownByDifficulty;
    public event Action SetLevelDifficulty;

    [Button]
    public void ChangeLevelDifficulty()
    {
        SetLevelDifficulty();
    }

    private void Awake()
    {
        buttons = GameObject.Find("Canvas/Buttons");
        controller = GameObject.Find("Canvas").GetComponent<PlayerController>();
        ChangeMaxGameSpeedByDifficulty = () => maxGameSpeed = defaultMaxGameSpeed * levelDifficulty;
        ChangeSpeedCooldownByDifficulty = () => speedChangeCooldown = defaultSpeedChangeCooldown / levelDifficulty;
        SetLevelDifficulty += ChangeMaxGameSpeedByDifficulty;
        SetLevelDifficulty += ChangeSpeedCooldownByDifficulty;
    }

    private void Start()
    {
        //levelDifficulty = PlayerPrefs.GetFloat("difficulty", 1);
        ChangeLevelDifficulty();
    }

    public void StartGame()
    {
        isPlay = true;
        StartChangeGameSpeed();
        timer.StartTimer();
        //events.ActivateEventsByChanceActivator();
        events.ActivateEvent(EventType.ActivateSpawner, (int)SpawnerType.MainEnemySpawner);
        if (!isTutorial) events.ActivateEvent(EventType.ActivateSpawner, (int)SpawnerType.BonusSpawner);
    }

    IEnumerator GameSpeedChanger()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedChangeCooldown);
            generalGameSpeed += 0.1f;
            if (generalGameSpeed >= maxGameSpeed && isChangeGameSpeed)
            {
                generalGameSpeed = maxGameSpeed;
                isChangeGameSpeed = false;
            }

            if (!isChangeGameSpeed) break;
        }
    }

    public void FinishGameWithoutWindow(bool isFinishWithWindow)
    {
        if (battleService.isActivated) battleService.FinishBossBattle(!isFirstChance);
        timer.StopTimer();
        events.ActivateEvent(EventType.DestroyAllEnemies);
        events.DeactivateEvent(EventType.ActivateSpawner, (int)SpawnerType.AllSpawners);
        controller.isCanMove = false;
        buttons.SetActive(false);
        isPlay = false;
        if (!isFinishWithWindow && ADVManager.Instance) ADVManager.Instance.ShowADV();
    }

    public void ContinueGame()
    {
        isFirstChance = false;
        isPlay = true;
        if (timer.GetTime() >= 30f) battleService.ActivateBossBattle();
        events.ActivateEvent(EventType.ActivateSpawner, (int)SpawnerType.AllSpawners);
        windowsActivator.ActivateDeactivateWindow(WindowType.ContinueWindow, false);

        timer.StartTimer();
        controller.isCanMove = true;
        buttons.SetActive(true);
        AudioManager.instance.PauseStartAudio("true");
    }

    public void FinishGame(bool isWin, float windowTimeout)
    {
        if (isWin) LevelManager.instance.ActivateNextLevel(windowTimeout, levelIndex);
        FinishGameWithoutWindow(true);

        WindowType windowType;
        if (isWin) windowType = isTutorial ? WindowType.FakeLoseWindow : WindowType.WinWindow;
        else windowType = isFirstChance && !isTutorial ? WindowType.ContinueWindow : WindowType.LoseWindow;
  
        StartCoroutine(WaitForWindowActivated(windowType, windowTimeout));
    }

    private IEnumerator WaitForWindowActivated(WindowType windowType, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        windowsActivator.ActivateDeactivateWindow(windowType, true);
        yield return null;
        if (windowType == WindowType.WinWindow)
        {
            rewarder.CollectReward();
            AudioManager.instance.PlaySound("Win");
        }
        else AudioManager.instance.PlaySound("Lose");

        if (windowType == WindowType.FakeLoseWindow) ScoreCounter.instance.AddScore(100);
        yield return new WaitForSeconds(0.5f);
        if (ADVManager.Instance) ADVManager.Instance.ShowADV();
        yield return new WaitForSeconds(0.5f);
        if (windowType == WindowType.WinWindow && levelIndex > 1)
        {
            if (ADVManager.Instance && ADVManager.Instance.ADVIsShowed) windowsActivator.ActivateDeactivateWindow(WindowType.SkinWindow, true);
            else windowsActivator.ActivateDeactivateWindow(WindowType.SkinWindowWithADV, true);
        }
        else if (SkinManager.instance.CanRememberAboutSkin() && windowType == WindowType.ContinueWindow)
        {
            windowsActivator.ActivateDeactivateWindow(WindowType.SkinWindow, true);
        }
    }

    public void StopChangeGameSpeed() => isChangeGameSpeed = false;

    public void StartChangeGameSpeed()
    {
        isChangeGameSpeed = true;
        StartCoroutine(GameSpeedChanger());
    }

    public bool IsBossStage() => isBossStage;
}
