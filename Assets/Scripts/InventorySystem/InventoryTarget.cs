using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTarget : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exit for inventory target: other gameobject {}", other.gameObject);
        var inventoryItemBehavior = other.gameObject.GetComponent<InventoryItemBehavior>();
        if (inventoryItemBehavior != null)
        {
            inventoryItemBehavior.OnExitInventoryTarget(this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter for inventory target: other gameobject {}", other.gameObject);
        var inventoryItemBehavior = other.gameObject.GetComponent<InventoryItemBehavior>();
        if(inventoryItemBehavior != null)
        {
            inventoryItemBehavior.OnEnterInventoryTarget(this);
        }
    }
}