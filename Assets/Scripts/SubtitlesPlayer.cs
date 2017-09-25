using System;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class SubtitlesPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.AttachDelegate<ShowTextEvent>(this.OnShowTextEvent);
	}

    void OnDestroy() {
        EventManager.RemoveDelegate<ShowTextEvent>(this.OnShowTextEvent);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnShowTextEvent(ShowTextEvent evt) {
        throw new NotImplementedException();
    }
}
