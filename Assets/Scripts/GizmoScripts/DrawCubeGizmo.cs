using UnityEngine;

public class DrawCubeGizmo : MonoBehaviour {
    public Color color = Color.red;
    void OnDrawGizmos()
    {
        #if (UNITY_EDITOR)
        UnityEditor.Handles.color = color;
        
        UnityEditor.Handles.matrix *= Matrix4x4.TRS(Vector3.zero, this.gameObject.transform.rotation, Vector3.one);
        UnityEditor.Handles.DrawWireCube(transform.position, transform.localScale);
        #endif
    }
}
