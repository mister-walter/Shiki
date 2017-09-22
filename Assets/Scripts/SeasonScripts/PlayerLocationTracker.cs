using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.Constants;
using UnityEngine.SceneManagement;

public class PlayerLocationTracker : MonoBehaviour {
    private SeasonInfo[] seasonInfos;
    private string currentSeason = SeasonName.None;
    private bool allScenesLoaded = false;


    // Use this for initialization
    void Start() {
        EventManager.AttachDelegate<PlayerEnteredAreaEvent>(this.OnPlayerEnteredAreaEvent);
        EventManager.AttachDelegate<AllScenesLoadedEvent>(this.OnAllScenesLoaded);
	}

    private void OnDestroy() {
        EventManager.RemoveDelegate<PlayerEnteredAreaEvent>(this.OnPlayerEnteredAreaEvent);
        EventManager.RemoveDelegate<AllScenesLoadedEvent>(this.OnAllScenesLoaded);
    }

    // Update is called once per frame
    void Update () {
        if(allScenesLoaded) {
            if(this.seasonInfos == null) {
                this.seasonInfos = new SeasonInfo[] {
                    SceneManager.GetSceneByName(SceneName.Winter).FindInSceneShallow<SeasonInfo>(),
                    SceneManager.GetSceneByName(SceneName.Spring).FindInSceneShallow<SeasonInfo>(),
                    SceneManager.GetSceneByName(SceneName.Summer).FindInSceneShallow<SeasonInfo>(),
                    SceneManager.GetSceneByName(SceneName.Fall).FindInSceneShallow<SeasonInfo>()
                };
            }
            foreach(SeasonInfo si in this.seasonInfos) {
                if(si.IsPositionInSeason(this.gameObject.transform.position)) {
                    if(si.seasonName != this.currentSeason) {
                        this.currentSeason = si.seasonName;
                        EventManager.FireEvent(new PlayerEnteredAreaEvent(this.currentSeason));
                        return;
                    }
                }
            }
        }
	}

    void OnPlayerEnteredAreaEvent(PlayerEnteredAreaEvent evt) {
        Debug.Log(string.Format("Player entered area {0}", evt.seasonName));
    }

    void OnAllScenesLoaded(AllScenesLoadedEvent evt) {
        this.allScenesLoaded = true;
    }
}
