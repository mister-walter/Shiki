using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_EDITOR)
public class DrawSeasonSegment : MonoBehaviour {
    public Color color = Color.green;
    public float startAngle = 0;
    private float radius = 60;
    void OnDrawGizmos()
    {
        var firstPoint = new Vector3(Mathf.Cos(Mathf.Deg2Rad * startAngle), 0, Mathf.Sin(Mathf.Deg2Rad * startAngle)) * radius;
        var secondPoint = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (startAngle + 90)), 0, Mathf.Sin(Mathf.Deg2Rad * (startAngle + 90))) * radius;
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.down, firstPoint, 90, radius);
        UnityEditor.Handles.DrawLine(firstPoint, firstPoint * 0.5f);
        UnityEditor.Handles.DrawLine(secondPoint, secondPoint * 0.5f);
        UnityEditor.Handles.DrawLine(firstPoint * 0.5f, secondPoint * 0.5f);
    }
}
#endif
