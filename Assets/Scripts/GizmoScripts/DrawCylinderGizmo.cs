#if (UNITY_EDITOR)
using UnityEngine;
#endif

public class DrawCylinderGizmo : MonoBehaviour {
    public Color color = Color.blue;
    void OnDrawGizmos()
    {
        #if (UNITY_EDITOR)
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.down, transform.localScale.x / 2);
        #endif
    }
}
