using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shiki.Inventory
{
    public class InventoryTarget : MonoBehaviour
    {
        private GameObject storedObject = null;

        void OnTriggerExit(Collider other)
        {
            var inventoryItemBehavior = other.gameObject.GetComponent<InventoryItemBehavior>();
            if (inventoryItemBehavior != null)
            {
                inventoryItemBehavior.OnExitInventoryTarget(this);
                this.storedObject = null;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var inventoryItemBehavior = other.gameObject.GetComponent<InventoryItemBehavior>();
            if (inventoryItemBehavior != null)
            {
                inventoryItemBehavior.OnEnterInventoryTarget(this);
                this.storedObject = other.gameObject;
            }
        }
    }
}