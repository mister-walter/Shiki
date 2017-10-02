using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.InternalEvents;
using Shiki.Constants;
using UnityEngine.SceneManagement;

public static class GameObjectLocatorSingleton {
    private static GameObjectLocator singleton;
    
    public static GameObjectLocator GetSingleton() {
        return singleton;
    }

    public static void SetSingleton(GameObjectLocator instance) {
        singleton = instance;
    }
}

public class GameObjectLocator : MonoBehaviour {
    private bool loaded = false;
    private List<SeasonInfo> seasonInfos;
	// Use this for initialization
	void Start () {
        GameObjectLocatorSingleton.SetSingleton(this);
        EventManager.AttachDelegate<AllScenesLoadedEvent>(this.OnAllScenesLoaded);
	}

    void OnAllScenesLoaded(AllScenesLoadedEvent evt) {
        this.seasonInfos = new List<SeasonInfo>();
        foreach(var seasonName in SeasonName.AllSeasons) {
            var sceneName = MainSceneManager.SeasonNameToSceneName(seasonName);
            var scene = SceneManager.GetSceneByName(sceneName);
            var seasonInfo = scene.FindInSceneDeep<SeasonInfo>();
            this.seasonInfos.Add(seasonInfo);
        }
        loaded = true;
    }

    public Scene? GetSceneForGameObject(GameObject go) {
        if(this.loaded) {
            foreach(var seasonInfo in this.seasonInfos) {
                var status = seasonInfo.CheckPosition(go.transform.position);
                switch(status) {
                    case IsInSeasonStatus.InSeason:
                        return seasonInfo.gameObject.scene;
                    case IsInSeasonStatus.InVillage:
                        return SceneManager.GetSceneByName(SceneName.Village);
                    default:
                        break;
                }
            }
        }
        return null;
    }
}
