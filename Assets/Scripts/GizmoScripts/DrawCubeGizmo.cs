#if(UNITY_EDITOR)
using UnityEngine;
#endif

public class DrawCubeGizmo : MonoBehaviour {
    public Color color = Color.red;
    void OnDrawGizmos()
    {
        #if (UNITY_EDITOR)
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawWireCube(transform.position, transform.localScale);
        #endif
    }
}
