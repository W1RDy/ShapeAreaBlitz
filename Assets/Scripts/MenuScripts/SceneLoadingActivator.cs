using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadingActivator : MonoBehaviour
{
    private void Start()
    {
        //bool isTutorial = System.Convert.ToBoolean(PlayerPrefs.GetString("Tutorial", "true"));
        StartCoroutine(WaitUntilDataLoading());
    }

    private IEnumerator WaitUntilDataLoading()
    {
        yield return new WaitUntil(DataContainer.Instance.DataIsLoading);
        bool isTutorial = DataContainer.Instance.playerData.isNeedTutorial;
        var sceneIndex = isTutorial ? 2 : 1;
        LoadSceneManager.instance.LoadScene(sceneIndex);
    }
}
