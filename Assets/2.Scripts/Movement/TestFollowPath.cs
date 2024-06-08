using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFollowPath : MonoBehaviour
{
    private Vector3[] pathPoints;
    public float speed = 2.0f; // 이동 속도
    private int currentPointIndex = 0;

    public void SetPosition()
    {
        // PathPointsGenerator 스크립트로부터 꼭지점을 가져옵니다.
        pathPoints = Managers.Grid.GetPath();
        transform.position = pathPoints[0];
        currentPointIndex = 0;
    }

    void Update()
    {
        // 경로 포인트가 있는지 확인
        if (pathPoints.Length == 0) return;

        // 현재 목표 포인트
        Vector3 targetPoint = pathPoints[currentPointIndex];

        // 목표 포인트로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

        // 목표 포인트에 도달했는지 확인
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            // 다음 포인트로 이동
            currentPointIndex = (currentPointIndex + 1) % pathPoints.Length;
        }
    }
}
