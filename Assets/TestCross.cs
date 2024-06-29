using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCross : MonoBehaviour
{
    // 두 벡터를 정의합니다.
    public Vector3 vectorA = new Vector3(-2, 3, -4);
    public Vector3 vectorB = new Vector3(-5, 6, -7);

    void OnDrawGizmos()
    {
        // 원점 위치
        Vector3 origin = Vector3.zero;

        // 외적 벡터를 계산합니다.
        Vector3 crossProduct = Vector3.Cross(vectorB, vectorA);

        // 벡터 A를 그립니다.
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, vectorA);
        Gizmos.DrawSphere(vectorA, 0.1f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(vectorA, vectorA * 1.1f); // 화살표 효과

        // 벡터 B를 그립니다.
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin, vectorB);
        Gizmos.DrawSphere(vectorB, 0.1f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(vectorB, vectorB * 1.1f); // 화살표 효과

        // 외적 벡터를 그립니다.
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, crossProduct);
        Gizmos.DrawSphere(crossProduct, 0.1f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(crossProduct, crossProduct * 1.1f); // 화살표 효과

        // 벡터들의 텍스트 설명을 추가합니다.
        UnityEditor.Handles.Label(vectorA, " A");
        UnityEditor.Handles.Label(vectorB, " B");
        UnityEditor.Handles.Label(crossProduct, " A x B");
    }
}
