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

    Renderer rd1;
    Renderer rd2;

    Vector3[] _vec1;
    Vector3[] _vec2;

    RaycastHit hit;

    LineRenderer lr1;
    LineRenderer lr2;

    List<Node> path;

    public Grid grid;

    public bool neighborusRedLine;
    public bool fastRedLine;

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
     public void FindPath()
     {
        Node startNode = grid.FindNode(this.startNode.position);
        if (startNode == null)
        {          
            return;
        }
        Node targetNode = grid.FindNode(this.endNode.position);
        if (targetNode == null)
        {
            return;
        }      
     /*   _vec1 = new Vector3[] { startNode.vertexOne, startNode.vertexTwo, startNode.vertexThree };
        meshFilter.mesh.vertices = _vec1;
        meshFilter.mesh.triangles = new int[]
        {
            0, 1, 2
        };*/
        //print($"{targetNode.vertexOne} {targetNode.vertexTwo} {targetNode.vertexThree}");

        bool succes = false;           
        Heap<Node> openSet = new Heap<Node>(grid.vertes.Length * 100);

         
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
            foreach (Node neighbours in grid.GetNeighborus(currentNode))           
            {          
                if (closeSet.Contains(neighbours))           
                    continue;

                List<Vector3> neighboursVertexs = new List<Vector3>();

                for (int i = 0; i < neighbours.vertexs.Length; i++)
                {
                    //bool overlap = false;

                   /* for (int j = 0; j < currentNode.vertexs.Length; j++)
                    {
                        if (neighbours.vertexs[i] == currentNode.vertexs[j])
                        {
                            overlap = true;
                            break;
                        }
                    }

                    if (!overlap)
                    {*/
                        neighboursVertexs.Add(neighbours.vertexs[i]);
                    //}
                }

                for (int i = 0; i < currentNode.vertexs.Length; i++)
                {
                    for (int j = 0; j < neighboursVertexs.Count; j++)
                    {
                        //bool obstacle = false;

                        float newNeighboursPath = currentNode.gCost + GetDistance(currentNode.vertexs[i], neighboursVertexs[j]/*, out obstacle*/);

                       /* if (obstacle)
                        {
                            continue;
                        }*/

                        float newHuristicPath = GetDistance(neighboursVertexs[j], endNode.position/*, out obstacle*/);

                        if (/*!obstacle &&*/ (newNeighboursPath + newHuristicPath) < neighbours.fCost || !openSet.Contains(neighbours))
                        {
                            neighbours.gCost = newNeighboursPath;
                            neighbours.hCost = newHuristicPath;
                            neighbours.parent = currentNode;

                            currentNode.currentVertex = currentNode.vertexs[i];

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
            }     
        }       
        if (succes)
        {
           path = RetracePath(startNode, targetNode);
            print($"{startNode.vertexOne} {startNode.vertexTwo} {startNode.vertexThree}");
            print("succes");
        }
        else
        {
            print("fail");
        }
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {

        List<Node> path = new List<Node>();
        Node currentNode = endNode;



        while (currentNode != startNode)
        {
            path.Add(currentNode);

            currentNode = currentNode.parent;
        }

        path.Add(startNode);

        path.Reverse();
        Vector3[] vector3s = new Vector3[path.Count];

        for (int i = 0; i < vector3s.Length; i++)
        {
            vector3s[i] = path[i].currentVertex;
        }

        StartCoroutine(Move(vector3s));

        print(path.Count);
        return path;
        /*for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i].worldPosition, path[i + 1].worldPosition);
        }     */
    }

    IEnumerator Move(Vector3[] vectors)
    {
        int i = 0;
        while (true)
        {
            Debug.Log("간다잇!!");
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

    void OnDrawGizmos()
    {
        Node startNode = grid.FindNode(this.startNode.position);
        if (startNode == null)
        {
            return;
        }
        Node targetNode = grid.FindNode(this.endNode.position);
        if (targetNode == null)
        {
            return;
        }
        /*   _vec1 = new Vector3[] { startNode.vertexOne, startNode.vertexTwo, startNode.vertexThree };
           meshFilter.mesh.vertices = _vec1;
           meshFilter.mesh.triangles = new int[]
           {
               0, 1, 2
           };*/
        //print($"{targetNode.vertexOne} {targetNode.vertexTwo} {targetNode.vertexThree}");

        bool succes = false;
        Heap<Node> openSet = new Heap<Node>(grid.vertes.Length * 100);


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
            foreach (Node neighbours in grid.GetNeighborus(currentNode))
            {
                if (closeSet.Contains(neighbours))
                    continue;

                List<Vector3> neighboursVertexs = new List<Vector3>();

                for (int i = 0; i < neighbours.vertexs.Length; i++)
                {
                 //   bool overlap = false;

                 /*   for (int j = 0; j < currentNode.vertexs.Length; j++)
                    {
                        if (neighbours.vertexs[i] == currentNode.vertexs[j])
                        {
                            overlap = true;
                            break;
                        }
                    }*/

                   // if (!overlap)
                  //  {
                        neighboursVertexs.Add(neighbours.vertexs[i]);
                  //  }
                }

                for (int i = 0; i < currentNode.vertexs.Length; i++)
                {
                    for (int j = 0; j < neighboursVertexs.Count; j++)
                    {
                    //    bool obstacle = false;
                           
                        float newNeighboursPath = currentNode.gCost + GetDistance(currentNode.vertexs[i], neighboursVertexs[j] /*out obstacle*/);

                       /* if (obstacle)
                        {
                            continue;
                        }*/
                            
                        float newHuristicPath = GetDistance(neighboursVertexs[j], endNode.position/*out obstacle*/);
                       
                        if (/*!obstacle &&*/ (newNeighboursPath + newHuristicPath) < neighbours.fCost || !openSet.Contains(neighbours))
                        {
                            if (fastRedLine)
                            {
                                Debug.DrawLine(currentNode.vertexs[i], neighboursVertexs[j], Color.red);
                            }
                            neighbours.gCost = newNeighboursPath;
                            neighbours.hCost = newHuristicPath;
                            neighbours.parent = currentNode;

                            currentNode.currentVertex = currentNode.vertexs[i];

                            if (!openSet.Contains(neighbours))
                            {
                                openSet.Add(neighbours);
                            }
                            else
                            {
                                openSet.UpdateItem(neighbours);
                            }
                        }
                        else 
                        {
                            if (neighborusRedLine)
                            {
                                Debug.DrawLine(currentNode.vertexs[i], neighboursVertexs[j], Color.red);
                            }
                        }

                    }
                }
            }
        }
        if (succes)
        {
            path = RetracePath(startNode, targetNode);
            print($"{startNode.vertexOne} {startNode.vertexTwo} {startNode.vertexThree}");
            print("succes");
        }
        else
        {
            print("fail");
        }

        if (path.Count > 0)
        {
            Debug.DrawLine(this.startNode.position, path[0].currentVertex,Color.blue);

            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i].currentVertex, path[i + 1].currentVertex, Color.blue);
            }

            Debug.DrawLine(path[path.Count - 1].currentVertex, endNode.position, Color.blue);
        } 
    }

    private void OnDrawGizmosSelected()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            print("");

            
        } 
    }




    /* Vector3[] SimplifyPath(List<Node> path)
     {
         List<Vector3> waypoints = new List<Vector3>();
         Vector2 directionOld = Vector2.zero;

         for (int i = 1; i < path.Count; i++)
         {
             Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
             if (directionNew != directionOld)
             {
                 waypoints.Add(path[i].worldPosition);
             }

             directionOld = directionNew;
         }

         return waypoints.ToArray();
     }*/

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
}
