using System;
using UnityEngine;
using UnityEngine.UI;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class SupertitlePlayer : MonoBehaviour {
    public Text textArea;

	void Start() {
        EventManager.AttachDelegate<ShowTextEvent>(this.OnShowTextEvent);
	}

    void OnDestroy() {
        EventManager.RemoveDelegate<ShowTextEvent>(this.OnShowTextEvent);
    }

    void OnShowTextEvent(ShowTextEvent evt) {
        textArea.text = evt.text;
    }
}
