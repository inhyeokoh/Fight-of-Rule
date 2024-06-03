using System.IO;
using System.Text;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMashExtract : MonoBehaviour
{
    public Vector3[] v;
    public int[] e;
    [MenuItem("Custom/Export NavMesh to mesh")]
    static void Export()
    {
        NavMeshTriangulation triangulatedNavMesh = NavMesh.CalculateTriangulation();

        Mesh mesh = new Mesh();
        mesh.name = "ExportedNavMesh";
        mesh.vertices = triangulatedNavMesh.vertices;
        mesh.triangles = triangulatedNavMesh.indices;
        string filename = Application.dataPath + "/" + Path.GetFileNameWithoutExtension(EditorApplication.currentScene) + " Exported NavMesh.obj";
        MeshToFile(mesh, filename);
        print("NavMesh exported as '" + filename + "'");
        AssetDatabase.Refresh();
    }

    static string MeshToString(Mesh mesh)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append(mesh.name).Append("\n");
        foreach (Vector3 v in mesh.vertices)
        {
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in mesh.normals)
        {
            sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in mesh.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }
        for (int material = 0; material < mesh.subMeshCount; material++)
        {
            sb.Append("\n");
            //sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            //sb.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = mesh.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
            }
        }
        return sb.ToString();
    }

    static void MeshToFile(Mesh mesh, string filename)
    {
        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write(MeshToString(mesh));
        }
    }

    private void Awake()
    {
      /*  MeshFilter filter = GetComponent<MeshFilter>();

        NavMeshTriangulation navmeshData = NavMesh.CalculateTriangulation();
        v = navmeshData.vertices;
        e = navmeshData.indices;
        Mesh mesh = new Mesh();
        


        mesh.vertices = navmeshData.vertices;
        mesh.triangles = navmeshData.indices;
        mesh.SetVertices(navmeshData.vertices);
        mesh.SetIndices(navmeshData.indices, MeshTopology.Triangles, 0);

        filter.mesh = mesh;*/
    }
}
