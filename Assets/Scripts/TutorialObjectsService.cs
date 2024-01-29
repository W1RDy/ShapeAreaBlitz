using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TutorialObjectsService : MonoBehaviour
{
    [SerializeField] TutorialObjectsActivator tutorialObjectsActivator;
    [SerializeField] Pointer pointer;
    [SerializeField] TutorialDelayer delayer;
    [SerializeField] BossBattleService battleService;
    [SerializeField] HealthIndicator healthIndicator;
    public Pointer currentPointer;
    GameService gameService;
    Transform player;

    private void Start()
    {
        try { 
            player = GameObject.FindGameObjectWithTag("Player").transform;
            gameService = GameObject.Find("GameService").GetComponent<GameService>();
        }
        catch { }
    }

    public Action<TutorialElementConfig> GetEnabledAction(TutorialActivatedObjectsType objectsType)
    {
        switch (objectsType)
        {
            case TutorialActivatedObjectsType.FirstEnemy:
                return element => tutorialObjectsActivator.ActivateEnemyOnSameWay("Horizontal");
            case TutorialActivatedObjectsType.Surrounding:
                return element => tutorialObjectsActivator.ActivateSurrounding();
            case TutorialActivatedObjectsType.Simulation:
                return element => ActivateSimulation();
            case TutorialActivatedObjectsType.BossBattle:
                return element => ActivateBossBattle();
            case TutorialActivatedObjectsType.FakeLoseWindow:
                return element => FinishTutorialLevel(2f);
            case TutorialActivatedObjectsType.Pointer:
                return element => EnablePointer(element);
            case TutorialActivatedObjectsType.Dialog:
                return element => EnableBranch(element);
            case TutorialActivatedObjectsType.Outline:
                return element => EnableOutline(element);
            case TutorialActivatedObjectsType.CameZone:
                return element => EnableCame(element);
            case TutorialActivatedObjectsType.Button:
                return element => EnableButton(element);
            case TutorialActivatedObjectsType.TutorialEnding:
                return element => DisableTutorial();
        }
        throw new NullReferenceException(objectsType + "is incorrect TutorialActivatedObjectsType!");
    }

    private void ActivateSimulation()
    {
        gameService.StartGame();
        delayer.DelayForTutorialElement(null, TutorialActivatedObjectsType.Simulation);
    }

    private void ActivateBossBattle()
    {
        battleService.ActivateBossBattle();
        delayer.DelayForTutorialElement(null, TutorialActivatedObjectsType.BossBattle);
    }

    private void FinishTutorialLevel(float delay)
    {
        AudioManager.instance.PlaySound("TakeHit");
        healthIndicator.ChangeHealthIndicator(0);
        gameService.FinishGame(true, delay);
        delayer.DelayForTime(delay);
    }

    private void EnablePointer(TutorialElementConfig tutorialElement)
    {
        currentPointer = Instantiate(pointer.gameObject, new Vector2(0, 2f), Quaternion.identity).GetComponent<Pointer>();
        DirectionType[] directionTypes = DirectionService.GetTutorialDirectionTypes(tutorialElement, tutorialObjectsActivator, player);
        currentPointer.InitializePointer(directionTypes, 0.5f);
        delayer.DelayForMoving(directionTypes);
    }

    private void EnableBranch(TutorialElementConfig tutorialElement)
    {
        DialogManager.instance.StartDialogBranch(tutorialElement.branchName);
        delayer.DelayForTutorialElement(tutorialElement, TutorialActivatedObjectsType.Dialog);
    }

    private void EnableOutline(TutorialElementConfig tutorialElement)
    {
        OutlineManager.instance.OutlineObject(tutorialElement.outlinedObjects, false);
    }

    private void EnableButton(TutorialElementConfig tutorialElement)
    {
        delayer.DelayForTutorialElement(tutorialElement, TutorialActivatedObjectsType.Button);
    }

    private void EnableCame(TutorialElementConfig tutorialElement)
    {
        delayer.DelayForTutorialElement(tutorialElement, TutorialActivatedObjectsType.CameZone);
    }

    private void DisableTutorial()
    {
        //PlayerPrefs.SetString("Tutorial", "false");
        DataContainer.Instance.playerData.isNeedTutorial = false;
        DataContainer.Instance.SaveDataOnServer();
        //InteractorWithBrowser.Authorize();
    }
}
