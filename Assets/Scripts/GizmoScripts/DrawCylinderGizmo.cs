using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if(UNITY_EDITOR)
public class DrawCylinderGizmo : MonoBehaviour {
    public Color color = Color.blue;
    void OnDrawGizmos()
    {
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.down, transform.localScale.x / 2);
    }
}
#endif