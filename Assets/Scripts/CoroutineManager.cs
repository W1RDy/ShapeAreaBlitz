using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    List<IEnumerator> activatedCoroutines;

    void Start()
    {
        activatedCoroutines = new List<IEnumerator>();
    }

    public void StartCoroutineWithOrder(IEnumerator coroutine)
    {
        activatedCoroutines.Add(coroutine);
        if (activatedCoroutines.Count == 1) StartCoroutine(WaitWhileCoroutineEnds());
    }

    private IEnumerator WaitWhileCoroutineEnds()
    {
        while (true)
        {
            yield return StartCoroutine(activatedCoroutines[0]);
            activatedCoroutines.RemoveAt(0);
            if (activatedCoroutines.Count == 0) break;
        }
    }
}
