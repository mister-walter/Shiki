using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shiki.EventSystem.InternalEvents {
    /// <summary>
    /// Fired by: GrabObjects
    /// Fires: When an object is placed
    /// Listened for by: SeasonInfo, which then fires ObjectPlacedInSeasonStartEvent if the object was placed in its season
    /// </summary>
    public class ObjectPlacedEvent : IGameEvent {
        /// <summary>
        /// The GameObject that was placed.
        /// </summary>
        public GameObject placedObject;

        /// <summary>
        /// The SeasonalEffect from the season in which the object was placed. Could be null.
        /// </summary>
        public SeasonalEffect effect = null;

        public ObjectPlacedEvent(GameObject go) {
            this.placedObject = go;
        }
        public ObjectPlacedEvent(GameObject go, SeasonalEffect effect) {
            this.placedObject = go;
            this.effect = effect;
        }
    }

    /// <summary>
    /// Fired by: SeasonInfo
    /// Fires: When an object is placed into a season (after ObjectPlacedEvent)
    /// Listened for by: SeasonalEffect
    /// </summary>
    public class ObjectPlacedInSeasonStartEvent : IGameEvent {
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

        public ObjectPlacedInSeasonStartEvent(GameObject go, SeasonalEffect effect, SeasonCoordinate coord, string seasonName) {
            this.placedObject = go;
            this.effect = effect;
            this.coord = coord;
            this.seasonName = seasonName;
        }

        public ObjectPlacedInSeasonStartEvent(GameObject go, SeasonalEffect effect, SeasonCoordinate coord, string seasonName, string previousSeason) {
            this.placedObject = go;
            this.effect = effect;
            this.coord = coord;
            this.seasonName = seasonName;
            this.previousSeason = previousSeason;
        }
    }

    /// <summary>
    /// Fired by: SeasonalEffect
    /// Fires: When an object is placed for the first time (i.e. has no other variants)
    /// Listened for by: SeasonInfo
    /// </summary>
    public class SeasonalObjectPlacedForFirstTime : IGameEvent {
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

        public SeasonalObjectPlacedForFirstTime(GameObject go, SeasonalEffect effect, string season, SeasonCoordinate placedAtCoord) {
            this.placedObject = go;
            this.placedInSeason = season;
            this.placedAtCoord = placedAtCoord;
            this.effect = effect;
        }
    }

    /// <summary>
    /// Fired by: GrabObjects
    /// Fires: When an object is released inside of an InventoryTarget
    /// 
    /// </summary>
    public class ObjectPlacedOnInventoryTargetEvent : IGameEvent {
        public GameObject placedObject;
        public ObjectPlacedOnInventoryTargetEvent(GameObject go) {
            this.placedObject = go;
        }
    }

    public class ObjectRemovedFromInventoryTargetEvent : IGameEvent {
        public GameObject placedObject;
        public ObjectRemovedFromInventoryTargetEvent(GameObject go) {
            this.placedObject = go;
        }
    }

    public class ObjectAcceptedByInventoryTargetEvent : IGameEvent {
        public GameObject acceptedObject;
        public ObjectAcceptedByInventoryTargetEvent(GameObject go) {
            this.acceptedObject = go;
        }
    }

    public class ObjectEjectedByInventoryTargetEvent : IGameEvent {
        public GameObject ejectedObject;
        public ObjectEjectedByInventoryTargetEvent(GameObject go) {
            this.ejectedObject = go;
        }
    }

    /// <summary>
    /// Fires when all of the game's scenes have been loaded asynchronously from MainScene
    /// </summary>
    public class AllScenesLoadedEvent : IGameEvent { }

    public class PrologueDoneEvent : IGameEvent { }

    public class PrologueStartEvent : IGameEvent { }

    public class MenuEnableEvent : IGameEvent { }

    public class MenuDisableEvent : IGameEvent { }
}
