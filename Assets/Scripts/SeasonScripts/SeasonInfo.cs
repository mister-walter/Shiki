using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventSystem.Events;

public class SeasonInfo : MonoBehaviour {
    public float startAngle;
    public string seasonName;
    public GameObject ground;

    public void Awake()
    {
        GameEventSystem.AttachDelegate<ObjectPlacedEvent>(OnObjectPlaced);
        GameEventSystem.AttachDelegate<SeasonalObjectPlacedForFirstTime>(OnSeasonalObjectPlacedForFirstTime);
    }

    #region Event Handlers
    public void OnObjectPlaced(ObjectPlacedEvent evt)
    {
        if(evt.effect != null)
        {
            if (IsPositionInSeason(evt.placedObject.transform.position))
            {
                SceneManager.MoveGameObjectToScene(evt.placedObject, this.gameObject.scene);
                var coord = SeasonCoordinateManager.GlobalToSeasonCoordinate(this.gameObject.transform.position);
                GameEventSystem.FireEvent(new ObjectPlacedInSeasonEvent(evt.placedObject, evt.effect, coord, this.seasonName));
            }
        }
    }

    public void OnSeasonalObjectPlacedForFirstTime(SeasonalObjectPlacedForFirstTime evt)
    {
        var newObject = GameObject.Instantiate<GameObject>(evt.placedObject);
        newObject.transform.position = SeasonCoordinateManager.SeasonToGlobalCoordinate(evt.placedInSeason, evt.placedAtCoord);
        var newSeasonalEffect = newObject.GetComponent<SeasonalEffect>();
        newSeasonalEffect.id = evt.effect.id;
        SceneManager.MoveGameObjectToScene(newObject, this.gameObject.scene);
        newSeasonalEffect.UpdateColor();
    }
    #endregion

    public bool IsPositionInSeason(Vector3 position)
    {
        position.y = 5; // Make sure it's above the ground
        var collider = ground.GetComponent<MeshCollider>();
        var ray = new Ray(position, Vector3.down);
        RaycastHit info;
        var res = collider.Raycast(ray, out info, 10);
        return res;
    }
}
