using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour {
    private int villageSceneIndex;
    private int mainSceneIndex;
    public static MainSceneManager GetMainSceneManager()
    {
        var mainScene = SceneManager.GetSceneByName("MainScene");
        foreach (var gameObject in mainScene.GetRootGameObjects())
        {
            Debug.Log(gameObject);
            var sceneManager = gameObject.GetComponent<MainSceneManager>();
            if (sceneManager != null) {
                return sceneManager;
            }
        }
        Debug.LogError("Couldnt find main scene manager");
        return null;
    }

	// Use this for initialization
	void Start () {
        SceneManager.LoadSceneAsync("VillageScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("SpringScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("SummerScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("FallScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("WinterScene", LoadSceneMode.Additive);
        villageSceneIndex = SceneUtility.GetBuildIndexByScenePath("VillageScene");
        mainSceneIndex = SceneUtility.GetBuildIndexByScenePath("MainScene");
        Debug.Log(villageSceneIndex);
    }

    public IEnumerable<Scene> GetLoadedSeasonScenes()
    {
        yield return SceneManager.GetSceneByName("SpringScene");
        yield return SceneManager.GetSceneByName("SummerScene");
        yield return SceneManager.GetSceneByName("FallScene");
        yield return SceneManager.GetSceneByName("WinterScene");
    }

    internal void UpdatePositionInOtherSeasons(Guid id, SeasonCoordinate coordinates, Scene exceptSeason)
    {
        foreach (var scene in GetLoadedSeasonScenes())
        {
            if (exceptSeason != scene)
            {
                var info = scene.GetSeasonInfo();
                info.UpdatePositionInSeason(id, coordinates);
            }
        }
    }

    /// <summary>
    /// Gets the season that a given position is part of, if any
    /// </summary>
    /// <param name="position"></param>
    /// <returns>The Scene for the season that the position is part of, if any, null otherwise</returns>
    public Scene? GetSeasonSceneFromPosition(Vector3 position)
    {
        foreach (var scene in GetLoadedSeasonScenes())
        {
            var info = scene.GetSeasonInfo();
            // Only check a scene if it has season information associated with it
            if (info != null)
            {
                if (info.IsPositionInSeason(position))
                {
                    return scene;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Gets the name of the season that a given position is part of, if any
    /// </summary>
    /// <param name="position"></param>
    /// <returns>The name of the season that the position is inside of, or the empty string</returns>
    public string GetSeasonNameFromPosition(Vector3 position)
    {
        var scene = GetSeasonSceneFromPosition(position);
        if(scene != null)
        {
            var info = scene.Value.GetSeasonInfo();
            return info.seasonName;
        }
        return "";
    }

    internal void RemoveFromOtherSeasons(Guid id, Scene exceptSeason)
    {
        foreach (var scene in GetLoadedSeasonScenes())
        {
            if (exceptSeason != scene)
            {
                var info = scene.GetSeasonInfo();
                info.RemoveFromSeason(id);
            }
        }
    }

    public void PlaceInOtherSeasons(GameObject obj, SeasonCoordinate coordinates, Scene exceptSeason)
    {
        foreach(var scene in GetLoadedSeasonScenes())
        {
            if(exceptSeason != scene)
            {
                var info = scene.GetSeasonInfo();
                info.PlaceInSeason(obj, coordinates);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}

// https://gamedev.stackexchange.com/q/120643
//public static class AsyncOperationExtensions {
//    public static IEnumerator Await(this AsyncOperation operation)
//    {
//        while (!operation.isDone)
//            yield return operation;
//    }
//}

public static class SceneExtensions
{
    /// <summary>
    /// Get the SeasonInfo component of one of the children in the given Scene
    /// </summary>
    /// <param name="scene">The Scene to search for a SeasonInfo instance</param>
    /// <returns>The SeasonInfo from one of the children in the given Scene if it exists, null otherwise</returns>
    public static SeasonInfo GetSeasonInfo(this Scene scene)
    {
        foreach (var childObject in scene.GetRootGameObjects())
        {
            var info = childObject.GetComponent<SeasonInfo>();
            if (info != null)
            {
                return info;
            }
        }
        return null;
    }

    public static SeasonCoordinateManager GetSeasonCoordinateManager(this Scene scene)
    {
        foreach (var childObject in scene.GetRootGameObjects())
        {
            var scm = childObject.GetComponent<SeasonCoordinateManager>();
            if (scm != null)
            {
                return scm;
            }
        }
        return null;
    }
}