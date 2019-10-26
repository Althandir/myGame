using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChangeCamera))]

public class BuildingMovement : MonoBehaviour
{
    // Values for MouseOver
    [Header("DEBUG_VALUES FOR MOUSEOVER")]
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Color initColor;

    void Awake()
    {
        //setValues for OnMouseOver
        meshRenderer = this.GetComponent<MeshRenderer>();
        initColor = meshRenderer.material.color;
    }

    public void OnClick()
    {
        // EnterBuilding / leave Structure
        this.gameObject.GetComponent<ChangeCamera>().ChangeCam(Camera.allCameras[0]);
    }

    // TEST
    void OnMouseOver()
    {
        meshRenderer.material.color = Color.yellow;
    }

    void OnMouseExit()
    {
        meshRenderer.material.color = initColor;
    }
}
