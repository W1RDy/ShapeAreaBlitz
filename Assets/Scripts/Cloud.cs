using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cloud : MonoBehaviour
{
    [SerializeField] Sprite[] clouds;
    [SerializeField] Vector2[] scaleInterval;
    [SerializeField] bool isSaveProportions;
    [SerializeField] float speed = 0.5f;
    float scaleX, scaleY;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = clouds[Random.Range(0, clouds.Length)];
        SetScale();
        GetComponent<IMovable>().SetSpeed(speed);
    }

    private void SetScale()
    {
        scaleX = Random.Range(scaleInterval[0].x, scaleInterval[1].x);
        if (isSaveProportions) scaleY = scaleX;
        else scaleY = Random.Range(scaleInterval[0].y, scaleInterval[1].y);
        transform.localScale = new Vector2(scaleX, scaleY);
    }
}
