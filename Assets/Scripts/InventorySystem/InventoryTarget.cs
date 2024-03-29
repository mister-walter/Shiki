﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shiki.Inventory {
    public class InventoryTarget : MonoBehaviour {
        internal uint index;
        private GameObject storedObject = null;

        void OnTriggerExit(Collider other) {
            var inventoryItemBehavior = other.gameObject.GetComponent<InventoryItemBehavior>();
            if(inventoryItemBehavior != null) {
                inventoryItemBehavior.OnExitInventoryTarget(this);
                this.storedObject = null;
            }
        }

        void OnTriggerEnter(Collider other) {
            var inventoryItemBehavior = other.gameObject.GetComponent<InventoryItemBehavior>();
            if(inventoryItemBehavior != null) {
                inventoryItemBehavior.OnEnterInventoryTarget(this);
                this.storedObject = other.gameObject;
            }
        }

        /// <summary>
        /// Determines whether this InventoryTarget contains an item or not
        /// </summary>
        /// <returns>True if this InventoryTarget does not contain an item, false otherwise</returns>
        public bool IsEmpty() {
            return this.storedObject == null;
        }

        /// <summary>
        /// Set this InventoryTarget's item to the given GameObject, correctly positioning it inside the target.
        /// </summary>
        /// <param name="go">The GameObject to set as this InventoryTarget's item</param>
        public void SetItem(GameObject go) {
            this.storedObject = go;
            var inventoryItemBehavior = go.GetComponent<InventoryItemBehavior>();
            inventoryItemBehavior.PositionObjectInsideTarget(this);
        }
    }
}