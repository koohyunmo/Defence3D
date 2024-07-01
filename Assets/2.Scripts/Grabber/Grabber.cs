using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] GridObject selectedObject;
    Vector3 originalPos = Vector3.zero;
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

        Cursor.visible = true;
        selectedObject = null;
    }

    private void MouseDrag()
    {
        if (selectedObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, .25f, worldPosition.z);
            Managers.Grid.GetGrid().GetPointToGrid(worldPosition, originalPos);

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
}