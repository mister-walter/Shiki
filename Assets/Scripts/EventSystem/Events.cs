using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventSystem.Events {
    /// <summary>
    /// Fires: When an object is placed
    /// Listened for by: SeasonInfo, which then fires ObjectPlacedInSeasonEvent if the object was placed in its season
    /// </summary>
    public class ObjectPlacedEvent : IGameEvent
    {
        public GameObject placedObject;
        public SeasonalEffect effect = null;
        public ObjectPlacedEvent(GameObject go)
        {
            this.placedObject = go;
        }
        public ObjectPlacedEvent(GameObject go, SeasonalEffect effect)
        {
            this.placedObject = go;
            this.effect = effect;
        }
    }

    /// <summary>
    /// Fired by: SeasonInfo
    /// Fires: When an object is placed into a season (after ObjectPlacedEvent)
    /// Listened for by: SeasonalEffect
    /// </summary>
    public class ObjectPlacedInSeasonEvent : IGameEvent
    {
        public GameObject placedObject;
        public SeasonCoordinate coord;
        public string seasonName;
        public SeasonalEffect effect;
        public ObjectPlacedInSeasonEvent(GameObject go, SeasonalEffect effect, SeasonCoordinate coord, string seasonName)
        {
            this.placedObject = go;
            this.effect = effect;
            this.coord = coord;
            this.seasonName = seasonName;
        }
    }

    public class ObjectPickedUpEvent : IGameEvent
    {
        public GameObject pickedUpObject;
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

    public class SeasonalObjectPlacedForFirstTime : IGameEvent
    {
        public GameObject placedObject;
        public string placedInSeason;
        public SeasonCoordinate placedAtCoord;
        public SeasonalEffect effect;

        public SeasonalObjectPlacedForFirstTime(GameObject go, SeasonalEffect effect, string season, SeasonCoordinate placedAtCoord)
        {
            this.placedObject = go;
            this.placedInSeason = season;
            this.placedAtCoord = placedAtCoord;
            this.effect = effect;
        }
    }
}

