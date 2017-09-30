using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class ReplaceableBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.AttachDelegate<ObjectReplaceEvent>(this.OnTaskCompletedChangeEvent);
	}

    void OnDestroy() {
        EventManager.RemoveDelegate<ObjectReplaceEvent>(this.OnTaskCompletedChangeEvent);
    }

    void OnTaskCompletedChangeEvent(ObjectReplaceEvent evt) {
        Debug.Log("Task completed change event recieved");
        if(evt.originalObject == this.gameObject.name) {
            var newObject = Shiki.Loader.LoadPrefabInstance(evt.objectToChangeTo);
            SceneManager.MoveGameObjectToScene(newObject, this.gameObject.scene);
            newObject.transform.position = this.gameObject.transform.position;
            newObject.transform.rotation = this.gameObject.transform.rotation;
            Destroy(this.gameObject);
        }
    }
}
