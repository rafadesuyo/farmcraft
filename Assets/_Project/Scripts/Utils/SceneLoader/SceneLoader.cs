using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] private float minimumLoadingTime = 0.2f;
    private float lastSceneLoadingTime = 0f;

    public void LoadScene(string sceneName, Action onFinishStageLoad = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, onFinishStageLoad));
    }

    private IEnumerator LoadSceneAsync(string sceneName, Action onFinishStageLoad)
    {
        yield return Spinner.Instance.TransitionIn();

        GenericPool.ClearPools();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        float initialTime = Time.timeSinceLevelLoad;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        lastSceneLoadingTime = Time.timeSinceLevelLoad - initialTime;

        while (lastSceneLoadingTime < minimumLoadingTime)
        {
            lastSceneLoadingTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitUntil(() => asyncLoad.isDone == true);

        EventsManager.Publish(EventsManager.onSceneLoad);
        onFinishStageLoad?.Invoke();

        yield return Spinner.Instance.TransitionOut();
    }
}
