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

public static class SeasonCoordinateManager {

    private static Dictionary<string, float> seasonStartAngles;

    public static void RegisterSeasonStartAngle(string seasonName, float startAngle)
    {
        seasonStartAngles.Add(seasonName, startAngle);
    }

    public static Vector3 SeasonToGlobalCoordinate(string seasonName, SeasonCoordinate sc)
    {
        var startAngle = seasonStartAngles[seasonName];
        var angleRad = Mathf.Deg2Rad * (sc.angle + startAngle);
        var x = sc.radius * Mathf.Cos(angleRad);
        var z = sc.radius * Mathf.Sin(angleRad);
        return new Vector3(x, sc.height, z);
    }

    public static Vector3 SeasonToGlobalCoordinate(float startAngle, SeasonCoordinate sc)
    {
        var angleRad = Mathf.Deg2Rad * (sc.angle + startAngle);
        var x = sc.radius * Mathf.Cos(angleRad);
        var z = sc.radius * Mathf.Sin(angleRad);
        return new Vector3(x, sc.height, z);
    }

    public static SeasonCoordinate GlobalToSeasonCoordinate(Vector3 gc)
    {
        var radius = Mathf.Sqrt(gc.x * gc.x + gc.z * gc.z);
        var angle = AngleInQuadrant(gc.x, gc.z);
        return new SeasonCoordinate(radius, angle, gc.y);
    }

    /// <summary>
    /// Determines the angle (ccw) between a point and the start of the quadrant that it is in.
    /// The quadrants start 90 degrees apart, starting from -45 degrees.
    /// Note: if x and y are 0, returns 45.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>The angle, in degrees.</returns>
    public static float AngleInQuadrant(float x, float y)
    {
        var angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        if (-45 <= angle && angle < 45)
        {
            return angle + 45;
        } else if (45 <= angle && angle < 135)
        {
            return angle - 45;
        } else if (-135 <= angle && angle < -45)
        {
            return angle + 135;
        } else if (135 <= angle)
        {
            return angle - 135;
        } else if (-135 > angle)
        {
            return angle + 225;
        } else
        {
            throw new System.ArithmeticException(string.Format("Can't find angle for coordinates: {0}, {1}, angle {2}", x, y, angle));
        }
    }
}
