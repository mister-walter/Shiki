using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideChildBehavior : MonoBehaviour {

    private GameObject child {
        get {
            if(this.gameObject.transform.childCount < 1) {
                return null;
            }
            return this.gameObject.transform.GetChild(0).gameObject;
        }
    }

    // Use this for initialization
    void Start() {
        EventManager.AttachDelegate<ShowObjectEvent>(this.OnShowObjectEvent);
        EventManager.AttachDelegate<HideObjectEvent>(this.OnHideObjectEvent);
    }

    private void OnDestroy() {
        EventManager.RemoveDelegate<ShowObjectEvent>(this.OnShowObjectEvent);
        EventManager.RemoveDelegate<HideObjectEvent>(this.OnHideObjectEvent);
    }

    void OnShowObjectEvent(ShowObjectEvent evt) {
        if(this.child != null && this.child.name == evt.toShow) {
            this.child.SetActive(true);
        }
    }

    void OnHideObjectEvent(HideObjectEvent evt) {
        if(this.child != null && this.child.name == evt.toHide) {
            this.child.SetActive(false);
        }
    }
}
