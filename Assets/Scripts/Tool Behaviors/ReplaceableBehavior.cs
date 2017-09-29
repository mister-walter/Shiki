using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class ReplaceableBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.AttachDelegate<ReplaceObjectEvent>(this.OnReplaceObjectEvent);
	}

    void OnDestroy() {
        EventManager.RemoveDelegate<ReplaceObjectEvent>(this.OnReplaceObjectEvent);
    }

    void OnReplaceObjectEvent(ReplaceObjectEvent evt) {
        if(evt.originalObject == this.gameObject.name ||
            (evt.exactOriginalObject != null && 
                evt.exactOriginalObject.GetInstanceID() == this.gameObject.GetInstanceID())) {
            var newObject = Shiki.Loader.LoadPrefabInstance(evt.changeToObject);
            SceneManager.MoveGameObjectToScene(newObject, this.gameObject.scene);
            newObject.transform.position = this.gameObject.transform.position;
            newObject.transform.rotation = this.gameObject.transform.rotation;
            Destroy(this.gameObject);
        }
    }
}
