﻿/// @author Andrew Walter

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.Constants;

namespace Shiki.Inventory {
    /// <summary>
    /// MonoBehavior that allows a GameObject to be stored in the player's inventory.
    /// </summary>
    public class InventoryItemBehavior : MonoBehaviour {
        internal InventoryTarget target;
        //TODO: change this to a collection that stores insertion order
        // this is so that we can put the item in the most recently entered inventory target (which seems to be more convenient)
        internal HashSet<InventoryTarget> currentTargets;
        private GameObject inventoryManager;
        private Transform oldParent;
        private Scene oldScene;

        // Use this for initialization
        void Start() {
            this.currentTargets = new HashSet<InventoryTarget>();
            this.inventoryManager = InventoryManagerSingleton.GetInventoryManager();
            GameEventSystem.AttachDelegate<ObjectPlacedInInventoryEvent>(this.OnObjectPlacedInInventory);
            GameEventSystem.AttachDelegate<ObjectRemovedFromInventoryEvent>(this.OnObjectRemovedFromInventory);
        }

        void OnDestroy()
        {
            GameEventSystem.RemoveDelegate<ObjectPlacedInInventoryEvent>(this.OnObjectPlacedInInventory);
            GameEventSystem.RemoveDelegate<ObjectRemovedFromInventoryEvent>(this.OnObjectRemovedFromInventory);
        }

        void OnObjectPlacedInInventory(ObjectPlacedInInventoryEvent evt)
        {
            if (evt.placedObject.GetInstanceID() == this.gameObject.GetInstanceID())
            {
                this.target = this.currentTargets.GetOne();
                var rigidBody = this.gameObject.GetComponent<Rigidbody>();
                this.gameObject.transform.rotation = Quaternion.identity;
                this.gameObject.transform.position = this.target.transform.position;
                rigidBody.velocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;
                rigidBody.useGravity = false;
                this.oldScene = this.gameObject.scene;
                this.gameObject.transform.parent = null;
                SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName(SceneName.Main));
                this.oldParent = this.gameObject.transform.parent;
                //Debug.Log(string.Format("{0}", this.oldParent));
                this.gameObject.transform.SetParent(this.inventoryManager.transform);
            }
        }

        void OnObjectRemovedFromInventory(ObjectRemovedFromInventoryEvent evt)
        {
            if (evt.placedObject.GetInstanceID() == this.gameObject.GetInstanceID())
            {
                var rigidBody = this.gameObject.GetComponent<Rigidbody>();
                rigidBody.useGravity = true;
                this.gameObject.transform.SetParent(this.oldParent);
                SceneManager.MoveGameObjectToScene(this.gameObject, this.oldScene);
            }
        }

        internal void OnEnterInventoryTarget(InventoryTarget target)
        {
            if(target.IsEmpty())
                this.currentTargets.Add(target);
        }

        internal void OnExitInventoryTarget(InventoryTarget target)
        {
            // We don't need to check if the target was a valid one for us, (i.e. empty when we entered)
            // because Remove will just do nothing if target isn't inside currentTargets
            this.currentTargets.Remove(target);
        }

        /// <summary>
        /// Determines whether or not the item is currently inside of an InventoryTarget
        /// </summary>
        /// <returns>True if the item is currently inside of an InventoryTarget, false otherwise</returns>
        public bool IsInsideTarget()
        {
            return this.currentTargets.Count > 0;
        }
    }
}

public static class HashSetExtensions
{
    public static T GetOne<T>(this HashSet<T> set)
    {
        var enumerator = set.GetEnumerator();
        enumerator.MoveNext();
        return enumerator.Current;
    }
}