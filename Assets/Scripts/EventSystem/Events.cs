/// @author Andrew Walter

using UnityEngine;

/// <summary>
/// Events for use with GameEventSystem
/// </summary>
namespace Shiki.EventSystem.Events
{
    /// <summary>
    /// Fired by: GrabObjects
    /// Fires: When an object is placed
    /// Listened for by: SeasonInfo, which then fires ObjectPlacedInSeasonEvent if the object was placed in its season
    /// </summary>
    public class ObjectPutDownEvent : IGameEvent
    {
        /// <summary>
        /// The GameObject that was placed.
        /// </summary>
        public GameObject placedObject;

        /// <summary>
        /// The SeasonalEffect from the season in which the object was placed. Could be null.
        /// </summary>
        public SeasonalEffect effect = null;

        public ObjectPutDownEvent(GameObject go)
        {
            this.placedObject = go;
        }
        public ObjectPutDownEvent(GameObject go, SeasonalEffect effect)
        {
            this.placedObject = go;
            this.effect = effect;
        }
    }

    /// <summary>
    /// Fired by: SeasonInfo
    /// Fires: When an object is placed into a season (after ObjectPutDownEvent)
    /// Listened for by: SeasonalEffect
    /// </summary>
    public class ObjectPlacedInSeasonEvent : IGameEvent
    {
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

        /// <summary>
        /// The SeasonalEffect from the season in which the object was placed.
        /// </summary>
        public SeasonalEffect effect;

        /// <summary>
        /// The name of the season which the object came from, if any. Could be null.
        /// </summary>
        public string previousSeason;

        public ObjectPlacedInSeasonEvent(GameObject go, SeasonalEffect effect, SeasonCoordinate coord, string seasonName)
        {
            this.placedObject = go;
            this.effect = effect;
            this.coord = coord;
            this.seasonName = seasonName;
        }

        public ObjectPlacedInSeasonEvent(GameObject go, SeasonalEffect effect, SeasonCoordinate coord, string seasonName, string previousSeason)
        {
            this.placedObject = go;
            this.effect = effect;
            this.coord = coord;
            this.seasonName = seasonName;
            this.previousSeason = previousSeason;
        }
    }

    /// <summary>
    /// Fired by: GrabObjects
    /// Fires: When an object is picked up
    /// Listened for by: SeasonalEffect
    /// </summary>
    public class ObjectPickedUpEvent : IGameEvent
    {
        /// <summary>
        /// The GameObject that was picked up.
        /// </summary>
        public GameObject pickedUpObject;

        /// <summary>
        /// The SeasonalEffect from the season in which the object was picked up. Could be null.
        /// </summary>
        public SeasonalEffect effect = null;

        public ObjectPickedUpEvent(GameObject go)
        {
            this.pickedUpObject = go;
        }

        public ObjectPickedUpEvent(GameObject go, SeasonalEffect effect)
        {
            this.pickedUpObject = go;
            this.effect = effect;
        }
    }

    /// <summary>
    /// Fired by: SeasonalEffect
    /// Fires: When an object is placed for the first time (i.e. has no other variants)
    /// Listened for by: SeasonInfo
    /// </summary>
    public class SeasonalObjectPlacedForFirstTime : IGameEvent
    {
        /// <summary>
        /// The GameObject that was placed.
        /// </summary>
        public GameObject placedObject;

        /// <summary>
        /// The SeasonalEffect of the season in which the object was placed.
        /// </summary>
        public SeasonalEffect effect;

        /// <summary>
        /// The name of the season in which the object was placed.
        /// </summary>
        public string placedInSeason;

        /// <summary>
        /// The SeasonCoordinate at which the object was placed.
        /// </summary>
        public SeasonCoordinate placedAtCoord;

        public SeasonalObjectPlacedForFirstTime(GameObject go, SeasonalEffect effect, string season, SeasonCoordinate placedAtCoord)
        {
            this.placedObject = go;
            this.placedInSeason = season;
            this.placedAtCoord = placedAtCoord;
            this.effect = effect;
        }
    }

    /// <summary>
    /// Fired by: TODO
    /// Fires: TODO
    /// Listened for by: InventoryManager
    /// </summary>
    public class ObjectStoredEvent : IGameEvent
    {
        /// <summary>
        /// The object that was stored.
        /// </summary>
        public GameObject storedObject;

        public ObjectStoredEvent(GameObject go)
        {
            this.storedObject = go;
        }
    }

    public class ObjectRetrievedEvent : IGameEvent
    {
        public GameObject retrievedObject;
    }

    public class ObjectPlacedInInventoryEvent : IGameEvent
    {
        public GameObject placedObject;
        public ObjectPlacedInInventoryEvent(GameObject go)
        {
            this.placedObject = go;
        }
    }

    public class ObjectRemovedFromInventoryEvent : IGameEvent
    {
        public GameObject placedObject;
        public ObjectRemovedFromInventoryEvent(GameObject go)
        {
            this.placedObject = go;
        }
    }

    public class ToggleInventoryEvent : IGameEvent { }
}

