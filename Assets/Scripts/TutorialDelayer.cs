using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDelayer : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameService gameService;
    [SerializeField] BossBattleService bossBattleService;
    Tutorial tutorial;
    PlayerMove playerMovable;
    CoroutineManager coroutineManager;
    bool isButtonClicked;
    Func<bool, bool> IsClicked;

    private void Start()
    {
        tutorial = GetComponent<Tutorial>();
        try
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            playerMovable = player.GetComponent<PlayerMove>();
            coroutineManager = GameObject.Find("CoroutineManager").GetComponent<CoroutineManager>();
        }
        catch { }

        IsClicked = isChange =>
        {
            if (isChange) isButtonClicked = true;
            return isButtonClicked;
        };
    }

    public void DelayForMoving(DirectionType[] directionTypes)
    {
        var coroutineIndex = 0;
        foreach (var directionType in directionTypes)
            coroutineManager.StartCoroutineWithOrder(WaitUntilMoving(directionType, ++coroutineIndex, directionTypes.Length));
    }

    public void DelayForEnemyClosing(List<MovableEnemy> enemies) => StartCoroutine(WaitUntilEnemyBeClose(enemies));

    public void DelayForTime(float time) => StartCoroutine(WaitSomeTime(time));

    public void DelayForTutorialElement(TutorialElementConfig tutorialElement, TutorialActivatedObjectsType objectsType)
    {
        if (objectsType == TutorialActivatedObjectsType.CameZone) StartCoroutine(WaitUntilPlayerCame(tutorialElement.camePlaces));
        else if (objectsType == TutorialActivatedObjectsType.Simulation) StartCoroutine(WaitUntilBoss());
        else if (objectsType == TutorialActivatedObjectsType.BossBattle) StartCoroutine(WaitUntilBossFinishAttack());
        else if (objectsType == TutorialActivatedObjectsType.Dialog) StartCoroutine(WaitWhileDialog());
        else if (objectsType == TutorialActivatedObjectsType.Button) StartCoroutine(WaitUntilClick(tutorialElement.button));
    }

    private IEnumerator WaitUntilMoving(DirectionType directionType, int coroutineIndex, int directionsCount)
    {
        UnavailableDirectionsManager.instance.AddAllDirectionsExcept(directionType);
        yield return new WaitUntil(playerMovable.IsMoving);
        if (coroutineIndex >= directionsCount) playerController.isCanMove = false;
        yield return new WaitWhile(playerMovable.IsMoving);
        if (coroutineIndex >= directionsCount)
        {
            tutorial.ShowNextElement();
            yield return null;
            playerController.isCanMove = true;
        }
    }

    private IEnumerator WaitUntilEnemyBeClose(List<MovableEnemy> enemies)
    {
        yield return new WaitUntil(() => enemies[0].IsCloseToPlayer(playerMovable.transform, 5));
        StartStopAllEnemiesMoving(enemies, false);
        tutorial.ShowNextElement();
        gameService.generalGameSpeed /= 2;
        yield return new WaitUntil(playerMovable.IsMoving);
        StartStopAllEnemiesMoving(enemies, true);
    }

    private IEnumerator WaitUntilPlayerCame(Transform[] camePlace)
    {
        yield return new WaitUntil(() => playerMovable.PlayerIsCame(camePlace));
        tutorial.ShowNextElement();
    }

    private IEnumerator WaitUntilBoss()
    {
        yield return new WaitUntil(gameService.IsBossStage);
        tutorial.ShowNextElement();
    }

    private IEnumerator WaitUntilBossFinishAttack()
    {
        yield return new WaitWhile(bossBattleService.IsActivated);
        tutorial.ShowNextElement();
    }

    private IEnumerator WaitSomeTime(float time)
    {
        yield return new WaitForSeconds(time);
        tutorial.ShowNextElement();
    }

    private IEnumerator WaitWhileDialog()
    {
        yield return new WaitUntil(() => DialogManager.instance.DialogIsFinished() || DialogManager.instance.DialogIsStopped());
        tutorial.ShowNextElement();
    }

    private IEnumerator WaitUntilClick(Button button)
    {
        button.onClick.AddListener(() => IsClicked(true));
        yield return new WaitUntil(() => IsClicked(false));
        button.onClick.RemoveAllListeners();
        isButtonClicked = false;
        tutorial.ShowNextElement();
    }

    private void StartStopAllEnemiesMoving(List<MovableEnemy> enemies, bool isStart)
    {
        foreach (var enemy in enemies) 
            if (enemy) enemy.StartStopEnemyMoving(isStart);        
    }
}
