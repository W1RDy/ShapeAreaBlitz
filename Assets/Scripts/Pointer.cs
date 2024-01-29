using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] float speed;
    private DirectionalMove directionalMove;
    private Vector3 startPosition;
    ParticleSystem _particleSystem;

    public void InitializePointer(DirectionType[] directionTypes, float movingTime)
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        startPosition = transform.position;
        directionalMove = GetComponent<DirectionalMove>();
        directionalMove.SetSpeed(speed);
        _particleSystem.Play();
        StartCoroutine(Point(movingTime, directionTypes));
    }

    private IEnumerator Point(float movingTime, DirectionType[] directionTypes)
    {
        while (true)
        {
            foreach (DirectionType directionType in directionTypes)
            {
                directionalMove.SetDirection(directionType);
                directionalMove.SetMovableState(true);
                yield return new WaitForSeconds(movingTime / directionTypes.Length);
            }
            directionalMove.SetMovableState(false);
            yield return new WaitForSeconds(0.5f);
            _particleSystem.Stop();
            yield return new WaitForSeconds(0.5f);
            transform.position = startPosition;
            _particleSystem.Play();
        }
    }

    public void StopPoint()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
