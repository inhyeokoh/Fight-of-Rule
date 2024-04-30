using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMashExtract : MonoBehaviour
{
    public Vector3[] h;
    private void Awake()
    {
        MeshFilter filter = GetComponent<MeshFilter>();

        NavMeshTriangulation navmeshData = NavMesh.CalculateTriangulation();
        h = navmeshData.vertices;
        Mesh mesh = new Mesh();

        mesh.vertices = navmeshData.vertices;
        mesh.triangles = navmeshData.indices;
       /* mesh.SetVertices(navmeshData.vertices);
        mesh.SetIndices(navmeshData.indices, MeshTopology.Triangles, 0);
*/
        filter.mesh = mesh;
    }
}
