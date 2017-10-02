using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.Constants;
using UnityEngine.SceneManagement;
using Shiki.EventSystem.InternalEvents;

/// <summary>
/// Tracks the player's movement, firing an event when they move into a different season.
/// Probably would be more efficient to place colliders on all of the boundaries between seasons,
/// but this way is a little cleaner and more robust.
/// </summary>
public class PlayerLocationTracker : MonoBehaviour {
    // Backing variable, with lazy initializer in the getter
    private SeasonInfo[] _seasonInfos;
    private SeasonInfo[] seasonInfos {
        get {
            if(_seasonInfos == null) {
                _seasonInfos = new SeasonInfo[] {
                    SceneManager.GetSceneByName(SceneName.Winter).FindInSceneShallow<SeasonInfo>(),
                    SceneManager.GetSceneByName(SceneName.Spring).FindInSceneShallow<SeasonInfo>(),
                    SceneManager.GetSceneByName(SceneName.Summer).FindInSceneShallow<SeasonInfo>(),
                    SceneManager.GetSceneByName(SceneName.Fall).FindInSceneShallow<SeasonInfo>()
                };
            }
            return _seasonInfos;
        }
    }

    private string currentSeason = SeasonName.None;
    private bool allScenesLoaded = false;

    void Start() {
        EventManager.AttachDelegate<PlayerEnteredAreaEvent>(this.OnPlayerEnteredAreaEvent);
        EventManager.AttachDelegate<AllScenesLoadedEvent>(this.OnAllScenesLoadedEvent);
	}

    private void OnDestroy() {
        EventManager.RemoveDelegate<PlayerEnteredAreaEvent>(this.OnPlayerEnteredAreaEvent);
        EventManager.RemoveDelegate<AllScenesLoadedEvent>(this.OnAllScenesLoadedEvent);
    }

    void Update() {
        if(allScenesLoaded) {
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

    void OnAllScenesLoadedEvent(AllScenesLoadedEvent evt) {
        this.allScenesLoaded = true;
    }
}
