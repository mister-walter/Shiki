using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Struct representing a coordinate in 3D cylindrical space.
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

    private static Dictionary<string, float> seasonStartAngles = new Dictionary<string, float>();

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
        while (angle < 0)
        {
            angle = angle + 360;
        }

        if (angle <= 90) {
            return angle;
        } else if(angle > 90 && angle <= 180) {
            return angle - 90;
        } else if (angle > 180 && angle < 270) {
            return angle - 180;
        } else {
            return angle - 270;
        }
    }
}
