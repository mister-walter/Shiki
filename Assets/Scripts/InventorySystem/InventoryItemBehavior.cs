using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class InventoryItemBehavior : MonoBehaviour {

    internal bool isInInventoryTarget = false;

	// Use this for initialization
	void Start () {
        GameEventSystem.AttachDelegate<ObjectPlacedInInventoryEvent>(this.OnObjectPlacedInInventory);
	}

    void OnDestroy()
    {
        GameEventSystem.RemoveDelegate<ObjectPlacedInInventoryEvent>(this.OnObjectPlacedInInventory);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnObjectPlacedInInventory(ObjectPlacedInInventoryEvent evt)
    {
        if(evt.placedObject == this.gameObject)
        {
            var rigidBody = this.gameObject.GetComponent<Rigidbody>();
            rigidBody.useGravity = false;
            //rigidBody.isKinematic = false;
        }
    }

    internal void OnEnterInventoryTarget(InventoryTarget target)
    {
        this.isInInventoryTarget = true;
    }

    internal void OnExitInventoryTarget(InventoryTarget target)
    {
        this.isInInventoryTarget = false;
    }
}
