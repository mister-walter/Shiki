using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.Constants;

public class MainSceneManager : MonoBehaviour {
	void Start () {
        // Load all of the scenes, additively
#if UNITY_EDITOR
#else
        SceneManager.LoadSceneAsync(SceneName.Village, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(SceneName.Spring, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(SceneName.Summer, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(SceneName.Fall, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(SceneName.Winter, LoadSceneMode.Additive);
#endif
    }

    /// <summary>
    /// Converts the name of a scene to the name of the corresponding season.
    /// </summary>
    /// <param name="sceneName">The name of the scene to get the season of.</param>
    /// <returns>The name of the corresponding season, or null if it does not exist.</returns>
    public static string SceneNameToSeasonName(string sceneName)
    {
        switch(sceneName)
        {
            case SceneName.Fall: return SeasonName.Fall;
            case SceneName.Winter: return SeasonName.Winter;
            case SceneName.Spring: return SeasonName.Spring;
            case SceneName.Summer: return SeasonName.Summer;
            default: return null;
        }
    }
}

public static class AsyncOperationExtensions
{
    // from https://gamedev.stackexchange.com/q/120643
    public static IEnumerator Await(this AsyncOperation operation)
    {
        while (!operation.isDone)
            yield return operation;
    }
}
