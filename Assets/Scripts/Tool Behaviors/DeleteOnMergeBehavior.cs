using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class DeleteOnMergeBehavior : MonoBehaviour {
    void Start() {
        EventManager.AttachDelegate<ObjectMergeEvent>(this.OnObjectMergeEvent);
    }

    private void OnDestroy() {
        EventManager.RemoveDelegate<ObjectMergeEvent>(this.OnObjectMergeEvent);
    }

    void OnObjectMergeEvent(ObjectMergeEvent evt) {
        if(evt.obj1 == this.gameObject || evt.obj2 == this.gameObject) {
            Destroy(this.gameObject);
        }
    }
}
