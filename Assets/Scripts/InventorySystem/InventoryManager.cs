/// @author Andrew Walter

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.EventSystem.InternalEvents;
using Shiki.Inventory.Backend;


namespace Shiki.Inventory {
    /// <summary>
    /// Handles storing the player's inventory, and manages the inventory UI
    /// </summary>
    public class InventoryManager : MonoBehaviour {
        public Camera cam;
        public uint numSlots;
        private InventoryTarget[] slots;
        private IInventoryBackend<GameObject> inventory;
        public GameObject targetPrefab;
        private float targetWidth = 0.3f;
        private float targetSeparation = 0.15f;
        private float inventoryDistance = 0.5f;

        void Start() {
            InventoryManagerSingleton.SetInventoryManager(this.gameObject);
            this.inventory = new ArrayInventoryBackend(this.numSlots);
            this.slots = new InventoryTarget[this.numSlots];
            EventManager.AttachDelegate<ObjectAcceptedByInventoryTargetEvent>(this.OnObjectStored);
            EventManager.AttachDelegate<ObjectEjectedByInventoryTargetEvent>(this.OnObjectRetrieved);
            EventManager.AttachDelegate<PlayerOpenedInventoryEvent>(this.OnInventoryToggled);
            EventManager.AttachDelegate<PlayerRecieveObjectEvent>(this.OnTaskCompletedGetObjectEvent);
            this.GenerateTargets();
            this.OnInventoryToggled(null);
        }

        /// <summary>
        /// Instantiates as many InventoryTargets as there are inventory slots
        /// </summary>
        void GenerateTargets() {
            float totalWidth = this.numSlots * targetWidth + (this.numSlots - 1) * targetSeparation;
            float widthPerSlot = totalWidth / this.numSlots;
            float zCoord = -targetWidth / 2.0f;
            float yCoord = 0;
            for(uint i = 0; i < this.numSlots; i++) {
                float xCoord = i * widthPerSlot;
                var target = GameObject.Instantiate(this.targetPrefab);
                target.transform.position = new Vector3(xCoord, yCoord, zCoord);
                target.transform.localScale = new Vector3(this.targetWidth, this.targetWidth, this.targetWidth);
                target.transform.parent = this.gameObject.transform;
                InventoryTarget it = target.GetComponent<InventoryTarget>();
                it.index = i;
                this.slots[i] = it;
            }
        }

        void OnDestroy() {
            EventManager.RemoveDelegate<ObjectAcceptedByInventoryTargetEvent>(this.OnObjectStored);
            EventManager.RemoveDelegate<ObjectEjectedByInventoryTargetEvent>(this.OnObjectRetrieved);
            EventManager.RemoveDelegate<PlayerOpenedInventoryEvent>(this.OnInventoryToggled);
            EventManager.RemoveDelegate<PlayerRecieveObjectEvent>(this.OnTaskCompletedGetObjectEvent);
        }

        void OnTaskCompletedGetObjectEvent(PlayerRecieveObjectEvent evt) {
            var obj = Shiki.Loader.LoadPrefabInstance(evt.objectToReceive);
            this.AddToInventory(obj);
        }

        void OnInventoryToggled(PlayerOpenedInventoryEvent evt) {
            if(this.gameObject.activeSelf) {
                this.gameObject.SetActive(false);
            } else {
                this.gameObject.transform.LookAt(cam.transform);
                this.gameObject.transform.position = cam.transform.position + cam.transform.forward * inventoryDistance;
                this.gameObject.SetActive(true);
            }
        }

        void OnObjectStored(ObjectAcceptedByInventoryTargetEvent evt) {
            inventory.AddToEnd(evt.acceptedObject);
            EventManager.FireEvent(new ObjectStoredEvent(evt.acceptedObject));
        }

        void OnObjectRetrieved(ObjectEjectedByInventoryTargetEvent evt) {
            Debug.Log("Object retrieved!");
            inventory.Remove(evt.ejectedObject);
            EventManager.FireEvent(new ObjectRetrievedEvent(evt.ejectedObject));
        }

        /// <summary>
        /// Get a list of the items that are currently in the inventory
        /// </summary>
        /// <returns></returns>
        public List<GameObject> GetInventoryItems() {
            return new List<GameObject>(this.inventory);
        }

        public void AddToInventory(GameObject obj) {
            for(var i = 0; i < this.slots.Length; i++) {
                if(this.slots[i].IsEmpty()) {
                    this.slots[i].SetItem(obj);
                    return;
                }
            }
            Debug.LogError("Tried to add an item to a full inventory!");
        }

        /// <summary>
        /// Set the inventory's contents
        /// </summary>
        /// <param name="items"></param>
        public void SetInventoryItems(List<GameObject> items) {
            this.inventory.SetItems(items);
        }
    }
}
