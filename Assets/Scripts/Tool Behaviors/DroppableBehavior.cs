using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class DroppableBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.gameObject.GetComponent<DropTargetBehavior>() != null) {
            EventManager.FireEvent(new ObjectDroppedOntoDropTargetEvent(this.gameObject, collision.collider.gameObject));
        }
    }
}
