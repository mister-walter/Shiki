using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeasonalEffect : MonoBehaviour {

    private string seasonName;
    private SeasonCoordinate coord;
    private MainSceneManager manager;
    public Guid id = Guid.Empty;

    private MainSceneManager getMSM()
    {
        if(manager == null)
        {
            manager = MainSceneManager.GetMainSceneManager();
        }
        return manager;
    }

	void Start () {
        manager = MainSceneManager.GetMainSceneManager();
        UpdateColor(GetSeasonName());
	}

    private string GetSeasonName()
    {
        Debug.Log(manager);
        Debug.Log(getMSM());
        return getMSM().GetSeasonNameFromPosition(this.gameObject.transform.position);
    }

    private Scene GetSeasonScene()
    {
        return manager.GetSeasonSceneFromPosition(this.gameObject.transform.position).Value;
    }

    void Update()
    {
        //var newSeasonName = manager.GetSeasonNameFromPosition(this.gameObject.transform.position);
        //Debug.Log(newSeasonName);
    }

    public void UpdateSeason()
    {
        UpdateColor(GetSeasonName());
    }

    public void OnPlaced()
    {
        var seasonName = GetSeasonName();
        if (seasonName != "")
        {
            UpdateColor(seasonName);

            var seasonScene = GetSeasonScene();
            var scm = seasonScene.GetSeasonCoordinateManager();
            var coords = scm.globalToSeasonCoordinate(this.gameObject.transform.position);
            // check if this object has a unique id yet
            // if not, this is the first time this object has been placed, so we have to place it in the other seasons
            if (this.id == Guid.Empty)
            {
                this.id = Guid.NewGuid();
                manager.PlaceInOtherSeasons(this.gameObject, coords, seasonScene);
            }
            else
            {
                manager.UpdatePositionInOtherSeasons(this.id, coords, seasonScene);
            }
        }
    }

    void UpdateColor (string seasonName)
    {
        switch (seasonName)
        {
            case "Winter": this.GetComponent<Renderer>().material.SetColor("_Color", Color.blue); break; //turns blue
            case "Spring": this.GetComponent<Renderer>().material.SetColor("_Color", Color.magenta); break; //turns pink
            case "Summer": this.GetComponent<Renderer>().material.SetColor("_Color", Color.green); break; //turns green
            case "Fall": this.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow); break; //turns yellow
            default: this.GetComponent<Renderer>().material.SetColor("_Color", Color.white); break; ; //turns to white
        }
    }
}
