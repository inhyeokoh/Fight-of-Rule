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

    public Grid grid;

    public void FindPath()
    {
        Node startNode = new Node(this.startNode.position);
        Node targetNode = new Node(this.endNode.position);
    
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
           
                int newNeighboursPath = currentNode.gCost + GetDistance(currentNode, neighbours);             
                
                if (newNeighboursPath <= neighbours.gCost || !openSet.Contains(neighbours))            
                {               
                    neighbours.gCost = newNeighboursPath;               
                    neighbours.hCost = GetDistance(neighbours, targetNode);               
                    neighbours.parent = currentNode;
                 
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
          RetracePath(startNode, targetNode);
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {

        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);

            currentNode = currentNode.parent;
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i].worldPosition, path[i + 1].worldPosition);
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

    int GetDistance(Node nodeA, Node nodeB)
    {
        // 두노드 사이의 가로방향 거리
        //int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        // 두노드 사이의 세로방향 거리
        //int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        int distance = (int)Mathf.Abs(Mathf.Round(Vector3.Distance(nodeA.worldPosition,nodeB.worldPosition)));

        return distance;

     /*   if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }

        //print(14 * dstX + 10 * (dstY - dstX));

        return 14 * dstX + 10 * (dstY - dstX);*/
    }
}
