using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] Camera targetCamera = null;

    public void SetTargetCam(Camera newTarget)
    {
        this.targetCamera = newTarget; 
    }

    public void ChangeCam(Camera activeCam)
    {
        activeCam.GetComponent<Camera>().enabled = false;
        targetCamera.GetComponent<Camera>().enabled = true;
    }
}
