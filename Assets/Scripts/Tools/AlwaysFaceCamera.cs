using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    /* This script is to make elements to look the camera currently being used */

    public Camera targetCamera;

    private void Awake()
    {
        if(targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void Update()
    {
        Vector3 rotation = targetCamera.transform.eulerAngles;
        this.transform.eulerAngles = rotation;
    }
}
