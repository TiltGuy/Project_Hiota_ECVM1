using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CloneCamera : MonoBehaviour
{
    public new Camera camera => GetComponent<Camera>();
    public Camera otherCamera;

    void Update()
    {
        if ( otherCamera == null )
            otherCamera = Camera.main;

        if ( otherCamera != null )
        {
            camera.transform.position = otherCamera.transform.position;
            camera.transform.rotation = otherCamera.transform.rotation;
            camera.fieldOfView = otherCamera.fieldOfView;
        }
    }
}
