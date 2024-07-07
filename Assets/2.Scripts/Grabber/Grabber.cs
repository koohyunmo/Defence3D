using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] GridObject selectedObject;
    Vector3 originalPos = Vector3.zero;
    LineRenderer lineRenderer = null;

    private void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        ConfigureLineRenderer();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseButtonDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MouseButtonUp();
        }

        MouseDrag();
    }

    private void MouseButtonDown()
    {
        if (selectedObject == null)
        {
            RaycastHit hit = CastRay();

            if (hit.collider)
            {
                if (hit.collider == null)
                    return;

                //Debug.Log("Drag");
                selectedObject = hit.transform.gameObject.GetComponent<GridObject>();
                if (selectedObject)
                {
                    originalPos = selectedObject.transform.position;
                    selectedObject.Dragging();
                    Managers.Grid.GetGrid().GridCellViewerOn();
                }

            }
        }
    }

    private void MouseButtonUp()
    {
        //Debug.Log("Drop");
        if (selectedObject == null) return;

        if(Managers.Grid.Swap(selectedObject.transform.position,originalPos))
        {

        }
        else
        {
            selectedObject.transform.position = originalPos;
        }
        
        selectedObject.Drop();

        // 그리드 정보들 종료
        Cursor.visible = true;
        selectedObject = null;
        Managers.Grid.GetGrid().GridCellViewerOff();
        Managers.Effect.CloseReinforcePercent();
        lineRenderer.positionCount = 0; // Clear line
    }

    private void MouseDrag()
    {
        if (selectedObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, .25f, worldPosition.z);
            Managers.Grid.GetGrid().GetPointToGrid(worldPosition, originalPos);

            // 라인랜더러
            DrawLine(originalPos, selectedObject.transform.position);

            if (Input.GetMouseButtonDown(1))
            {
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                    selectedObject.transform.rotation.eulerAngles.x,
                    selectedObject.transform.rotation.eulerAngles.y + 90,
                    selectedObject.transform.rotation.eulerAngles.z
                    )
                );
            }

        }
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3
            (
                Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.farClipPlane
            );

        Vector3 screenMousePosNear = new Vector3
            (
                Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.nearClipPlane
            );
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;

        Debug.DrawRay(transform.position, worldMousePosFar - worldMousePosNear);

        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;

    }

    private void DrawLine(Vector3 startLine, Vector3 endLine)
    {
        lineRenderer.positionCount = 2;
        endLine.y += 1;
        lineRenderer.SetPosition(0, startLine);
        lineRenderer.SetPosition(1, endLine);
    }

    private void ConfigureLineRenderer()
    {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.receiveShadows = false;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 0;
    }
}