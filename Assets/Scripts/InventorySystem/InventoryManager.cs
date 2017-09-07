using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;


namespace Shiki.Inventory
{
    /// <summary>
    /// Keeps track of the player's inventory
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        private IInventoryBackend<GameObject> inventory;

        void Start()
        {
            GameEventSystem.AttachDelegate<ObjectStoredEvent>(this.OnObjectStored);
            GameEventSystem.AttachDelegate<ObjectRetrievedEvent>(this.OnObjectRetrieved);
        }

        void OnDestroy()
        {
            GameEventSystem.RemoveDelegate<ObjectStoredEvent>(this.OnObjectStored);
            GameEventSystem.RemoveDelegate<ObjectRetrievedEvent>(this.OnObjectRetrieved);
        }

        void OnObjectStored(ObjectStoredEvent evt)
        {
            Debug.Log("Recieved ObjectStoredEvent for object {}", evt.storedObject);
            inventory.AddToEnd(evt.storedObject);
        }

        void OnObjectRetrieved(ObjectRetrievedEvent evt)
        {
            Debug.Log("Recieved ObjectRetrievedEvent for object {}", evt.retrievedObject);
            inventory.Remove(evt.retrievedObject);
        }
    }
}
