using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.Inventory.Backend;


namespace Shiki.Inventory
{
    /// <summary>
    /// Keeps track of the player's inventory
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

        void GenerateTargets()
        {
            float totalWidth = this.numSlots * targetWidth + (this.numSlots - 1) * targetSeparation;
            float widthPerSlot = totalWidth / this.numSlots;
            float zCoord = -targetWidth / 2.0f;
            float yCoord = 0;
            for(int i = 0; i < this.numSlots; i++)
            {
                float xCoord = i * widthPerSlot;
                var target = GameObject.Instantiate(this.targetPrefab);
                target.transform.position = new Vector3(xCoord, yCoord, zCoord);
                target.transform.localScale = new Vector3(this.targetWidth, this.targetWidth, this.targetWidth);
                target.transform.parent = this.gameObject.transform;
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
    }
}
