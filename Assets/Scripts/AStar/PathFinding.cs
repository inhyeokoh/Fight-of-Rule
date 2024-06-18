using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField]
    Transform startNode;
    [SerializeField]
    Transform endNode;

    public Material red, blue;

    public MeshFilter meshFilter;

    float time = 1.5f;

    public bool funnel;

    Renderer rd1;
    Renderer rd2;

    Vector3[] _vec1;
    Vector3[] _vec2;

    RaycastHit hit;

    Vector3[] paths;
    [SerializeField]
    List<(Vector3 a, Vector3 b)> cross = new List<(Vector3 a, Vector3 b)>();
    public bool triangleOn;

    [SerializeField]
    Vector3[] leftVertices;
    [SerializeField]
    Vector3[] rightVertices;
    [SerializeField]
    List<Vector3> paths1;
    LineRenderer lr1;
    LineRenderer lr2;

    List<(Vector3 a, Vector3 b)> apexLeftLine = new List<(Vector3 a, Vector3 b)>();
    List<(Vector3 a, Vector3 b)> apexRightLine = new List<(Vector3 a, Vector3 b)>();


    List<Node> path;
    [SerializeField]
    List<Vector3> leftLine;
    [SerializeField]
    List<Vector3> rightLine;
    public Grid grid;

    public bool neighborusRedLine;
    public bool fastRedLine;
    public int index;
    void Start()
    {
        path = new List<Node>();
        rd1 = startNode.GetComponent<Renderer>();
        /*lr1 = startNode.GetComponent<LineRenderer>();

        lr1.startWidth = lr1.endWidth = .1f;
        lr1.material.color = Color.red;

        lr1.positionCount = 3;
        lr1.loop = true;*/

        rd2 = endNode.GetComponent<Renderer>();
        /* lr2 = endNode.GetComponent<LineRenderer>();

         lr2.startWidth = lr2.endWidth = .1f;
         lr2.material.color = Color.red;

         lr2.positionCount = 3;
         lr2.loop = true;*/
    }

    public void Update()
    {
        if (grid.ColorBox(this.startNode.position))
        {
            rd1.material = blue;
        }
        else
        {
            rd1.material = red;
        }

        if (grid.ColorBox(this.endNode.position))
        {
            rd2.material = blue;
        }
        else
        {
            rd2.material = red;
        }
    }
    public Transform[] FindPath(List<Node> nodePath, List<(Node nodeA, Node nodeB)> redPath)
    {

        Node startNode = grid.FindNode(this.startNode.position);
        Transform[] node = new Transform[2];
        if (startNode == null)
        {
            return null;
        }
        Node targetNode = grid.FindNode(this.endNode.position);
        if (targetNode == null)
        {
            return null;
        }

        nodePath.Clear();
        redPath.Clear();

        node[0] = this.startNode;
        node[1] = this.endNode;

        bool succes = false;
        Heap<Node> openSet = new Heap<Node>(grid.index * 100);


        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closeSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                succes = true;
                break;
            }
            foreach (Node neighbours in currentNode.neighbours)
            {
                if (closeSet.Contains(neighbours))
                    continue;
                float newNeighboursPath = currentNode.gCost + GetDistance(currentNode.vertexCenter, neighbours.vertexCenter);

                float newHuristicPath = GetDistance(neighbours.vertexCenter, endNode.position);

                if ((newNeighboursPath + newHuristicPath) < neighbours.fCost || !openSet.Contains(neighbours))
                {
                    if (fastRedLine)
                    {
                        //   redPath.Add((currentNode, neighbours));
                        Debug.DrawLine(currentNode.vertexCenter, neighbours.vertexCenter, Color.red);
                    }
                    neighbours.gCost = newNeighboursPath;
                    neighbours.hCost = newHuristicPath;
                    neighbours.parent = currentNode;

                    // currentNode.currentVertex = currentNode.vertexs[i];

                    if (!openSet.Contains(neighbours))
                    {
                        openSet.Add(neighbours);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbours);
                    }
                }
            }
        }
        if (succes)
        {
            paths = RetracePath(startNode, targetNode, nodePath);

            if (paths.Length > 0)
            {
                // Debug.DrawLine(this.startNode.position, path[0].vertexCenter, Color.blue);

                for (int i = 0; i < paths.Length - 1; i++)
                {
                    Debug.DrawLine(paths[i], paths[i + 1], Color.blue);
                }

                // Debug.DrawLine(path[path.Count - 1].vertexCenter, endNode.position, Color.blue);
            }

           // print($"{startNode.vertexOne} {startNode.vertexTwo} {startNode.vertexThree}");
          //  print("succes");

            return node;
        }
        else
        {
            print("fail");
            return null;
        }
    }

    Vector3[] RetracePath(Node startNode, Node endNode, List<Node> nodePath)
    {

        List<Node> path = new List<Node>();
        List<Node>  triangle = new List<Node>();
        Node currentNode = endNode;



        triangle.Add(endNode);

        while (currentNode != startNode)
        {
            if (currentNode == endNode)
            {
                currentNode = currentNode.parent;
                continue;
            }
            path.Add(currentNode);
            triangle.Add(currentNode);
            //nodePath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        triangle.Add(startNode);
        //path.Add(startNode);

        path.Reverse();
        triangle.Reverse();

        if (triangleOn)
        {
            for (int i = 0; i < triangle.Count; i++)
            {
                Debug.DrawLine(triangle[i].vertexOne, triangle[i].vertexTwo, Color.green);
                Debug.DrawLine(triangle[i].vertexTwo, triangle[i].vertexThree, Color.green);
                Debug.DrawLine(triangle[i].vertexThree, triangle[i].vertexOne, Color.green);
            }

        }
   
        Vector3[] vector3s = new Vector3[path.Count + 2];
        vector3s[0] = this.startNode.position;

        for (int i = 1; i < vector3s.Length - 1; i++)
        {
            vector3s[i] = path[i - 1].vertexCenter;
        }

        vector3s[vector3s.Length - 1] = this.endNode.position;
        //  StartCoroutine(Move(vector3s));
        
        if (funnel)
        {
            List<Vector3> postPath = PostProcessing(triangle, this.startNode.position, this.endNode.position);
            if (postPath != null && postPath.Count > 0)
            {
                // Debug.DrawLine(this.startNode.position, path[0].vertexCenter, Color.blue);

                for (int i = 0; i < postPath.Count - 1; i++)
                {
                    Debug.DrawLine(postPath[i], postPath[i + 1], Color.red);
                }

                // Debug.DrawLine(path[path.Count - 1].vertexCenter, endNode.position, Color.blue);
            }
        }

      // print(path.Count);
        return vector3s;
    }

    IEnumerator Move(Vector3[] vectors)
    {
        int i = 0;
        while (true)
        {       
            if (Vector3.Distance(startNode.position, vectors[vectors.Length - 1]) <= 0.2f) break;

            if (Vector3.Distance(startNode.position, vectors[i]) <= 0.2f)
            {
                i++;
            }
            else
            {
                Vector3.MoveTowards(startNode.position, vectors[i], 1f);
                yield return null;
            }

        }

    }
    float GetDistance(Vector3 nodeA, Vector3 nodeB/* , out bool obstacle*/)
    {
        // 두노드 사이의 가로방향 거리
        //int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        // 두노드 사이의 세로방향 거리
        //int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);




        float distance = Mathf.Abs(Vector3.Distance(nodeA, nodeB));

        /*  if (Physics.Raycast(nodeA, (nodeB - nodeA).normalized, distance, LayerMask.GetMask("Obstacle")))
          {
              obstacle = true;
          }
          else
          {
              obstacle = false;
          }*/

        return distance;

        /*   if (dstX > dstY)
           {
               return 14 * dstY + 10 * (dstX - dstY);
           }

           //print(14 * dstX + 10 * (dstY - dstX));

           return 14 * dstX + 10 * (dstY - dstX);*/

    }


    public List<Vector3> PostProcessing(List<Node> nodeList, Vector3 start, Vector3 end)
    {
        // Abort if list is empty.
        cross.Clear();
        apexLeftLine.Clear();
        apexRightLine.Clear();
     
        index = 0;
        if (nodeList.Count <= 0)
        {
            return new List<Vector3>() { start };
        }

        // Return immediately if list only has one entry.
        if (nodeList.Count <= 1)
        {
            return new List<Vector3>() { end };
        }
        List<Vector3> path = new List<Vector3>() { start };
        Vector3[] leftVertices = new Vector3[nodeList.Count + 1];
        Vector3[] rightVertices = new Vector3[nodeList.Count + 1];      
        Vector3 apex = start;
        int left = 1;
        int right = 1;

        // Initialise portal vertices.
        for (int i = 0; i < nodeList.Count - 1; i++)
        {
         
            for (int j = 0; j < nodeList[i].neighbours.Count; j++)
            {
                if (i > 0 && nodeList[i].neighbours[j] == nodeList[i - 1]) continue;

                if (nodeList[i].neighbours[j] == nodeList[i + 1])
                {
                    List<Vector3> potal = new List<Vector3>();
                    int indexLeft = 0;
                    int indexRight = 0;
                    for (int nexti = 0; nexti < nodeList[i].vertexs.Length; nexti++)
                    {
                        for (int nextj = 0; nextj < nodeList[i + 1].vertexs.Length; nextj++)
                        {
                            if (nodeList[i + 1].vertexs[nextj] == nodeList[i].vertexs[nexti])
                            {
                                potal.Add(nodeList[i + 1].vertexs[nextj]);
                            }                          
                        }                   
                    }                  
                    if (potal.Count == 2)
                    {
                        // 현재 포털 왼쪽 오른쪽을 구분하기 위한 중심점
                        Vector3 midPoint = new Vector3((potal[0].x + potal[1].x) / 2, (potal[0].y + potal[1].y) / 2, (potal[0].z + potal[1].z) / 2);
                        // 다음 포털을 foward방향으로 구분
                        Vector3 direction = (nodeList[i + 1].vertexCenter - midPoint).normalized;
                                    
                        indexLeft = DirectionCheck(potal[0], midPoint, direction);
                        if (indexLeft == 0)
                        {
                            indexRight = 1;
                        }
                        else if( indexLeft == 1)
                        {
                            indexRight = 0;
                        }
                        else if (indexLeft == -1)
                        {
                           print("null");
                          /*this.leftVertices = leftVertices;
                            this.rightVertices = rightVertices;*/
                            return null;
                        }


                        rightVertices[i + 1] = potal[indexRight];
                        leftVertices[i + 1] = potal[indexLeft];
                        break;
                    }
                }
            }
        }
        this.leftVertices = leftVertices;
        this.rightVertices = rightVertices;
        leftLine = new List<Vector3>();
        rightLine = new List<Vector3>();
        int next = 1;

        Vector3 leftSide = leftVertices[1] - apex;
        Vector3 rightSide = rightVertices[1] - apex;
        apexLeftLine.Add((apex, leftVertices[1]));
        apexRightLine.Add((apex, rightVertices[1]));

        // Step through channel. 
        for (int i = 2; i <= nodeList.Count - 1; i++)
        {
            // If new left vertex is different, process.
            if (i > left)
            {
                Vector3 newSide = leftVertices[i] - apex;
                apexLeftLine.Add((apex, leftVertices[i]));
                leftLine.Add(newSide);
                // If new side does not widen funnel, update.
                if (VectorSign(newSide,
                        leftVertices[left] - apex) <= 0)
                {
                    // If new side crosses other side, update apex.
                    if (VectorSign(newSide,
                        rightVertices[right] - apex) < 0)
                    {
                        // Find next vertex.
                     /*   for (int j = next; j <= nodeList.Count; j++)
                        {
                            if (rightVertices[j] != rightVertices[next])
                            {
                                next = j;
                                break;
                            }
                        }*/

                        path.Add(rightVertices[right]);
                        apex = rightVertices[right];
                        right = right + 1;
                    }
                    else
                    {
                        left = i;
                    }
                }

            }
            // If new right vertex is different, process.
            if (i > right)
            {
                Vector3 newSide = rightVertices[i] - apex;
                apexRightLine.Add((apex, rightVertices[i]));
                rightLine.Add(newSide);
                // If new side does not widen funnel, update.
                if (VectorSign(newSide,
                        rightVertices[right] - apex) >= 0)
                {
                    // If new side crosses other side, update apex.
                    if (VectorSign(newSide,
                        leftVertices[left] - apex) > 0)
                    {
                        // Find next vertex.
                     /*   for (int j = next; j <= nodeList.Count; j++)
                        {
                            if (leftVertices[j] != leftVertices[next])
                            {
                                next = j;
                                break;
                            }
                        }*/

                        path.Add(leftVertices[left]);
                        apex = leftVertices[left];
                        left = left + 1;
                    }
                    else
                    {
                        right = i;
                    }
                }
            }
        }

        path.Add(end);
        paths1 = path;
        return path;


        int VectorSign(Vector3 vector1, Vector3 vector2)
        {
            Vector3 crossProduct = Vector3.Cross(vector1, vector2);
            return crossProduct.z > 0 ? 1 : -1;
        }

             int DirectionCheck(Vector3 vertex, Vector3 midPoint, Vector3 direction)
             {
                 //Vector3 localForward = direction * Vector3.forward;
                 Vector3 midPointNomal = midPoint + direction; 
             
                 //midPointNomal = midPointNomal.normalized;
                 Vector3 vertexPoint = (vertex - midPoint).normalized;
                 Vector3 vertexPointNomal = vertex + vertexPoint;
                 Vector3 check = Vector3.Cross(direction, vertexPoint);

               //  Vector3 check = Vector3.Cross(midPointNomal, vertexPointNomal);
                 //float checkDot = Vector3.Dot(check, Vector3.up);
            
                 cross.Add((midPoint, check));

                // print($"midcheck : {check}");

                 // 결과 벡터의 y 값이 양수이면 vectorB는 vectorA의 왼쪽에 있음
                 // 결과 벡터의 y 값이 음수이면 vectorB는 vectorA의 오른쪽에 있음
                 if (check.y > 0f)
                 {
                     print($"{index}는 왼쪽입니다.");
                     print($"{index}의 위치는 { vertex}");
                     print($"{index}의 중점 방향 백터는 { midPointNomal}");
                     print($"{index}의 비교하는 위치 { vertexPointNomal}");
                     print($"{index}의 외적 결과 {check}.");
                     index++;
                     return 1;
                     //Debug.Log("벡터 B는 벡터 A의 왼쪽에 있습니다.");
                 }
                 else if (check.y < 0f)
                 {
                     print($"{index}는 오른쪽입니다."); 
                     print($"{index}의 위치는 { vertex}");
                     print($"{index}의 중점 방향 백터는 { midPointNomal}");
                     print($"{index}의 비교하는 위치 { vertexPointNomal}");
                     index++;
                     return 0;
                    // Debug.Log("벡터 B는 벡터 A의 오른쪽에 있습니다.");
                 }
                 else
                 {
                     print(check);
                     return -1;
                     //Debug.Log("벡터 B는 벡터 A와 같은 평면 상에 있습니다.");
                 }
             }
    }


    private void OnDrawGizmos()
    {
        if (leftVertices != null && leftVertices.Length > 0)
        {
            for (int i = 1; i < leftVertices.Length - 1; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(leftVertices[i], Vector3.one);
                Gizmos.color = Color.red;
                Gizmos.DrawCube(rightVertices[i], Vector3.one);

                Gizmos.color = Color.black;
                Gizmos.DrawLine(leftVertices[i], rightVertices[i]);
            }
        }

/*
          for (int i = 0; i < apexLeftLine.Count; i++)
          {
              Gizmos.color = Color.green;
              Gizmos.DrawLine(apexLeftLine[i].a , apexLeftLine[i].b);
          }

          for (int i = 0; i < apexRightLine.Count; i++)
          {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(apexRightLine[i].a, apexRightLine[i].b);
        }*/
  
    }
}


