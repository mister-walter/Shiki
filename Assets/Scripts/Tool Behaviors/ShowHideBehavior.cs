using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class ShowHideBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.AttachDelegate<ShowObjectEvent>(this.OnShowObjectEvent);
        EventManager.AttachDelegate<HideObjectEvent>(this.OnHideObjectEvent);
	}

    private void OnDestroy() {
        EventManager.RemoveDelegate<ShowObjectEvent>(this.OnShowObjectEvent);
        EventManager.RemoveDelegate<HideObjectEvent>(this.OnHideObjectEvent);
    }

    void OnShowObjectEvent(ShowObjectEvent evt) {
        Debug.Log("Got show obj evt " + evt.toShow);
        if(this.gameObject.name == evt.toShow) {
            var renderer = this.gameObject.GetComponent<Renderer>();
            renderer.enabled = true;
            //this.gameObject.SetActive(true);
        }
    }

    void OnHideObjectEvent(HideObjectEvent evt) {
        if(this.gameObject.name == evt.toHide) {
            this.gameObject.SetActive(false);
        }
    }

}
