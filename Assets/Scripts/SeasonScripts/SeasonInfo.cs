using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeasonInfo : MonoBehaviour {
    public string seasonName;
    public GameObject ground;
    private MainSceneManager manager;

    public void Start()
    {
        manager = MainSceneManager.GetMainSceneManager();
    }

    public bool IsPositionInSeason(Vector3 position)
    {
        position.y = 5; // Make sure it's above the ground
        var collider = ground.GetComponent<MeshCollider>();
        var ray = new Ray(position, Vector3.down);
        RaycastHit info;
        var res = collider.Raycast(ray, out info, 10);
        return res;
    }

    public void PlaceInSeason(GameObject obj, SeasonCoordinate coordinates)
    {
        var scene = this.gameObject.scene;
        var scm = scene.GetSeasonCoordinateManager();
        var newObject = GameObject.Instantiate<GameObject>(obj);
        newObject.transform.position = scm.seasonToGlobalCoordinate(coordinates);
        var originalSe = obj.GetComponent<SeasonalEffect>();
        var newSe = newObject.GetComponent<SeasonalEffect>();
        newSe.id = originalSe.id;
        SceneManager.MoveGameObjectToScene(newObject, scene);
        newSe.UpdateSeason();
        
    }

    public void UpdatePositionInSeason(Guid id, SeasonCoordinate coordinates)
    {
        var scene = this.gameObject.scene;
        var scm = scene.GetSeasonCoordinateManager();
        foreach(var gameObject in scene.GetRootGameObjects())
        {
            var seasonalEffect = gameObject.GetComponent<SeasonalEffect>();
            if(seasonalEffect != null)
            {
                if(seasonalEffect.id == id)
                {
                    gameObject.transform.position = scm.seasonToGlobalCoordinate(coordinates);
                    return;
                }
            }
        }
    }
}
