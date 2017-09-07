/// @author Andrew Walter

using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

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

    void Awake()
    {
        SeasonCoordinateManager.RegisterSeasonStartAngle(seasonName, startAngle);
        GameEventSystem.AttachDelegate<ObjectPutDownEvent>(OnObjectPlaced);
        GameEventSystem.AttachDelegate<SeasonalObjectPlacedForFirstTime>(OnSeasonalObjectPlacedForFirstTime);
    }

    void OnDestroy()
    {
        GameEventSystem.RemoveDelegate<ObjectPutDownEvent>(OnObjectPlaced);
        GameEventSystem.RemoveDelegate<SeasonalObjectPlacedForFirstTime>(OnSeasonalObjectPlacedForFirstTime);
    }

    #region Event Handlers
    public void OnObjectPlaced(ObjectPutDownEvent evt)
    {
        if(evt.effect != null)
        {
            if (IsPositionInSeason(evt.placedObject.transform.position))
            {
                // Check whether the object being placed came from a different scene than this one
                if (evt.placedObject.scene != this.gameObject.scene)
                {
                    // Save the name of the scene the object came from
                    var previousScene = evt.placedObject.scene.name;
                    // If it came from a different scene, we have to move it to this scene first.
                    SceneManager.MoveGameObjectToScene(evt.placedObject, this.gameObject.scene);
                    var coord = SeasonCoordinateManager.GlobalToSeasonCoordinate(evt.placedObject.transform.position);
                    // We add the name of the previous season to the ObjectPlacedInSeasonEvent so that the object
                    // originally in this season can move there (essentially swapping itself with the placed object).
                    // This ensures that there is always one copy of the object for each season.
                    GameEventSystem.FireEvent(new ObjectPlacedInSeasonEvent(evt.placedObject, evt.effect, coord, this.seasonName, previousScene));
                } else
                {
                    // This object came from within this scene, so all we have to do is update its position and fire
                    // a ObjectPlacedInSeasonEvent
                    var coord = SeasonCoordinateManager.GlobalToSeasonCoordinate(evt.placedObject.transform.position);
                    GameEventSystem.FireEvent(new ObjectPlacedInSeasonEvent(evt.placedObject, evt.effect, coord, this.seasonName));
                }
            }
        }
    }

    public void OnSeasonalObjectPlacedForFirstTime(SeasonalObjectPlacedForFirstTime evt)
    {
        // We want to ignore this event if the object that fired this event came from this season
        if (evt.placedInSeason != this.seasonName)
        {
            // Make a new copy of the object
            var newObject = GameObject.Instantiate<GameObject>(evt.placedObject);
            // Determine the correct global coordinate for the object and set it
            newObject.transform.position = SeasonCoordinateManager.SeasonToGlobalCoordinate(this.seasonName, evt.placedAtCoord);
            var newSeasonalEffect = newObject.GetComponent<SeasonalEffect>();
            // Set the new object's seasonal effect id so that we can tell the new object is a variant of the original
            newSeasonalEffect.id = evt.effect.id;
            SceneManager.MoveGameObjectToScene(newObject, this.gameObject.scene);
            newSeasonalEffect.UpdateColor();
            newObject.SetActive(true);
        }
    }
    #endregion

    /// <summary>
    /// Determines whether or not a given position is inside this season.
    /// </summary>
    /// <param name="position">The position to test.</param>
    /// <returns>True if the position is inside this season, false otherwise.</returns>
    public bool IsPositionInSeason(Vector3 position)
    {
        position.y = 5; // Make sure it's above the ground
        // TODO: @Drew can we just replace MeshCollider with Collider?
        var collider = ground.GetComponent<MeshCollider>();
        var ray = new Ray(position, Vector3.down);
        RaycastHit info;
        var res = collider.Raycast(ray, out info, 10);
        return res;
    }
}
