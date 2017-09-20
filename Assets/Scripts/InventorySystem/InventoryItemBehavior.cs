using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.Constants;

namespace Shiki.Inventory {
    public class InventoryItemBehavior : MonoBehaviour {

        internal bool isInInventoryTarget = false;
        internal InventoryTarget currentTarget;
        internal InventoryTarget target;
        private GameObject inventoryManager;
        private Transform oldParent;
        private Scene oldScene;

        // Use this for initialization
        void Start() {
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
            if (evt.placedObject == this.gameObject)
            {
                this.target = this.currentTarget;
                var rigidBody = this.gameObject.GetComponent<Rigidbody>();
                this.gameObject.transform.rotation = Quaternion.identity;
                rigidBody.velocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;
                rigidBody.useGravity = false;
                this.oldScene = this.gameObject.scene;
                SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName(SceneName.Main));
                this.oldParent = this.gameObject.transform.parent;
                this.gameObject.transform.SetParent(this.inventoryManager.transform);
            }
        }

        void OnObjectRemovedFromInventory(ObjectRemovedFromInventoryEvent evt)
        {
            if (evt.placedObject == this.gameObject)
            {
                this.gameObject.transform.SetParent(this.oldParent);
                SceneManager.MoveGameObjectToScene(this.gameObject, this.oldScene);
            }
        }

        internal void OnEnterInventoryTarget(InventoryTarget target)
        {
            this.isInInventoryTarget = true;
            this.currentTarget = target;
        }

        internal void OnExitInventoryTarget(InventoryTarget target)
        {
            this.isInInventoryTarget = false;
            this.currentTarget = null;
        }
    }
}