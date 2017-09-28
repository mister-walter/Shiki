using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class DroppableBehavior : MonoBehaviour {
    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Dropped");
        if(collision.collider.gameObject.GetComponent<DropTargetBehavior>() != null) {
            Debug.Log("Dropped on something");
            EventManager.FireEvent(new ObjectDroppedOntoDropTargetEvent(this.gameObject, collision.collider.gameObject));
        }
    }
}
