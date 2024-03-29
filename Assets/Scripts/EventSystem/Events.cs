﻿/// @author Andrew Walter

using UnityEngine;

/// <summary>
/// Frontend events for use with EventManager
/// </summary>
namespace Shiki.EventSystem.Events {
    /// <summary>
    /// Fired by: SeasonalEffect
    /// Fires: After an object is placed into a season (after internal ObjectPlacedInSeasonStartEvent)
    /// Listened for by: ?
    /// </summary>
    public class ObjectPlacedInSeasonFinishedEvent : IGameEvent {
        /// <summary>
        /// The GameObject that was placed
        /// </summary>
        public GameObject placedObject;

        /// <summary>
        /// The SeasonCoordinate where the object was placed
        /// </summary>
        public SeasonCoordinate coord;

        /// <summary>
        /// The name of the season in which the object was placed
        /// </summary>
        public string seasonName;

        public ObjectPlacedInSeasonFinishedEvent(GameObject go, SeasonCoordinate coord, string seasonName) {
            this.placedObject = go;
            this.coord = coord;
            this.seasonName = seasonName;
        }
    }

    /// <summary>
    /// Fired by: GrabObjects
    /// Fires: When an object is picked up
    /// Listened for by: SeasonalEffect
    /// </summary>
    public class ObjectPickedUpEvent : IGameEvent {
        /// <summary>
        /// The GameObject that was picked up.
        /// </summary>
        public GameObject pickedUpObject;

        /// <summary>
        /// The SeasonalEffect from the season in which the object was picked up. Could be null.
        /// </summary>
        public SeasonalEffect effect = null;

        public ObjectPickedUpEvent(GameObject go) {
            this.pickedUpObject = go;
        }

        public ObjectPickedUpEvent(GameObject go, SeasonalEffect effect) {
            this.pickedUpObject = go;
            this.effect = effect;
        }
    }

    /// <summary>
    /// Fired by: InventoryManager
    /// Fires: After an object has been stored in the inventory
    /// Listened for by: Task triggers
    /// </summary>
    public class ObjectStoredEvent : IGameEvent {
        /// <summary>
        /// The object that was stored.
        /// </summary>
        public GameObject storedObject;

        public ObjectStoredEvent(GameObject go) {
            this.storedObject = go;
        }
    }

    /// <summary>
    /// Fired by: InventoryManager
    /// Fires: After an object has been retrieved from the inventory
    /// Listened for by: Task triggers
    /// </summary>
    public class ObjectRetrievedEvent : IGameEvent {
        public GameObject retrievedObject;

        public ObjectRetrievedEvent(GameObject go) {
            this.retrievedObject = go;
        }
    }

    public class PlayerOpenedInventoryEvent : IGameEvent { }

    public class PlayerEnteredAreaEvent : IGameEvent {
        public string seasonName;

        public PlayerEnteredAreaEvent(string seasonName) {
            this.seasonName = seasonName;
        }
    }

    /// <summary>
    /// On complete get event. Gets fired when a task's oncomplete function is called.
    /// Results in the player receiving an item.
    /// </summary>
    public class PlayerRecieveObjectEvent : IGameEvent {
        /// <summary>
        /// Object the player receives
        /// </summary>
        public string objectToReceive { get; set; }

        public PlayerRecieveObjectEvent(string otr) {
            objectToReceive = otr;
        }
    }

    /// <summary>
    /// Gets fired when a task's oncomplete function is called
    /// Results in one item transforming into another.
    /// </summary>
    public class ReplaceObjectEvent : IGameEvent {
        /// <summary>
        /// Object to be changed
        /// </summary>
        public string originalObject { get; set; }

        public GameObject exactOriginalObject { get; set; }

        /// <summary>
        /// New object (what the original object turns into)
        /// </summary>
        public string objectToChangeTo { get; set; }

        public ReplaceObjectEvent(string originalObject, string objectToChangeTo) {
            this.originalObject = originalObject;
            this.objectToChangeTo = objectToChangeTo;
        }
        public ReplaceObjectEvent(GameObject exactOriginalObject, string objectToChangeTo) {
            this.exactOriginalObject = exactOriginalObject;
            this.objectToChangeTo = objectToChangeTo;
        }

    }

    public class ObjectHitEvent : IGameEvent {
        public GameObject hitObject;
        public GameObject tool;

        public ObjectHitEvent(GameObject go, GameObject tool) {
            this.hitObject = go;
            this.tool = tool;
        }
    }

    public class ObjectDroppedOntoDropTargetEvent : IGameEvent {
        public GameObject droppedObject;
        public GameObject dropTarget;

        public ObjectDroppedOntoDropTargetEvent(GameObject droppedObject, GameObject dropTarget) {
            this.droppedObject = droppedObject;
            this.dropTarget = dropTarget;
        }
    }

    public class PlaySoundEvent : IGameEvent {
        public string soundName;
        public PlaySoundEvent(string soundName) {
            this.soundName = soundName;
        }
    }

    public class ShowTextEvent : IGameEvent {
        public string text;
        public ShowTextEvent(string text) {
            this.text = text;
        }
    }

    public class ObjectMergeEvent : IGameEvent {
        public GameObject obj1;
        public GameObject obj2;

        public ObjectMergeEvent(GameObject obj1, GameObject obj2) {
            this.obj1 = obj1;
            this.obj2 = obj2;
        }
    }

    public class DeleteObjectEvent : IGameEvent {
        public string toDelete;
        public DeleteObjectEvent(string toDelete) {
            this.toDelete = toDelete;
        }
    }

    public class HideObjectEvent : IGameEvent {
        public string toHide;
        public HideObjectEvent(string toHide) {
            this.toHide = toHide;
        }
    }

    public class ShowObjectEvent : IGameEvent {
        public string toShow;
        public ShowObjectEvent(string toShow) {
            this.toShow = toShow;
        }
    }

    public class ItemWaitedEvent : IGameEvent {
        public GameObject item;
        public uint seasonsWaited;

        public ItemWaitedEvent(GameObject item, uint distance) {
            this.item = item;
            this.seasonsWaited = distance;
        }
    }
}

