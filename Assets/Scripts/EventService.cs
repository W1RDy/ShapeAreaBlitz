using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EventService : MonoBehaviour
{
    [SerializeField] EventConfig[] eventConfigs;
    [SerializeField] Transform objects;
    [SerializeField] PlayerController playerController;
    [SerializeField] SpawnersController spawnersController;
    [SerializeField] BossActivator bossActivator;
    [SerializeField] Transform playerPointByBoss;
    [SerializeField] CompositeCamera _camera;
    [SerializeField] WayService wayService;
    Transform room;
    Transform rotatedObject;
    PlayerMove playerMove;
    List<EventConfig> eventConfigsForChance;
    Dictionary<EventType, Event> eventsDict;
    Action<float> callbackRotation, callbackScale;
    public event Action DestroyAllEnemies;
    public event Action DestroyAllBonuses;
    public delegate void EventAction(bool isActivate, int typeIndex, ParametersConfigs parameters);
    int changeCounter;
    GameService gameService;

    private void Awake()
    {
        room = objects.GetChild(0);
        InitializeEventsCollections();
    }

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerMove = player.GetComponent<PlayerMove>();

        callbackRotation = angle =>
        {
            rotatedObject.eulerAngles = new Vector3(0, 0, angle);
            if (angle % 90 < 0.01)
            {
                changeCounter++;
                playerMove.ChangeDirectionByRotation(changeCounter % 2 == 1);
                if (changeCounter % 2 == 0) UnavailableDirectionsManager.instance.UpdateUnavailableDirections();
            }                
        };

        callbackScale = size => room.localScale = new Vector3(size, size, 1);
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
    }

    private void InitializeEventsCollections()
    {
        eventsDict = new Dictionary<EventType, Event>();
        eventConfigsForChance = new List<EventConfig>();

        foreach (var config in eventConfigs)
        {
            config._event.action = GetEventAction(config._event.type);
            eventsDict[config._event.type] = config._event;

            if (config.isChanceEvent) eventConfigsForChance.Add(config);
        }
    }

    public Event GetRandomEvent()
    {
        var eventConfig = RandomizerWithChances<EventConfig>.RandomWithChances(eventConfigsForChance.ToArray());
        if (eventConfig != null) return eventConfig._event;
        return null;
    }

    public Event GetEvent(EventType type) => eventsDict[type];

    public EventAction GetEventAction(EventType eventType)
    {
        switch (eventType)
        {
            case EventType.Rotate:
                return (isActivate, typeIndex, parameters) =>
                {
                    if (isActivate)
                    {
                        var rotationType = (RotationType)typeIndex;
                        bool isEventRotation = rotationType == RotationType.ObjectsFromTheCurrent ? true : false;
                        StartCoroutine(StopController(2f, isEventRotation));
                        rotatedObject = rotationType == RotationType.Objects || rotationType == RotationType.ObjectsFromTheCurrent ? objects : room;
                        float newAngle = rotationType == RotationType.Room || rotationType == RotationType.Objects ? newAngle = parameters["angle"] : rotatedObject.eulerAngles.z + parameters["angle"];

                        changeCounter = 0;
                        StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(rotatedObject.eulerAngles.z), newAngle, parameters["duration"], callbackRotation));
                    }
                };
            case EventType.ActivateSpawner:
                return (isActivate, typeIndex, values) =>
                {
                    if (isActivate) spawnersController.ActivateDeactivateSpawners((SpawnerType)typeIndex, true);
                    else spawnersController.ActivateDeactivateSpawners((SpawnerType)typeIndex, false);
                };
            case EventType.Boss:
                return (isActivate, typeIndex, parameters) =>
                {
                    if (isActivate)
                    {
                        DestroyAllBonuses?.Invoke();
                        _camera.ChangeSize(8);
                        var roomSize = gameService.LevelIndex == 2 ? 2 : 1.8f;
                        StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(1), roomSize, 2f, callbackScale));
                        var _event = GetEvent(EventType.Rotate);
                        _event.action(true, (int)RotationType.Room, _event.parametersConfigs);
                        playerMove.MoveToTarget(playerPointByBoss);
                        playerMove.SetDirection(Vector2.zero);
                        bossActivator.ActivateBoss((BossType)parameters["boss"]);
                    }
                };
            case EventType.DestroyAllEnemies:
                return (isActivate, typeIndex, values) =>
                {
                    if (DestroyAllEnemies != null)
                    {
                        DestroyAllEnemies();
                        wayService.ClearAllWays();
                    }
                };
        }
        throw new InvalidOperationException(eventType + "is incorrect event type!");
    }

    private IEnumerator StopController(float duration, bool isStopTime)
    {
        if (isStopTime) Time.timeScale = 0;
        playerController.isCanMove = false;
        yield return new WaitForSecondsRealtime(duration);
        if (isStopTime) Time.timeScale = 1;
        playerController.isCanMove = true;
    }
}
