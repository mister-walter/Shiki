using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Cylindrical coordinate
/// <summary>
/// Struct representing a coordinate in 3D cylindrical space
/// </summary>
public struct SeasonCoordinate
{
    public float radius;
    /// <summary>
    /// Angle (in degrees)
    /// </summary>
    public float angle;
    public float height;

    public SeasonCoordinate(float radius, float angle, float height)
    {
        this.radius = radius;
        this.angle = angle;
        this.height = height;
    }
}

public class SeasonCoordinateManager : MonoBehaviour {

    public float startAngle;
    private MainSceneManager mainSceneManager;

    public Vector3 seasonToGlobalCoordinate(SeasonCoordinate sc)
    {
        var angleRad = Mathf.Deg2Rad * (sc.angle + startAngle);
        var x = sc.radius * Mathf.Cos(angleRad);
        var z = sc.radius * Mathf.Sin(angleRad);
        return new Vector3(x, sc.height, z);
    }

    public SeasonCoordinate globalToSeasonCoordinate(Vector3 gc)
    {
        var radius = Mathf.Sqrt(gc.x * gc.x + gc.z * gc.z);
        var angle = Mathf.Rad2Deg * Mathf.Atan2(gc.z, gc.x) - startAngle;
        return new SeasonCoordinate(radius, angle, gc.y);
    }

    // Use this for initialization
    void Start () {
        mainSceneManager = GetComponentInParent<MainSceneManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
