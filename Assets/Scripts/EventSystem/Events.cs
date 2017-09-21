/// @author Andrew Walter

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
    public class TaskCompletedGetObjectEvent : IGameEvent {
        /// <summary>
        /// Object the player receives
        /// </summary>
        public string objectToReceive { get; set; }

        public TaskCompletedGetObjectEvent(string otr) {
            objectToReceive = otr;
        }
    }

    /// <summary>
    /// On complete change event. Gets fired when a task's oncomplete function is called
    /// Results in one item transforming into another.
    /// </summary>
    public class TaskCompletedChangeEvent : IGameEvent {

        /// <summary>
        /// Object to be changed
        /// </summary>
        public string origObject { get; set; }

        /// <summary>
        /// New object (what the original object turns into)
        /// </summary>
        public string objectChangedTo { get; set; }


        public TaskCompletedChangeEvent(string oo, string oct) {
            origObject = oo;
            objectChangedTo = oct;
        }
    }

    /// <summary>
    /// UI action kind.
    /// Requests that the UI perform one of these actions.
    /// </summary>
    public enum UIActionKind {
        Sound, Dialog, None
    };

    public class TaskCompletedUIEvent : IGameEvent {
        /// <summary>
        /// What the UI should do
        /// </summary>
        public UIActionKind uiEventKind { get; set; }

        /// <summary>
        /// Name of the action to be played
        /// </summary>
        public string name { get; set; }

        public TaskCompletedUIEvent(UIActionKind uiek, string n) {
            uiEventKind = uiek;
            name = n;
        }
    }

    public class ObjectHitEvent : IGameEvent {
        public GameObject hitObject;

        public ObjectHitEvent(GameObject go) {
            this.hitObject = go;
        }
    }
}

