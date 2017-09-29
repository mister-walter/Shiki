using System;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class SubtitlesPlayer : MonoBehaviour {
	void Start () {
        EventManager.AttachDelegate<ShowTextEvent>(this.OnShowTextEvent);
	}

    void OnDestroy() {
        EventManager.RemoveDelegate<ShowTextEvent>(this.OnShowTextEvent);
    }

    void OnShowTextEvent(ShowTextEvent evt) {
        throw new NotImplementedException();
    }
}
