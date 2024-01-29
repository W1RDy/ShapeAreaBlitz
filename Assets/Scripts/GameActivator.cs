using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActivator : MonoBehaviour
{
    [SerializeField] GameService gameService;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if (!gameService.isTutorial) ActivateGame();
        }
    }

    public void ActivateGame()
    {
        gameService.StartGame();
        Destroy(gameObject);
    }
}
