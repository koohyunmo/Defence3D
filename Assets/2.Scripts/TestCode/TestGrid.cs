using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    Grid grid;
    [SerializeField] TestAddUnit testAddUnit;

    void Start()
    {
        // Plane 크기 가져오기
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Vector3 planeSize = meshRenderer.bounds.size;

        // 원하는 그리드 크기 (행과 열의 수)
        int desiredWidth = 5;
        int desiredHeight = 5;

        // 각 셀의 크기 계산
        float cellSize = Mathf.Min(planeSize.x / desiredWidth, planeSize.z / desiredHeight);

        // Plane의 왼쪽 아래 모서리 위치 계산
        Vector3 planePosition = transform.position - new Vector3(planeSize.x / 2, 0, planeSize.z / 2);
        Debug.Log($"{desiredWidth} x {desiredHeight} cells of size {cellSize}");

        // 그리드 생성
        Managers.Grid.CreateGrid(desiredWidth, desiredHeight, cellSize, planePosition);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 worldPosition = hit.point;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 worldPosition = hit.point;
                Managers.Grid.RemoveGridObject(worldPosition);
            }
        }
    }
}
