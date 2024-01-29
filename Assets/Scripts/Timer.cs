using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float timeForBoss;
    Text timeIndicator;
    int time = 0;
    EventActivator events;
    GameService gameService;
    bool isCount = false;

    void Start()
    {
        timeIndicator = GetComponent<Text>();
        events = GameObject.Find("EventsActivator").GetComponent<EventActivator>();
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
        timeForBoss = gameService.isTutorial ? 15 : 30;
    }

    public void StartTimer()
    {
        if (!isCount && gameService.isPlay)
        {
            isCount = true;
            StartCoroutine(Count());
        }
    }

    IEnumerator Count()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!isCount) break;
            time ++;
            timeIndicator.text = time.ToString();
            if (time == 5) events.ActivateEvent(EventType.ActivateSpawner, (int)SpawnerType.AdditiveEnemySpawner); 
            if (time == timeForBoss) events.ActivateBoss();  
        }
    }

    public void StopTimer() => isCount = false;

    public int GetTime() => time;
}
