using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastToCursor : MonoBehaviour
{
    void DoRayCast()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.allCameras[0].ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            DebugClick(raycastHit);
            // Calls onClick on hitGameObject
            ClickedOn(raycastHit);
        }
    }

    void ClickedOn(RaycastHit raycastHit)
    {
        Transform objectHit = raycastHit.transform;
        if (objectHit.GetComponent<BuildingMovement>())
        {
            BuildingMovement buildingMovement = objectHit.gameObject.GetComponent<BuildingMovement>();
            buildingMovement.OnClick();
        }
        else if (objectHit.GetComponent<Interactable>())
        {
            Interactable interactable = objectHit.gameObject.GetComponent<Interactable>();
            interactable.OnClick();
        }
    }


    void DebugClick(RaycastHit raycastHit)
    {
        //Debug for Clicking on GameObject
        Transform objectHit = raycastHit.transform;
        Debug.Log("You clicked on " + objectHit.gameObject.ToString());
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DoRayCast();
        }
    }
}
