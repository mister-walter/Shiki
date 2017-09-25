using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class DeleteOnMergeBehavior : MonoBehaviour {

    // Use this for initialization
    void Start() {
        EventManager.AttachDelegate<ObjectMergeEvent>(this.OnObjectMergeEvent);
    }

    private void OnDestroy() {
        EventManager.RemoveDelegate<ObjectMergeEvent>(this.OnObjectMergeEvent);
    }

    // Update is called once per frame
    void Update() {

    }

    void OnObjectMergeEvent(ObjectMergeEvent evt) {
        if(evt.obj1 == this.gameObject || evt.obj2 == this.gameObject) {
            Destroy(this.gameObject);
        }
    }
}
