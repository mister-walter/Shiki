using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.Constants;

public class LeavableItemBehavior : MonoBehaviour {
    private string currentSeason = SeasonName.None;
	void Start () {
        EventManager.AttachDelegate<PlayerEnteredAreaEvent>(this.OnPlayerEnteredAreaEvent);
        EventManager.AttachDelegate<ObjectPlacedInSeasonFinishedEvent>(this.OnObjectPlacedInSeason);
        // Hopefully this will work.
        this.currentSeason = MainSceneManager.SceneNameToSeasonName(this.gameObject.scene.name);
        Debug.Log(string.Format("LeavableItemBehavior for item {0} is in season {1}", this.gameObject.name, this.currentSeason));
    }

    private void OnDestroy() {
        EventManager.RemoveDelegate<PlayerEnteredAreaEvent>(this.OnPlayerEnteredAreaEvent);
        EventManager.RemoveDelegate<ObjectPlacedInSeasonFinishedEvent>(this.OnObjectPlacedInSeason);
    }

    void OnPlayerEnteredAreaEvent(PlayerEnteredAreaEvent evt) {
        if(this.currentSeason != SeasonName.None) {
            var distance = SeasonName.Distance(this.currentSeason, evt.seasonName);
            EventManager.FireEvent(new ItemWaitedEvent(this.gameObject, distance));
        }
    }

    void OnObjectPlacedInSeason(ObjectPlacedInSeasonFinishedEvent evt) {
        if(evt.placedObject.GetInstanceID() == this.gameObject.GetInstanceID()) {
            this.currentSeason = evt.seasonName;
        }
    }
}
