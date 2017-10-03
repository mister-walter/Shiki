using System;
using UnityEngine;
using UnityEngine.UI;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class SupertitlePlayer : MonoBehaviour {
    public Text textArea;
    public GameObject canvas;

    private GameObject child {
        get {
            if(this.gameObject.transform.childCount < 1) {
                return null;
            }
            return this.gameObject.transform.GetChild(0).gameObject;
        }
    }

	void Start() {
        EventManager.AttachDelegate<ShowTextEvent>(this.OnShowTextEvent);
	}

    void OnDestroy() {
        EventManager.RemoveDelegate<ShowTextEvent>(this.OnShowTextEvent);
    }

    void OnShowTextEvent(ShowTextEvent evt) {
        textArea.text = evt.text;
    }

    public void OnSubtitleToggleToggled(bool state) {
        this.child.SetActive(state);
    }

    public void OnSubtitlePositionChanged(float value) {
        var position = this.canvas.transform.localPosition;
        position.y = value;
        this.canvas.transform.localPosition = position;
    }
}
