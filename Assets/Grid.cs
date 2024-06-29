using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grid : MonoBehaviour
{

    private Node[] vertes;
    public PathFinding pathFinding;

    public int index;

    private List<Node> path = new List<Node>();
    private List<(Node nodeA, Node nodeB)> redLine = new List<(Node nodeA, Node nodeB)>();

    Transform[] nodes;
   
    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshDataInstance;

    public bool grid;

    private void Awake()

    {
        // NavMeshData 생성
        navMeshData = new NavMeshData();
        navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);
        
        // NavMeshData를 기반으로 그리드 생성
        CrateGrid();
      //  pathFinding.FindPath();
    }

    public void Update()
    {
        pathFinding.FindPath(path, redLine);
       /* if (Input.GetKeyDown(KeyCode.M))
        {
            if (path != null)
            {
                path.Clear();
            }
            if(redLine != null)
            {
                redLine.Clear();
            }
            nodes = pathFinding.FindPath(path, redLine);
            print(path.Count);
            print(redLine.Count);
        }*/

        /*if (path != null && path.Count > 0)
        {
            Debug.DrawLine(this.nodes[0].position, path[0].vertexCenter, Color.blue);

            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i].vertexCenter, path[i + 1].vertexCenter, Color.blue);
            }

            Debug.DrawLine(path[path.Count - 1].vertexCenter, nodes[1].position, Color.blue);
        }

        if (redLine != null & redLine.Count > 0)
        {
            for (int i = 0; i < redLine.Count; i++)
            {
                Debug.DrawLine(redLine[i].nodeA.vertexCenter, redLine[i].nodeB.vertexCenter, Color.red);
            }
        }*/
    }

    void CrateGrid()
    {
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        vertes = new Node[triangulation.indices.Length / 3] ;
        index = vertes.Length;
        int vertexsIndex = 0;
        int number = 0;

        for (int i = 0; i < triangulation.indices.Length; i += 3)
        {
            Vector3 vert0 = triangulation.vertices[triangulation.indices[i]];       
            Vector3 vert1 = triangulation.vertices[triangulation.indices[i + 1]];      
            Vector3 vert2 = triangulation.vertices[triangulation.indices[i + 2]];
           
            vertes[vertexsIndex] = new Node(vert0, vert1, vert2, number);
            vertexsIndex++;
            number++;
        }

        for(int i =0; i < vertes.Length; i++)
        {
            vertes[i].neighbours = GetNeighborus(vertes[i]);
        }
    }

    void OnDestroy()
    {
        NavMesh.RemoveNavMeshData(navMeshDataInstance); // 그리드 생성 후에는 NavMeshDataInstance 제거
    }

    void OnDrawGizmos()
    {
        // NavMeshData가 없는 경우 또는 유효하지 않은 경우 함수를 종료
        if (navMeshData == null || !navMeshDataInstance.valid)
            return;

        // NavMesh를 시각적으로 표시
        Gizmos.color = Color.cyan; 
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        if (grid)
        {
            foreach (Vector3 a in triangulation.vertices)
            {
                if (Mathf.Abs(a.x) >= 0.1f)
                {
                    Gizmos.DrawCube(a, Vector3.one * 0.3f);
                }
            }

            for (int i = 0; i < vertes.Length; i++)
            {
                Gizmos.DrawLine(vertes[i].vertexOne, vertes[i].vertexTwo);
                Gizmos.DrawLine(vertes[i].vertexTwo, vertes[i].vertexThree);
                Gizmos.DrawLine(vertes[i].vertexThree, vertes[i].vertexOne);

                Gizmos.DrawCube(vertes[i].vertexCenter, Vector3.one * 0.1f);
            }
        }
    }

    public List<Node> GetNeighborus(Node currentNode)
    {
        List<Node> nodes = new List<Node>();

        foreach (var node in vertes)
        {
            if (node == currentNode)
                continue;

            int a = 0;

            if (currentNode.vertexOne == node.vertexOne || currentNode.vertexOne == node.vertexTwo || currentNode.vertexOne == node.vertexThree)
            {
                a++;
                //nodes.Add(node);
            }
            if (currentNode.vertexTwo == node.vertexOne || currentNode.vertexTwo == node.vertexTwo || currentNode.vertexTwo == node.vertexThree)
            {
                a++;
                //nodes.Add(node);
            }
            if (currentNode.vertexThree == node.vertexOne || currentNode.vertexThree == node.vertexTwo || currentNode.vertexThree == node.vertexThree)
            {
                a++;
                //nodes.Add(node);
            }

            if (a >= 2)
            {
                nodes.Add(node);
            }
        }
      
        return nodes;
    }

    public bool ColorBox(Vector3 nodePosition)
    {
        foreach (var node in vertes)
        {
            if (CheckTriangleInPoint(node.vertexOne, node.vertexTwo, node.vertexThree, nodePosition))
            {
                return true;
            }
        }

        return false;
    }


    public Node FindNode(Vector3 nodePosition)
    {
        foreach (var node in vertes)
        {
            if (CheckTriangleInPoint(node.vertexOne, node.vertexTwo, node.vertexThree, nodePosition))
            {
                //return true;
                return node;
            }
        }

        //return false;
        return null;
    }

    bool CheckTriangleInPoint(Vector3 dot1, Vector3 dot2, Vector3 dot3, Vector3 checkPoint)
    {
        float area = getAreaOfTriangle(dot1, dot2, dot3);
        float dot12 = getAreaOfTriangle(dot1, dot2, checkPoint);
        float dot23 = getAreaOfTriangle(dot2, dot3, checkPoint);
        float dot31 = getAreaOfTriangle(dot3, dot1, checkPoint);

        return (dot12 + dot23 + dot31) <= area + 0.1f /* 오차 허용 */;
    }


    float getAreaOfTriangle(Vector3 dot1, Vector3 dot2, Vector3 dot3)
    {
        Vector3 a = dot2 - dot1;
        Vector3 b = dot3 - dot1;
        Vector3 cross = Vector3.Cross(a, b);

        return cross.magnitude / 2.0f;
    }
}
