using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.InternalEvents;

public class HideOnPrologueDone : MonoBehaviour {
	void Start () {
        EventManager.AttachDelegate<PrologueDoneEvent>(this.OnPrologueDoneEvent);
	}

    private void OnDestroy() {
        EventManager.RemoveDelegate<PrologueDoneEvent>(this.OnPrologueDoneEvent);
    }

    void OnPrologueDoneEvent(PrologueDoneEvent evt) {
        this.gameObject.SetActive(false);
    }
}
