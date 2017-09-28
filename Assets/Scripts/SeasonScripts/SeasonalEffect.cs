// @author Andrew Walter

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.EventSystem.InternalEvents;
using Shiki.Constants;
using Shiki;

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

    public string baseItemName;

    public bool wasPlacedVariant = false;

    private VariantDatabase variantDatabase { get { return VariantDatabaseSingleton.GetDatabase(); } }

    private GameObject child {
        get {
            if(this.transform.childCount < 1) {
                return null;
            } else {
                return this.transform.GetChild(0).gameObject;
            }
        }
    }

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
        if(!String.IsNullOrEmpty(this.baseItemName))
            UpdatePrefab();
    }

    void OnDestroy() {
        EventManager.RemoveDelegate<ObjectPlacedInSeasonStartEvent>(this.OnPlacedInSeason);
        EventManager.RemoveDelegate<ObjectPickedUpEvent>(this.OnPickedUp);
    }

    public void UpdatePrefab() {
        if(this.child == null) {
            var prefabName = variantDatabase.GetPrefabName(this.baseItemName, this.seasonName);
            var newGo = Shiki.Loader.LoadPrefabInstance(prefabName);
            SceneManager.MoveGameObjectToScene(newGo, this.gameObject.scene);
            newGo.transform.parent = this.gameObject.transform;
        } else {
            if(!this.wasPlacedVariant) {
                var prefabName = variantDatabase.GetPrefabName(this.baseItemName, this.seasonName);
                if(prefabName != this.child.name) {
                    var newGo = Shiki.Loader.LoadPrefabInstance(prefabName);
                    SceneManager.MoveGameObjectToScene(newGo, this.gameObject.scene);
                    newGo.transform.localPosition = child.transform.localPosition;
                    newGo.transform.localRotation = child.transform.localRotation;
                    Destroy(this.child);
                    newGo.transform.parent = this.gameObject.transform;
                }
            }
        }
    }

    /// <summary>
    /// Copy neccesary information from another SeasonalEffect to this one
    /// </summary>
    /// <param name="otherEffect"></param>
    public void SetupFromSeasonalEffect(SeasonalEffect otherEffect) {
        this.baseItemName = otherEffect.baseItemName;
        this.id = otherEffect.id;
    }

    #region Event Handlers
    public void OnPlacedInSeason(ObjectPlacedInSeasonStartEvent evt) {
        Debug.Log("Placed in season");
        Debug.Log(evt.seasonName);
        if(evt.placedObject.GetInstanceID() == this.gameObject.GetInstanceID()) {
            this.seasonName = evt.seasonName;
            this.wasPlacedVariant = true;
            UpdatePrefab();

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
                    UpdatePrefab();
                }
                this.gameObject.SetActive(true);
                this.child.transform.position = SeasonCoordinateManager.SeasonToGlobalCoordinate(this.seasonName, evt.coord);
                UpdatePrefab();
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
