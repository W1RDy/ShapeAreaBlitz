using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerMouse : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] Transform target;
    [SerializeField] Transform recycleBin;
    [SerializeField] CoroutineManager coroutineManager;
    Vector3 startPos = new Vector2(5.46f, -2.2f);
    Transform enemiesContainer;
    TargetMove targetMove;
    int enemyCount;

    private void Awake()
    {
        targetMove = GetComponent<TargetMove>();
        targetMove.SetMovableState(false);
        targetMove.SetSpeed(speed);
        targetMove.target = target;
    }

    private void Start()
    {
        enemiesContainer = GameObject.Find("Pool/EnemyContainer").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mouse") collision.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Mouse") collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public IEnumerator ThrowOut(Enemy enemy)
    {
        while (true)
        {
            yield return new WaitWhile(targetMove.IsMoving);
            if (!enemy.gameObject.activeInHierarchy) break;
            else if (target.localPosition == enemy.transform.localPosition)
            {
                enemy.transform.SetParent(transform);
                AudioManager.instance.PlaySound("MouseHold");
            }
            else if (target.position == recycleBin.position && enemy.transform.position == recycleBin.position)
            {
                enemy.StartDestroying(0);
                AudioManager.instance.PlaySound("MouseLet");
            }
            yield return new WaitForSeconds(0.0001f);
            MoveMouse(enemy);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemyCount--;
        enemy.transform.SetParent(enemiesContainer);
    }

    public void ThrowOutEnemy(Enemy enemy)
    {
        enemyCount++;
        coroutineManager.StartCoroutineWithOrder(ThrowOut(enemy));
    }

    private void MoveMouse(Enemy enemy)
    {
        if (enemyCount == 0) target.position = startPos;
        if (enemy.gameObject.activeInHierarchy)
        {
            if (target.position == enemy.transform.position) target.position = recycleBin.position;
            else target.position = enemy.transform.position;
        }

        targetMove.SetMovableState(true);
    }
}
