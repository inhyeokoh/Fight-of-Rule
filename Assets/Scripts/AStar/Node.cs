using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class Node : IHeapItem<Node>
{
    //이 노드에 장애물이 있는지
    public bool walkble;

    //현재 노드의 삼각형 정점
    public Vector3 vertexOne;
    public Vector3 vertexTwo;
    public Vector3 vertexThree;

    public Vector3 vertexCenter;

    public int number;

    //public Vector3 currentVertex;


    public Vector3[] vertexs;

    public float gCost;
    public float hCost;

    //현재 노드의 전 위치 노드 부모
    public Node parent;
    public List<Node> neighbours = new List<Node>();

    //우선 순위 큐 최단경로를 찾기위한
    int heapIndex;



    public Node(Vector3 _vertexOne, Vector3 _vertexTwo, Vector3 _vertexThree, int _number)
    {
        vertexOne = _vertexOne;
        vertexTwo = _vertexTwo;
        vertexThree = _vertexThree;
        number = _number;
        vertexCenter = CalculatePolygonCenteroid(vertexOne, vertexTwo, vertexThree);

        vertexs = new Vector3[] { vertexOne, vertexTwo, vertexThree};
    }


    //최단 경로 비용
    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    //우선순위큐 인덱스
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    //현재 인접해있는 최단경로 비교
    public int CompareTo(Node nodeToCompare)
    {
        // 어떤 노드가 더 h + g가 합쳐진 거리가 더 빠른지
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        // 만약에 같으면 누가더 도착지점에서 현재지점이 거리가 더 빠른지
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }


   /*   Vector3 CalculatePolygonCenteroid(params Vector3[] pList)
      {
          Vector3 vCenter = Vector3.one;

          float sum = 0;
          float temp_x = 0;
          float temp_y = 0;
          float temp_z = 0;

          for (int i = 0; i < pList.Length; i++)
          {
              int j = i + 1;

              if (i + 1 >= pList.Length)
              {
                  j = 0;
              }   // 마지막 항은 다각형의 첫번째 점과 이어야 하므로 따로 처리해줍니다.

              float temp = (pList[i].x * pList[j].z) - (pList[j].x * pList[i].z);


              temp_x += (pList[i].x + pList[j].x) * temp;
              temp_z += (pList[i].z + pList[j].z) * temp;

              sum += temp;
          }

          float temp_sum = (1.0f / sum);

          temp_x *= temp_sum;  
          temp_z *= temp_sum;

          vCenter = new Vector3(temp_x / 3f, 0, temp_z / 3f);

          return vCenter;
      }*/

    Vector3 CalculatePolygonCenteroid(params Vector3[] pList)
    {
        Vector3 vCenter = Vector3.one;
     
        float temp_x = 0;
        float temp_y = 0;
        float temp_z = 0;
       
        temp_x = (pList[0].x + pList[1].x + pList[2].x);          
        temp_y = (pList[0].y + pList[1].y + pList[2].y);        
        temp_z = (pList[0].z + pList[1].z + pList[2].z);                  
       
        vCenter = new Vector3(temp_x / 3f, temp_y /3f, temp_z / 3f);

        return vCenter;
    }

    /* Vector3 CalculatePolygonCenteroid(params Vector3[] pList)
     {
         Vector3 vCenter = Vector3.one;
         float sum = 0;
         float temp_x = 0;
         float temp_y = 0;
         float temp_z = 0;

         for (int i = 0; i < pList.Length; i++)
         {
             int j = i + 1;

             if (i + 1 >= pList.Length)
             {
                 j = 0;
             }

             float temp = (pList[i].x * pList[j].z) - (pList[j].x * pList[i].z);
           *//*  temp += (pList[i].z * pList[j].y) - (pList[j].z * pList[i].y);
             temp += (pList[i].y * pList[j].x) - (pList[j].y * pList[i].x);*//*

             temp_x += (pList[i].x + pList[j].x) * temp;
             temp_y += (pList[i].y + pList[j].y) * temp; // y 값 합산
             temp_z += (pList[i].z + pList[j].z) * temp;

             sum += temp;
         }

         float temp_sum = 1.0f / sum;

         temp_x *= temp_sum;
         temp_y *= temp_sum; // y 값도 정규화
         temp_z *= temp_sum;

         vCenter = new Vector3(temp_x / 3f, temp_y / 3f, temp_z / 3f); // y 값도 중점에 반영

         return vCenter;
     }*/
}

