/// @author Andrew Walter

using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.EventSystem.InternalEvents;
using Shiki.Constants;

public enum IsInSeasonStatus {
    InVillage,
    InSeason,
    NotInSeasonOrVillage
}

/// <summary>
/// Component that stores information about a season.
/// </summary>
public class SeasonInfo : MonoBehaviour {
    /// <summary>
    /// The name of the season. Must be the same as the season name in Shiki.Constants.SeasonName
    /// </summary>
    public string seasonName;

    /// <summary>
    /// The angle at which the season starts, in degrees (going ccw).
    /// </summary>
    public float startAngle;

    /// <summary>
    /// The GameObject representing the ground of this season. Used to determine whether an object is in this season or
    /// not. Must have a Collider.
    /// </summary>
    public GameObject ground;

    // TODO: @Drew fix this
    public GameObject villageGround;

    void Awake() {
        Physics.IgnoreLayerCollision(LayerManager.TeleportAreaLayer, LayerManager.DefaultLayer);
        SeasonCoordinateManager.RegisterSeasonStartAngle(seasonName, startAngle);
        EventManager.AttachDelegate<ObjectPlacedEvent>(OnObjectPlaced);
        EventManager.AttachDelegate<SeasonalObjectPlacedForFirstTime>(OnSeasonalObjectPlacedForFirstTime);
    }

    void OnDestroy() {
        EventManager.RemoveDelegate<ObjectPlacedEvent>(OnObjectPlaced);
        EventManager.RemoveDelegate<SeasonalObjectPlacedForFirstTime>(OnSeasonalObjectPlacedForFirstTime);
    }

    #region Event Handlers
    public void OnObjectPlaced(ObjectPlacedEvent evt) {
        if(evt.effect != null) {
            if(IsPositionInSeason(evt.placedObject.transform.position)) {
                // Check whether the object being placed came from a different scene than this one
                if(evt.placedObject.scene != this.gameObject.scene) {
                    // Save the name of the scene the object came from
                    var previousScene = evt.placedObject.scene.name;
                    var previousSeason = MainSceneManager.SceneNameToSeasonName(previousScene);
                    // If it came from a different scene, we have to move it to this scene first.
                    SeasonalEffect effect = evt.placedObject.GetComponentInSelfOrImmediateParent<SeasonalEffect>();
                    SceneManager.MoveGameObjectToScene(effect.gameObject, this.gameObject.scene);
                    var coord = SeasonCoordinateManager.GlobalToSeasonCoordinate(evt.placedObject.transform.position);
                    // We add the name of the previous season to the ObjectPlacedInSeasonStartEvent so that the object
                    // originally in this season can move there (essentially swapping itself with the placed object).
                    // This ensures that there is always one copy of the object for each season.
                    EventManager.FireEvent(new ObjectPlacedInSeasonStartEvent(evt.placedObject, evt.effect, coord, this.seasonName, previousSeason));
                } else {
                    // This object came from within this scene, so all we have to do is update its position and fire
                    // a ObjectPlacedInSeasonStartEvent
                    var coord = SeasonCoordinateManager.GlobalToSeasonCoordinate(evt.placedObject.transform.position);
                    EventManager.FireEvent(new ObjectPlacedInSeasonStartEvent(evt.placedObject, evt.effect, coord, this.seasonName));
                }
            }
        }
    }

    public void OnSeasonalObjectPlacedForFirstTime(SeasonalObjectPlacedForFirstTime evt) {
        // We want to ignore this event if the object that fired this event came from this season
        if(evt.placedInSeason != this.seasonName) {
            Debug.Log("Making first version of object in season " + this.seasonName);
            // Make a new copy of the object
            //var newObject = GameObject.Instantiate<GameObject>(evt.placedObject);
            var newObject = new GameObject();
            SeasonalEffect newSeasonalEffect = newObject.AddComponent<SeasonalEffect>();
            newSeasonalEffect.SetSeason(this.seasonName);
            newSeasonalEffect.SetupFromSeasonalEffect(evt.effect);
            // Determine the correct global coordinate for the object and set it

            // Set the new object's seasonal effect id so that we can tell the new object is a variant of the original
            newSeasonalEffect.wasPlacedVariant = false;
            SceneManager.MoveGameObjectToScene(newObject, this.gameObject.scene);
            newSeasonalEffect.UpdatePrefab();
            newSeasonalEffect.UpdateChildPosition(SeasonCoordinateManager.SeasonToGlobalCoordinate(this.seasonName, evt.placedAtCoord));
            newObject.SetActive(true);
        } else {
            Debug.Log(string.Format("Placed for first time in season {0}", evt.placedInSeason));
        }
    }
    #endregion

    /// <summary>
    /// Determines whether or not a given position is inside this season.
    /// </summary>
    /// <param name="position">The position to test.</param>
    /// <returns>True if the position is inside this season, false otherwise.</returns>
    public bool IsPositionInSeason(Vector3 position) {
        position.y = 5; // Make sure it's above the ground
        // TODO: @Drew can we just replace MeshCollider with Collider?
        var collider = ground.GetComponent<MeshCollider>();
        var vilCollider = villageGround.GetComponent<MeshCollider>();
        var ray = new Ray(position, Vector3.down);
        RaycastHit info;
        if(vilCollider.Raycast(ray, out info, 10)) {
            return false;
        }

        var res = collider.Raycast(ray, out info, 10);
        return res;
    }

    public IsInSeasonStatus CheckPosition(Vector3 position) {
        position.y = 5; // Make sure it's above the ground
        // TODO: @Drew can we just replace MeshCollider with Collider?
        var collider = ground.GetComponent<MeshCollider>();
        var vilCollider = villageGround.GetComponent<MeshCollider>();
        var ray = new Ray(position, Vector3.down);
        RaycastHit info;
        if(vilCollider.Raycast(ray, out info, 10)) {
            return IsInSeasonStatus.InVillage;
        }

        var res = collider.Raycast(ray, out info, 10);
        if(res) {
            return IsInSeasonStatus.InSeason;
        } else {
            return IsInSeasonStatus.NotInSeasonOrVillage;
        }
    }
}
