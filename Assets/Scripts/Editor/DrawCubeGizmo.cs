using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if(UNITY_EDITOR)
public class DrawCubeGizmo : MonoBehaviour {
    public Color color = Color.red;
    void OnDrawGizmos()
    {
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawWireCube(transform.position, transform.localScale);
    }
}
#endif