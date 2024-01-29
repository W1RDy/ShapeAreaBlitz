using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Image progressBar;
    [SerializeField] GameObject[] loadingElements;
    [SerializeField] float defaultLoadingTime;
    List<IColorable> loadingColorablesElements;
    Action<float> callback;

    public static LoadSceneManager instance;
    public static bool isNeedCloseLoadingScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeColorableElements();

            callback = opacity =>
            {
                foreach (var element in loadingColorablesElements)
                {
                    var color = element.GetColor();
                    element.SetColor(new Color(color.r, color.g, color.b, opacity));
                }
            };
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);

        if (isNeedCloseLoadingScreen) instance.ActivateDeactivateLoadingScreen(false);
    }

    private void InitializeColorableElements()
    {
        loadingColorablesElements = new List<IColorable>();

        foreach (var element in loadingElements)
        {
            var colorable = element.GetComponent<IColorable>();
            if (colorable == null) throw new NullReferenceException(element + "doesn't realize IColorable interface!");
            else loadingColorablesElements.Add(colorable);
        }
    }

    public void LoadScene(int sceneIndex) => LoadScene(sceneIndex, defaultLoadingTime);

    public void LoadScene(int sceneIndex, float loadingPanelTime)
    {
        StartCoroutine(AsyncLoading(sceneIndex, loadingPanelTime));
    }

    public void LoadNextScene() => LoadNextScene(defaultLoadingTime);

    public void LoadNextScene(float loadingPanelTime)
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        LoadScene(sceneIndex, loadingPanelTime);
    }

    public void ReloadCurrentScene() => ReloadCurrentScene(defaultLoadingTime);

    public void ReloadCurrentScene(float loadingPanelTime)
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(sceneIndex, loadingPanelTime);
    }

    private void ActivateDeactivateLoadingScreen(bool isActivate)
    {
        (float starValue, float endValue)? values = null;
        if (isActivate)
        {
            if (!canvas.gameObject.activeInHierarchy)
            {
                values = (0, 1);
                canvas.gameObject.SetActive(isActivate);
            }
            progressBar.fillAmount = 0;
        }
        else
        {
            StartCoroutine(WaitUntilLoadingScreenDisappear(defaultLoadingTime));
            values = (1, 0);
        }

        if (values != null)
            StartCoroutine(SmoothChanger<SmoothableFloat, float>.SmoothChange(new SmoothableFloat(values.Value.starValue), values.Value.endValue, defaultLoadingTime, callback));
    }

    private IEnumerator WaitUntilLoadingScreenDisappear(float time)
    {
        yield return new WaitForSeconds(time);
        canvas.gameObject.SetActive(false);
        isNeedCloseLoadingScreen = false;
    }

    private IEnumerator AsyncLoading(int sceneIndex, float loadingPanelTime)
    {
        ActivateDeactivateLoadingScreen(true);
        yield return new WaitForSeconds(loadingPanelTime);

        var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            progressBar.fillAmount = asyncOperation.progress;
            yield return null;
        }

        isNeedCloseLoadingScreen = true;
        asyncOperation.allowSceneActivation = true;
        progressBar.fillAmount = 1;
    }
}
