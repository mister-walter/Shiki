/// @author Andrew Walter

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.Inventory.Backend;


namespace Shiki.Inventory
{
    /// <summary>
    /// Handles storing the player's inventory, and manages the inventory UI
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        public Camera cam;
        public uint numSlots;
        private IInventoryBackend<GameObject> inventory;
        public GameObject targetPrefab;
        private float targetWidth = 0.3f;
        private float targetSeparation = 0.15f;
        private float inventoryDistance = 0.5f;

        void Start()
        {
            InventoryManagerSingleton.SetInventoryManager(this.gameObject);
            this.inventory = new ArrayInventoryBackend(this.numSlots);
            GameEventSystem.AttachDelegate<ObjectStoredEvent>(this.OnObjectStored);
            GameEventSystem.AttachDelegate<ObjectRetrievedEvent>(this.OnObjectRetrieved);
            GameEventSystem.AttachDelegate<ToggleInventoryEvent>(this.OnInventoryToggled);
            this.GenerateTargets();
        }

        /// <summary>
        /// Instantiates as many InventoryTargets as there are inventory slots
        /// </summary>
        void GenerateTargets()
        {
            float totalWidth = this.numSlots * targetWidth + (this.numSlots - 1) * targetSeparation;
            float widthPerSlot = totalWidth / this.numSlots;
            float zCoord = -targetWidth / 2.0f;
            float yCoord = 0;
            for(uint i = 0; i < this.numSlots; i++)
            {
                float xCoord = i * widthPerSlot;
                var target = GameObject.Instantiate(this.targetPrefab);
                target.transform.position = new Vector3(xCoord, yCoord, zCoord);
                target.transform.localScale = new Vector3(this.targetWidth, this.targetWidth, this.targetWidth);
                target.transform.parent = this.gameObject.transform;
                InventoryTarget it = target.GetComponent<InventoryTarget>();
                it.index = i;
            }
        }

        void OnDestroy()
        {
            GameEventSystem.RemoveDelegate<ObjectStoredEvent>(this.OnObjectStored);
            GameEventSystem.RemoveDelegate<ObjectRetrievedEvent>(this.OnObjectRetrieved);
            GameEventSystem.RemoveDelegate<ToggleInventoryEvent>(this.OnInventoryToggled);
        }

        void OnInventoryToggled(ToggleInventoryEvent evt)
        {
            if (this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.transform.LookAt(cam.transform);
                this.gameObject.transform.position = cam.transform.position + cam.transform.forward * inventoryDistance;
                this.gameObject.SetActive(true);
            }
        }

        void OnObjectStored(ObjectStoredEvent evt)
        {
            inventory.AddToEnd(evt.storedObject);
        }

        void OnObjectRetrieved(ObjectRetrievedEvent evt)
        {
            inventory.Remove(evt.retrievedObject);
        }

        /// <summary>
        /// Get a list of the items that are currently in the inventory
        /// </summary>
        /// <returns></returns>
        public List<GameObject> GetInventoryItems()
        {
            return new List<GameObject>(this.inventory);
        }

        /// <summary>
        /// Set the inventory's contents
        /// </summary>
        /// <param name="items"></param>
        public void SetInventoryItems(List<GameObject> items)
        {
            this.inventory.SetItems(items);
        }
    }
}
