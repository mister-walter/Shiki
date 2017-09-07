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
        private IInventoryBackend<GameObject> inventory;

        void Start()
        {
            this.inventory = new ArrayInventoryBackend(1);
            GameEventSystem.AttachDelegate<ObjectStoredEvent>(this.OnObjectStored);
            GameEventSystem.AttachDelegate<ObjectRetrievedEvent>(this.OnObjectRetrieved);
            GameEventSystem.AttachDelegate<ToggleInventoryEvent>(this.OnInventoryToggled);
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
                this.gameObject.transform.position = cam.transform.position + cam.transform.forward * 2;
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
