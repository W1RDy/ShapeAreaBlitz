using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class FireSplash : MonoBehaviour
{
    [SerializeField] float defaultSpeed = 6f;
    [SerializeField, ReadOnly] float speed;
    [SerializeField] Transform target;
    IMovable movable;
    GameService gameService;
    Action ChangeValueByDifficulty;

    void Start()
    {
        SetValueChanger();
        ChangeValueByDifficulty();

        movable = GetComponent<IMovable>();
        movable.SetSpeed(speed);
        transform.rotation = AngleService.GetAngleByTarget(transform, target);
    }

    public void SetTarget(Transform target) => this.target = target;

    private void OnTriggerEnter2D(Collider2D other) => Destroy(gameObject);

    public void SetValueChanger()
    {
        ChangeValueByDifficulty = () =>
        {
            speed = ChangerValueByDifficulty.instance.GetValueByDifficult(true, defaultSpeed);
            if (movable != null) movable.SetSpeed(speed);
        };
        gameService = GameObject.Find("GameService").GetComponent<GameService>();
        gameService.SetLevelDifficulty += ChangeValueByDifficulty;
    }

    private void OnDestroy()
    {
        gameService.SetLevelDifficulty -= ChangeValueByDifficulty;
    }
}
