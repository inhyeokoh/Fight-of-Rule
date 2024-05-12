using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grid : MonoBehaviour
{
   
    public Node[] vertes;
    public PathFinding pathFinding;

    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshDataInstance;
  
    private void Awake()
    {
        // NavMeshData 생성
        navMeshData = new NavMeshData();
        navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);
        
        // NavMeshData를 기반으로 그리드 생성
        CrateGrid();
    }
  
    void CrateGrid()
    {
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        vertes = new Node[triangulation.vertices.Length];

        for (int i = 0; i < triangulation.vertices.Length; i++)
        {
            vertes[i] = new Node(triangulation.vertices[i]);
        }
        
    }

    void OnDestroy()
    {
        NavMesh.RemoveNavMeshData(navMeshDataInstance); // 그리드 생성 후에는 NavMeshDataInstance 제거
    }

    void OnDrawGizmosSelected()
    {
        // NavMeshData가 없는 경우 또는 유효하지 않은 경우 함수를 종료
        if (navMeshData == null || !navMeshDataInstance.valid)
            return;

        // NavMesh를 시각적으로 표시
        Gizmos.color = Color.cyan; 
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        foreach(Vector3 a in triangulation.vertices)
        {
            Gizmos.DrawCube(a, Vector3.one * 0.3f);
        }
      
        for (int i = 0; i < triangulation.indices.Length; i += 3)
        {
            Vector3 vert0 = triangulation.vertices[triangulation.indices[i]];
            Vector3 vert1 = triangulation.vertices[triangulation.indices[i + 1]];
            Vector3 vert2 = triangulation.vertices[triangulation.indices[i + 2]];
            Gizmos.DrawLine(vert0, vert1);
            Gizmos.DrawLine(vert1, vert2);
            Gizmos.DrawLine(vert2, vert0);
        }
    }

    public List<Node> GetNeighborus(Node currentNode)
    {
        List<Node> nodes = new List<Node>();



        return nodes;
    }
}
