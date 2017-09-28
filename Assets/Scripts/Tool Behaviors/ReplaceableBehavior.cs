﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class ReplaceableBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.AttachDelegate<TaskCompletedChangeEvent>(this.OnTaskCompletedChangeEvent);
	}

    void OnDestroy() {
        EventManager.RemoveDelegate<TaskCompletedChangeEvent>(this.OnTaskCompletedChangeEvent);
    }

    void OnTaskCompletedChangeEvent(TaskCompletedChangeEvent evt) {
        Debug.Log("Task completed change event recieved");
        if(evt.origObject == this.gameObject.name) {
            var newObject = Shiki.Loader.LoadPrefabInstance(evt.objectChangedTo);
            SceneManager.MoveGameObjectToScene(newObject, this.gameObject.scene);
            newObject.transform.position = this.gameObject.transform.position;
            newObject.transform.rotation = this.gameObject.transform.rotation;
            Destroy(this.gameObject);
        }
    }
}