using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    void Start()
    {
        //Enables Cursor
        Cursor.visible = true;

        //Disable all Cams on Start & reactivates MainCamera
        foreach (Camera cam in Camera.allCameras)
        {
            cam.GetComponent<Camera>().enabled = false;
        }
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;

        ProductionLists.CreateProductionLists();

    }
}
