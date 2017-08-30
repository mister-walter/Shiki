using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventSystem.Events;

public class SeasonalEffect : MonoBehaviour {

    private string seasonName;
    private SeasonCoordinate coord;
    public Guid id = Guid.Empty;

    void Awake ()
    {
        GameEventSystem.AttachDelegate<ObjectPlacedInSeasonEvent>(this.OnPlacedInSeason);
        GameEventSystem.AttachDelegate<ObjectPickedUpEvent>(this.OnPickedUp);
    }

	void Start () {
        seasonName = MainSceneManager.SceneNameToSeasonName(this.gameObject.scene.name);

        UpdateColor();
	}

    public void UpdateColor()
    {
        switch (this.seasonName)
        {
            case SeasonName.Winter: this.GetComponent<Renderer>().material.SetColor("_Color", Color.blue); break; //turns blue
            case SeasonName.Spring: this.GetComponent<Renderer>().material.SetColor("_Color", Color.magenta); break; //turns pink
            case SeasonName.Summer: this.GetComponent<Renderer>().material.SetColor("_Color", Color.green); break; //turns green
            case SeasonName.Fall: this.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow); break; //turns yellow
            default: this.GetComponent<Renderer>().material.SetColor("_Color", Color.white); break; ; //turns to white
        }
    }

    #region Event Handlers
    public void OnPlacedInSeason(ObjectPlacedInSeasonEvent evt)
    {
        if (evt.placedObject.GetInstanceID() == this.gameObject.GetInstanceID())
        {
            this.seasonName = evt.seasonName;
            UpdateColor();

            // check if this object has a unique id yet
            if (this.id == Guid.Empty)
            {
                // if not, this is the first time this object has been placed, so we have to place it in the other seasons
                this.id = Guid.NewGuid();
                GameEventSystem.FireEvent(new SeasonalObjectPlacedForFirstTime(this.gameObject, this, evt.seasonName, evt.coord));
            }
        } else
        {
            if (this.IsSeasonalVariantOf(evt.placedObject, evt.effect))
            {
                this.gameObject.transform.position = SeasonCoordinateManager.SeasonToGlobalCoordinate(this.seasonName, evt.coord);
            }
        }
    }

    internal void OnPickedUp(ObjectPickedUpEvent evt)
    {
        // We only care when this happens for seasonal objects
        if (evt.effect != null)
        {
            if(this.IsSeasonalVariantOf(evt.pickedUpObject, evt.effect))
            {
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
    private bool IsSeasonalVariantOf(GameObject otherObj, SeasonalEffect otherEffect)
    {
        return otherEffect.id != Guid.Empty &&
               otherEffect.id == this.id &&
               otherObj.GetInstanceID() != this.gameObject.GetInstanceID();
    }
}
