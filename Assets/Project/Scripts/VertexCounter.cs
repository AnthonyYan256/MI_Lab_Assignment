    using UnityEngine;

    public class VertexCounter : MonoBehaviour
    {
        void Start()
        {
            // Get the MeshFilter component attached to this GameObject
            MeshFilter meshFilter = GetComponent<MeshFilter>();

            if (meshFilter != null && meshFilter.mesh != null)
            {
                // Get the Mesh from the MeshFilter
                Mesh mesh = meshFilter.mesh;

                // Print the vertex count to the console
                Debug.Log("Object: " + gameObject.name + " has " + mesh.vertexCount + " vertices.");
            }
            else
            {
                Debug.LogWarning("No MeshFilter or Mesh found on " + gameObject.name);
            }
        }
    }