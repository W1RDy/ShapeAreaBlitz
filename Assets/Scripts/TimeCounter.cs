using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public static TimeCounter instance;
    Text counter;
    Action<float> callback;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            counter = instance.GetComponentInChildren<Text>();
            counter.gameObject.SetActive(false);
        }
        else
        {
            var camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CompositeCamera>().GetCamera();
            instance.GetComponent<Canvas>().worldCamera = camera;
            Destroy(gameObject);
        }

        DontDestroyOnLoad(instance);

        callback = size => counter.resizeTextMaxSize = (int)size;
    }

    public void ActivateCounter(int time)
    {
        counter.gameObject.SetActive(true);
        Debug.Log(counter.gameObject.activeInHierarchy);
        StartCoroutine(Counter(time));
    }

    private IEnumerator Counter(int time)
    {
        var colorStep = time / 3;
        while (time > 0)
        {
            if (colorStep >= time) counter.color = Color.red;
            else if (colorStep * 2 >= time) counter.color = Color.yellow;
            
            counter.text = time.ToString();
            SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(100), 200, 1f, callback);
            yield return new WaitForSeconds(1);
            time--;
        }
        counter.text = "";
        counter.color = Color.green;
        counter.gameObject.SetActive(false);
    }
}
