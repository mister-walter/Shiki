// @author Andrew Walter

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.EventSystem.InternalEvents;
using Shiki.Constants;

/// <summary>
/// A component providing the ability for an object to have different variants for each season.
/// </summary>
public class SeasonalEffect : MonoBehaviour {
    /// <summary>
    /// The name of the season that this SeasonalEffect belongs to.
    /// </summary>
    private string seasonName;

    /// <summary>
    /// This is how a SeasonalEffect determines whether not another SeasonalEffect is another variant of the same object
    /// It is set to Guid.Empty by default, but gets set to a new pseudo-unique value when needed.
    /// </summary>
    public Guid id = Guid.Empty;

    public bool wasPlacedVariant = false;

    void Awake() {
        EventManager.AttachDelegate<ObjectPlacedInSeasonStartEvent>(this.OnPlacedInSeason);
        EventManager.AttachDelegate<ObjectPickedUpEvent>(this.OnPickedUp);
    }

    void Start() {
        var initSeason = this.gameObject.GetComponent<InitialSeason>();
        if(initSeason != null) {
            seasonName = initSeason.initialSeasonName;
            Destroy(initSeason);
        } else {
            seasonName = MainSceneManager.SceneNameToSeasonName(this.gameObject.scene.name);
        }

        UpdateColor();
    }

    void OnDestroy() {
        EventManager.RemoveDelegate<ObjectPlacedInSeasonStartEvent>(this.OnPlacedInSeason);
        EventManager.RemoveDelegate<ObjectPickedUpEvent>(this.OnPickedUp);
    }

    /// <summary>
    /// Updates the color of this GameObject depending on the season it is in.
    /// </summary>
    public void UpdateColor() {
        if(!this.wasPlacedVariant) {
            switch(this.seasonName) {
                case SeasonName.Winter:
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                    break; //turns blue
                case SeasonName.Spring:
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
                    break; //turns pink
                case SeasonName.Summer:
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                    break; //turns green
                case SeasonName.Fall:
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                    break; //turns yellow
                default:
                    this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    break; //turns to white
            }
        }
    }

    #region Event Handlers
    public void OnPlacedInSeason(ObjectPlacedInSeasonStartEvent evt) {
        Debug.Log("Placed in season");
        Debug.Log(evt.seasonName);
        if(evt.placedObject.GetInstanceID() == this.gameObject.GetInstanceID()) {
            this.seasonName = evt.seasonName;
            this.wasPlacedVariant = true;
            UpdateColor();

            // check if this object has a unique id yet
            if(this.id == Guid.Empty) {
                // if not, this is the first time this object has been placed, so we fire an event that causes the
                // other seasons to create their own variant of this object
                this.id = Guid.NewGuid();
                EventManager.FireEvent(new SeasonalObjectPlacedForFirstTime(this.gameObject, this, evt.seasonName, evt.coord));
            }
        } else {
            if(this.IsSeasonalVariantOf(evt.placedObject, evt.effect)) {
                this.wasPlacedVariant = false;
                // If another variant was placed in the same season as us, we have to move to the season that the
                // variant originally came from
                if(this.seasonName == evt.seasonName) {
                    this.seasonName = MainSceneManager.SceneNameToSeasonName(evt.previousSeason);
                    SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName(evt.previousSeason));
                    UpdateColor();
                }
                this.gameObject.SetActive(true);
                this.gameObject.transform.position = SeasonCoordinateManager.SeasonToGlobalCoordinate(this.seasonName, evt.coord);
                UpdateColor();
            }
        }
    }

    internal void OnPickedUp(ObjectPickedUpEvent evt) {
        // We only care when this happens for seasonal objects
        if(evt.effect != null) {
            if(this.IsSeasonalVariantOf(evt.pickedUpObject, evt.effect)) {
                // Hide seasonal variants of the object being picked up
                this.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    /// <summary>
    /// Determines whether the given GameObject is a seasonal variant of this SeasonalEffect's GameObject.
    /// </summary>
    /// <param name="otherObj"></param>
    /// <param name="otherEffect"></param>
    /// <returns>True if the other GameObject is a seasonal variant of this SeasonalEffect's GameObject, false otherwise.</returns>
    private bool IsSeasonalVariantOf(GameObject otherObj, SeasonalEffect otherEffect) {
        return otherEffect.id != Guid.Empty &&
               otherEffect.id == this.id &&
               otherObj.GetInstanceID() != this.gameObject.GetInstanceID();
    }
}
