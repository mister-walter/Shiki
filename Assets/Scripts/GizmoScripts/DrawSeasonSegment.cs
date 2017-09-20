using UnityEngine;

public class DrawSeasonSegment : MonoBehaviour {
    public Color color = Color.green;
    void OnDrawGizmos()
    {
        #if (UNITY_EDITOR)
        var meshFilter = this.gameObject.GetComponent<MeshFilter>();
        if(meshFilter == null)
        {
            return;
        }
        UnityEditor.Handles.matrix *= Matrix4x4.TRS(this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform.lossyScale);
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawPolyLine(meshFilter.sharedMesh.vertices);
        #endif
    }
}
