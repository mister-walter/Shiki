using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class DroppableBehavior : MonoBehaviour {
    private bool dropped;
    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Dropped");
        if(collision.collider.gameObject.GetComponent<DropTargetBehavior>() != null) {
            Debug.Log("Dropped on something");
            EventManager.FireEvent(new ObjectDroppedOntoDropTargetEvent(this.gameObject, collision.collider.gameObject));
        }
    }

    private void OnCollisionStay(Collision collision) {
        if(!this.dropped) {
            if(collision.collider.gameObject.GetComponent<DropTargetBehavior>() != null) {
                Debug.Log("Dropped on something");
                EventManager.FireEvent(new ObjectDroppedOntoDropTargetEvent(this.gameObject, collision.collider.gameObject));
                this.dropped = true;
            }
        }
    }
}
