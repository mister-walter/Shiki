using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class MergeableMasterBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("On collision enter");
        if (collision.rigidbody && collision.rigidbody.gameObject.GetComponent<MergeableSlaveBehavior>() != null) {
            EventManager.FireEvent(new ObjectMergeEvent(this.gameObject, collision.rigidbody.gameObject));
        }
    }
}
