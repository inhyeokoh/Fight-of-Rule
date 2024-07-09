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

    private bool endNodePosition;
    public Material red, blue;

    public MeshFilter meshFilter;

    float time = 1.5f;

    public bool funnel;
    private Vector3 targetPosition = Vector3.zero;

    Renderer rd1;
    Renderer rd2;

    Vector3[] _vec1;
    Vector3[] _vec2;

    RaycastHit hit;

    //[SerializeField]
   // List<Node> nodes;
    Vector3[] paths;
    //[SerializeField]
    //List<(Vector3 a, Vector3 b)> cross = new List<(Vector3 a, Vector3 b)>();
    public bool triangleOn;

   /* [SerializeField]
    Vector3[] leftVertices;
    [SerializeField]
    Vector3[] rightVertices;*/
    [SerializeField]
    List<Vector3> paths1;
    LineRenderer lr1;
    LineRenderer lr2;

   // List<(Vector3 a, Vector3 b)> apexLeftLine = new List<(Vector3 a, Vector3 b)>();
    //List<(Vector3 a, Vector3 b)> apexRightLine = new List<(Vector3 a, Vector3 b)>();

    //List<(Vector3 a, Vector3 b, string c)> greenLine = new List<(Vector3 a, Vector3 b, string c)>();


    List<Node> path;
   /* [SerializeField]
    List<Vector3> leftLine;
    [SerializeField]
    List<Vector3> rightLine;*/
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
    public Vector3[] FindPath(List<Node> nodePath, List<(Node nodeA, Node nodeB)> redPath)
    {
        if (!endNodePosition || targetPosition != endNode.position)
        {
            targetPosition = endNode.position;
            endNodePosition = true;
        }
        else
        {
           // print("리턴중");
            return null;
        }

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
            paths = RetracePath(startNode, targetNode);

          /*  if (paths.Length > 0)
            {
                Debug.DrawLine(this.startNode.position, path[0].vertexCenter, Color.blue);

                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(path[i].vertexCenter, path[i + 1].vertexCenter, Color.blue);
                }

                Debug.DrawLine(path[path.Count - 1].vertexCenter, endNode.position, Color.blue);
            }*/

            // print($"{startNode.vertexOne} {startNode.vertexTwo} {startNode.vertexThree}");
            //  print("succes");

            return paths;
        }
        else
        {
            print("fail");
            return null;
        }
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
       
        List<Node> triangleNodes = new List<Node>();
        Node currentNode = endNode;
       // path.Add(endNode);
        triangleNodes.Add(endNode);
        while (currentNode != startNode)
        {
            if (currentNode == endNode)
            {
                currentNode = currentNode.parent;
                continue;
            }
            path.Add(currentNode);
            triangleNodes.Add(currentNode);
            //nodePath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        triangleNodes.Add(startNode);
       // path.Add(startNode);

      //  path.Reverse();
        triangleNodes.Reverse();

        if (triangleOn)
        {
            for (int i = 0; i < triangleNodes.Count; i++)
            {
                Debug.DrawLine(triangleNodes[i].vertexOne, triangleNodes[i].vertexTwo, Color.green);
                Debug.DrawLine(triangleNodes[i].vertexTwo, triangleNodes[i].vertexThree, Color.green);
                Debug.DrawLine(triangleNodes[i].vertexThree, triangleNodes[i].vertexOne, Color.green);
            }

        }

        Vector3[] vector3s = new Vector3[path.Count + 2];
  //      vector3s[0] = this.startNode.position;

   /*     for (int i = 1; i < vector3s.Length - 1; i++)
        {
            vector3s[i] = path[i - 1].vertexCenter;
        }
*/
    //    vector3s[vector3s.Length - 1] = this.endNode.position;
        //  StartCoroutine(Move(vector3s));
     
        List<Vector3> postPath = PostProcessing(triangleNodes, this.startNode.position, this.endNode.position);
        vector3s = postPath.ToArray();
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

        if (nodeList.Count <= 0)
        {
            return null;
        }

        // 삼각형 노드 하나 안에 start end가 다 있을경우
        if (nodeList.Count <= 2)
        {
            paths1 = new List<Vector3>() { start };
            paths1.Add(end);
            return paths1;
        }
        List<Vector3> path = new List<Vector3>() { start };

        Vector3[] leftVertices = new Vector3[nodeList.Count];
        Vector3[] rightVertices = new Vector3[nodeList.Count];
        Vector3 apex = start;
        int left = 0;
        int right = 0;

        // 포탈 정점 초기화
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
                        else if (indexLeft == 1)
                        {
                            indexRight = 0;
                        }
                        else if (indexLeft == -1)
                        {
                            print("null");
                            return null;
                        }


                        rightVertices[i] = potal[indexRight];
                        leftVertices[i] = potal[indexLeft];
                        break;
                    }
                }
            }
        }

        leftVertices[leftVertices.Length - 1] = end;
        rightVertices[rightVertices.Length - 1] = end;

        // 퍼널 탐색 시작     
        for (int i = 1; i <= nodeList.Count; i++)
        {
            // 왼쪽 포털 검사       
            if (i <= leftVertices.Length)
            {
                // 마지막에 왼쪽 오른쪽 포털의 선분들이 겹치지 않아  업데이트가 안되고 타겟에게 도착할 경우           
                if (i == nodeList.Count)

                {
                    if (end == leftVertices[left])
                    {
                        path.Add(rightVertices[right]);
                        apex = rightVertices[right];
                        right = right + 1;
                        i = right + 1;
                    }
                }
                else
                {
                    //현재 에이펙스 지점과 다음 포털까지의 선분
                    Vector3 newSide = leftVertices[i] - apex;
                    // 현재 포털 꼭지점과 다음 포털 꼭지점이 다르면 퍼널 검사 시작 아니면 현재 포털을 다음포털로 바꾸고 건너뜀                 
                    if (leftVertices[left] != leftVertices[i])
                    {
                        // 다음 포털이 왼쪽 포털보다 더 오른쪽에 있을경우(현재 에이펙스랑 이어진 왼쪽 오른쪽 포털 사이에 있을경우)
                        if (SegmentSearch(newSide, leftVertices[left] - apex))
                        {
                            // 현재 에이펙스에서 다음 포털까지의 선분이 현재 오른쪽 포탈 선분보다 오른쪽에 있을경우(교차할경우) 에이펙스 업데이트
                            if (SegmentSearch(newSide, rightVertices[right] - apex))
                            {
                                if (apex != rightVertices[right])
                                {
                                    path.Add(rightVertices[right]);
                                    apex = rightVertices[right];
                                }
                                right = right + 1;
                                left = right;
                                i = right + 1;
                            }
                            else
                            {
                                left = i;
                            }
                        }
                    }
                    else
                    {
                        left = i;
                    }
                }
            }
            // 오른쪽 포털 검사
            if (i <= rightVertices.Length)
            {
                if (i == nodeList.Count)
                {
                    // 마지막에 왼쪽 오른쪽 포털의 선분들이 겹치지 않아(교차되지 않아) 업데이트가 안되고 타겟에게 도착할 경우
                    if (end == rightVertices[right])
                    {
                        path.Add(rightVertices[right]);
                        apex = rightVertices[right];
                        right = right + 1;
                        i = right + 1;
                    }

                }
                else
                {
                    Vector3 newSide = rightVertices[i] - apex;
                    if (rightVertices[right] != rightVertices[i])
                    {
                        if (!SegmentSearch(newSide, rightVertices[right] - apex))
                        {
                            if (!SegmentSearch(newSide, leftVertices[left] - apex))
                            {
                                if (apex != leftVertices[left])
                                {
                                    path.Add(leftVertices[left]);
                                    apex = leftVertices[left];
                                }
                                left = left + 1;
                                right = left;
                                i = left + 1;
                            }
                            else
                            {
                                right = i;
                            }
                        }
                    }
                    else
                    {
                        right = i;
                    }
                }
            }
        }

        if (path[path.Count - 1] != end)
        {
            path.Add(end);
        }
        paths1 = path;
        return path;

        bool SegmentSearch(Vector3 vector1, Vector3 vector2)
        {

            Vector3 crossProduct = Vector3.Cross(vector1, vector2);

            if (crossProduct.y < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int DirectionCheck(Vector3 vertex, Vector3 midPoint, Vector3 direction)
        {
            Vector3 vertexPoint = (vertex - midPoint).normalized;
            Vector3 check = Vector3.Cross(direction, vertexPoint);
            // 결과 벡터의 y 값이 양수이면 vectorB는 vectorA의 왼쪽에 있음
            // 결과 벡터의 y 값이 음수이면 vectorB는 vectorA의 오른쪽에 있음
            if (check.y > 0f)
            {
                return 1;
            }
            else if (check.y < 0f)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
    /*  public List<Vector3> PostProcessing(List<Node> nodeList, Vector3 start, Vector3 end)
      {

          if (nodeList.Count <= 0)
          {
              return null;
          }

          // 삼각형 노드 하나 안에 start end가 다 있을경우
          if (nodeList.Count <= 2)
          {
              paths1 = new List<Vector3>() { start };
              paths1.Add(end);
              return paths1;
          }
          List<Vector3> path = new List<Vector3>() { start };

          Vector3[] leftVertices = new Vector3[nodeList.Count];
          Vector3[] rightVertices = new Vector3[nodeList.Count];
          Vector3 apex = start;
          int left = 0;
          int right = 0;

          // 포탈 정점 초기화
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
                          else if (indexLeft == 1)
                          {
                              indexRight = 0;
                          }
                          else if (indexLeft == -1)
                          {
                              print("null");
                              return null;
                          }


                          rightVertices[i] = potal[indexRight];
                          leftVertices[i] = potal[indexLeft];
                          break;
                      }
                  }
              }
          }

          leftVertices[leftVertices.Length - 1] = end;
          rightVertices[rightVertices.Length - 1] = end;

          // 퍼널 탐색 시작     
          for (int i = 1; i <= nodeList.Count; i++)
          {
              // 왼쪽 포털 검사       
              if (i <= leftVertices.Length)
              {
                  // 마지막에 왼쪽 오른쪽 포털의 선분들이 겹치지 않아  업데이트가 안되고 타겟에게 도착할 경우           
                  if (i == nodeList.Count)

                  {
                      if (end == leftVertices[left])
                      {
                          path.Add(rightVertices[right]);
                          apex = rightVertices[right];
                          right = right + 1;
                          i = right + 1;
                      }
                  }
                  else
                  {
                      //현재 에이펙스 지점과 다음 포털까지의 선분
                      Vector3 newSide = leftVertices[i] - apex;
                      // 현재 포털 꼭지점과 다음 포털 꼭지점이 다르면 퍼널 검사 시작 아니면 현재 포털을 다음포털로 바꾸고 건너뜀                 
                      if (leftVertices[left] != leftVertices[i])
                      {
                          // 다음 포털이 왼쪽 포털보다 더 오른쪽에 있을경우(현재 에이펙스랑 이어진 왼쪽 오른쪽 포털 사이에 있을경우)
                          if (SegmentSearch(new Vector3(newSide.x,0,newSide.z), new Vector3((leftVertices[left] - apex).x,0, (leftVertices[left] - apex).z)))
                          {    
                              // 현재 에이펙스에서 다음 포털까지의 선분이 현재 오른쪽 포탈 선분보다 오른쪽에 있을경우(교차할경우) 에이펙스 업데이트
                              if (SegmentSearch(new Vector3(newSide.x, 0, newSide.z), new Vector3((rightVertices[right] - apex).x, 0, (rightVertices[right] - apex).z)))
                              {
                                  if (apex != rightVertices[right])
                                  {
                                      path.Add(rightVertices[right]);
                                      apex = rightVertices[right];
                                  }
                                  right = right + 1;
                                  left = right;
                                  i = right + 1;
                              }
                              else
                              {
                                  left = i;
                              }
                          }
                      }
                      else
                      {
                          left = i;
                      }
                  }
              }
              // 오른쪽 포털 검사
              if (i <= rightVertices.Length)
              {
                  if (i == nodeList.Count)
                  {
                      // 마지막에 왼쪽 오른쪽 포털의 선분들이 겹치지 않아(교차되지 않아) 업데이트가 안되고 타겟에게 도착할 경우
                      if (end == rightVertices[right])
                      {
                          path.Add(rightVertices[right]);
                          apex = rightVertices[right];
                          right = right + 1;
                          i = right + 1;
                      }

                  }
                  else
                  {
                      Vector3 newSide = rightVertices[i] - apex;
                      if (rightVertices[right] != rightVertices[i])
                      {
                          if (SegmentSearch(new Vector3(newSide.x, 0, newSide.z), new Vector3((rightVertices[right] - apex).x, 0, (rightVertices[right] - apex).z)))
                          {
                              if (SegmentSearch(new Vector3(newSide.x, 0, newSide.z), new Vector3((leftVertices[left] - apex).x, 0, (leftVertices[left] - apex).z)))
                              {
                                  if (apex != leftVertices[left])
                                  {
                                      path.Add(leftVertices[left]);
                                      apex = leftVertices[left];
                                  }
                                  left = left + 1;
                                  right = left;
                                  i = left + 1;
                              }
                              else
                              {
                                  right = i;
                              }
                          }
                      }
                      else
                      {
                          right = i;
                      }
                  }
              }
          }

          if (path[path.Count - 1] != end)
          {
              path.Add(end);
          }
          paths1 = path;
          return path;

          bool SegmentSearch(Vector3 vector1, Vector3 vector2)
          {

              Vector3 crossProduct = Vector3.Cross(vector1, vector2);

              if (crossProduct.y < 0)
              {
                  return true;
              }
              else
              {
                  return false;
              }
          }

          int DirectionCheck(Vector3 vertex, Vector3 midPoint, Vector3 direction)
          {
              Vector3 vertexPoint = (vertex - midPoint).normalized;
              Vector3 check = Vector3.Cross(direction, vertexPoint);
              // 결과 벡터의 y 값이 양수이면 vectorB는 vectorA의 왼쪽에 있음
              // 결과 벡터의 y 값이 음수이면 vectorB는 vectorA의 오른쪽에 있음
              if (check.y > 0f)
              {
                  return 1;
              }
              else if (check.y < 0f)
              {
                  return 0;
              }
              else
              {
                  return -1;
              }
          }
      }*/

    private void OnDrawGizmos()
    {
       /* if (leftVertices != null && leftVertices.Length > 0)
        {
            Vector3 origin = Vector3.zero;
            Vector3 A = leftVertices[1] - startNode.position;
            Vector3 B = leftVertices[6] - startNode.position;
            Vector3 cross = Vector3.Cross(A, B);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, A);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(origin, B);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, cross);

            print($"cross : {cross}");

            UnityEditor.Handles.Label(leftVertices[1], $"{1}");
            UnityEditor.Handles.Label(leftVertices[2], $"{2}");
            UnityEditor.Handles.Label(cross, $"{cross}");


            for (int i = 1; i < leftVertices.Length - 1; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(leftVertices[i], Vector3.one);
                Gizmos.color = Color.red;
                Gizmos.DrawCube(rightVertices[i], Vector3.one);

                Gizmos.color = Color.black;
                Gizmos.DrawLine(leftVertices[i], rightVertices[i]);

                UnityEditor.Handles.Label(leftVertices[i], $"{leftVertices[i]}");
                UnityEditor.Handles.Label(rightVertices[i], $"{rightVertices[i]}");

            }*/

            /* for (int i = 0; i < apexLeftLine.Count; i++)
             {
                 Gizmos.color = Color.cyan;
                 Gizmos.DrawLine(apexLeftLine[i].a, apexLeftLine[i].b);
             }

             for (int i = 0; i < apexRightLine.Count; i++)
             {
                 Gizmos.color = Color.yellow;
                 Gizmos.DrawLine(apexRightLine[i].a, apexRightLine[i].b);
             }

             for (int i = 0; i < greenLine.Count; i++)
             {
                 Gizmos.color = Color.green;
                 Gizmos.DrawLine(greenLine[i].a, greenLine[i].b);

                 Vector3 mid = (greenLine[i].a + greenLine[i].b) / 2;

                 UnityEditor.Handles.Label(mid, greenLine[i].c);

             }*/

            for (int i = 0; i < paths1.Count; i++)
            {
                Gizmos.color = Color.green;

                Gizmos.DrawCube(paths1[i], Vector3.one);
            }
        }
    }



      


