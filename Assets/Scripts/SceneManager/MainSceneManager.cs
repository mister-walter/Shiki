using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneName
{
    public const String Summer = "SummerScene";
    public const String Winter = "WinterScene";
    public const String Spring = "SpringScene";
    public const String Fall = "FallScene";
    public const String Village = "VillageScene";
    public const String Main = "MainScene";
}

public static class SeasonName
{
    public const String Summer = "Summer";
    public const String Winter = "Winter";
    public const String Spring = "Spring";
    public const String Fall = "Fall";
}

public class MainSceneManager : MonoBehaviour {
    private int villageSceneIndex;
    private int mainSceneIndex;

	// Use this for initialization
	void Start () {
        SceneManager.LoadSceneAsync(SceneName.Village, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(SceneName.Spring, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(SceneName.Summer, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(SceneName.Fall, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(SceneName.Winter, LoadSceneMode.Additive);
        villageSceneIndex = SceneUtility.GetBuildIndexByScenePath(SceneName.Village);
        mainSceneIndex = SceneUtility.GetBuildIndexByScenePath(SceneName.Main);
    }

    public IEnumerable<Scene> GetLoadedSeasonScenes()
    {
        yield return SceneManager.GetSceneByName(SceneName.Spring);
        yield return SceneManager.GetSceneByName(SceneName.Summer);
        yield return SceneManager.GetSceneByName(SceneName.Fall);
        yield return SceneManager.GetSceneByName(SceneName.Winter);
    }

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
