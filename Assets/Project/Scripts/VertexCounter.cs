using UnityEngine;

// This script requires a MeshFilter component to be on the same GameObject.
[RequireComponent(typeof(MeshFilter))]
public class VertexCounter : MonoBehaviour
{
    void Start()
    {
        // Get the MeshFilter component attached to this object.
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter.mesh != null)
        {
            // Get the vertex count from the mesh.
            int vertexCount = meshFilter.mesh.vertexCount;
            // Print the result to the console.
            Debug.Log(gameObject.name + " has " + vertexCount + " vertices.");
        }
        else
        {
            Debug.LogWarning("No mesh found on " + gameObject.name);
        }
    }
}