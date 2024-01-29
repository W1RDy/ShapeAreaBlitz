using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjectsActivator : MonoBehaviour
{
    [SerializeField] EnemySpawner[] spawners;
    [SerializeField] RetractableObjActivator blockForHitActivator;
    [SerializeField] GameService gameService;
    List<MovableEnemy> enemies = new List<MovableEnemy>();
    Transform player;
    TutorialDelayer delayer;
    Transform boss;

    private void Start()
    {
        delayer = GetComponent<TutorialDelayer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void InitializeBoss(Transform boss) => this.boss = boss; 

    public void ActivateSurrounding()
    {
        enemies.Clear();
        ActivateEnemyOnSameWay("Horizontal", "Vertical");
    }

    public void ActivateEnemyOnSameWay(params string[] wayDirectionIndexes)
    {
        UnavailableDirectionsManager.instance.AddAllDirectionsExcept(null);
        gameService.generalGameSpeed *= 2;
        foreach (var directionIndex in wayDirectionIndexes)
        {
            foreach (var spawner in spawners)
                AddEnemy(spawner.SpawnEnemyOnSameWay(player, EnemyType.TutorialEnemy, directionIndex));
        }
        delayer.DelayForEnemyClosing(enemies);
    }

    public DirectionType GetDirectionByObstacle(Transform obstacle)
    {
        var direction = DirectionService.GetMaxCoordDirectionToTarget(player, obstacle).normalized;
        var directionType = DirectionService.GetDirectionType(direction);
        return directionType.Value;
    }

    public Transform GetConnectedObjectByIndex(string index)
    {
        if (index == "Enemy") return enemies[enemies.Count - 1].transform;
        else if (index == "BlockForHit") return blockForHitActivator.GetCurrentRetractableObject()?.transform;
        else if (index == "Boss") return boss;
        throw new System.InvalidOperationException(index + " is incorrect index!");
    }

    public void AddEnemy(MovableEnemy enemy)
    {
        if (enemy != null)
        {
            if (enemies.Count > 0 && enemies[enemies.Count - 1] == null) enemies.Clear();
            enemies.Add(enemy);
        }
    }
}
