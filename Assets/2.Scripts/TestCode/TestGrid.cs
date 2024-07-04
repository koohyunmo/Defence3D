using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    [SerializeField] TestAddUnit testAddUnit;
    [SerializeField] GameObject plane;
    private Vector3[] corners = new Vector3[4];


    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        // Plane의 MeshRenderer를 가져옵니다.
        MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();

        // Plane의 실제 크기 가져오기 (로컬 스케일을 반영)
        Vector3 planeSize = Vector3.Scale(meshRenderer.bounds.size, plane.transform.localScale);

        // 원하는 그리드 크기 (행과 열의 수)
        int desiredWidth = 5;
        int desiredHeight = 5;

        // 각 셀의 크기 계산
        float cellSize = Mathf.Min(planeSize.x / desiredWidth, planeSize.z / desiredHeight);

        // Plane의 왼쪽 아래 모서리 위치 계산
        Vector3 planePosition = plane.transform.position - new Vector3(planeSize.x / 2, 0, planeSize.z / 2);
        Debug.Log($"{desiredWidth} x {desiredHeight} cells of size {cellSize}");

        // 그리드 생성 (임의의 그리드 생성 함수 호출)
        Managers.Grid.CreateGrid(desiredWidth, desiredHeight, cellSize, planePosition);

        Vector3 size = meshRenderer.bounds.size;
        Vector3 halfSize = size / 2.0f;

        // Plane의 World Position을 기준으로 꼭지점을 구합니다.
        corners[0] = plane.transform.position + new Vector3(-halfSize.x, 0, -halfSize.z);
        corners[1] = plane.transform.position + new Vector3(halfSize.x, 0, -halfSize.z);
        corners[2] = plane.transform.position + new Vector3(halfSize.x, 0, halfSize.z);
        corners[3] = plane.transform.position + new Vector3(-halfSize.x, 0, halfSize.z);

        // 꼭지점 확인 로그
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] *= 1.2f;
            Debug.Log($"Corner {i}: {corners[i]}");
        }
        Managers.Grid.SetPath(corners);

       //StartCoroutine(Start_2());
    }


    IEnumerator Start_2()
    {
        yield return new WaitForSeconds(1f);

        // Plane의 MeshRenderer를 가져옵니다.
        MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();

        // Plane의 실제 크기 가져오기 (로컬 스케일을 반영)
        Vector3 planeSize = Vector3.Scale(meshRenderer.bounds.size, plane.transform.localScale);

        // 원하는 그리드 크기 (행과 열의 수)
        int desiredWidth = 5;
        int desiredHeight = 5;

        // 각 셀의 크기 계산
        float cellWidth = planeSize.x / desiredWidth;
        float cellHeight = planeSize.z / desiredHeight;

        // Plane의 왼쪽 아래 모서리 위치 계산
        Vector3 startPosition = plane.transform.position - new Vector3(planeSize.x / 2, 0, planeSize.z / 2);

        List<CombineInstance> combineInstances = new List<CombineInstance>();

        // 그리드를 생성하여 배치합니다.
        for (int x = 0; x < desiredWidth; x++)
        {
            for (int z = 0; z < desiredHeight; z++)
            {
                Vector3 cellPosition = startPosition + new Vector3(x * cellWidth + cellWidth / 2, 0, z * cellHeight + cellHeight / 2);
                GameObject quad = CreateQuad(cellPosition, cellWidth, cellHeight);

                MeshFilter meshFilter = quad.GetComponent<MeshFilter>();
                CombineInstance combineInstance = new CombineInstance
                {
                    mesh = meshFilter.sharedMesh,
                    transform = meshFilter.transform.localToWorldMatrix
                };
                combineInstances.Add(combineInstance);

                Destroy(quad); // 병합 후 개별 쿼드는 삭제합니다.
            }
        }

        // 병합된 메쉬 생성
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances.ToArray(), true, true);

        // 병합된 메쉬를 표시할 게임 오브젝트 생성
        GameObject combinedObject = new GameObject("CombinedMesh");
        MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
        MeshRenderer combinedMeshRenderer = combinedObject.AddComponent<MeshRenderer>();

        combinedMeshFilter.mesh = combinedMesh;
        combinedMeshRenderer.material = meshRenderer.material;

        // 스테틱 메쉬로 설정
        combinedObject.isStatic = true;
    }

    // 쿼드를 생성하고 위치를 설정하는 함수입니다.
    GameObject CreateQuad(Vector3 position, float width, float height)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.position = position;
        quad.transform.rotation = Quaternion.Euler(90, 0, 0); // 쿼드를 바닥에 맞도록 회전합니다.
        quad.transform.localScale = new Vector3(width, height, 1);
        return quad;
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
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (corners[0] != Vector3.zero)
        {
            for (int i = 0; i < corners.Length; i++)
            {
                Gizmos.DrawWireCube(corners[i], Vector3.one);

                Handles.Label(corners[i], $"Path[{i}]");

            }
        }
    }
#endif
}
