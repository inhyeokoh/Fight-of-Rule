using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    //이 노드에 장애물이 있는지
    public bool walkble;

    //현재 노드의 월드 포지션
    public Vector3 worldPosition;

    public int gCost;
    public int hCost;

    //현재 노드의 전 위치 노드 부모
    public Node parent;

    //우선 순위 큐 최단경로를 찾기위한
    int heapIndex;



    public Node(Vector3 _worldPos)
    {       
        worldPosition = _worldPos;     
    }


    //최단 경로 비용
    public int fCost
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

}

