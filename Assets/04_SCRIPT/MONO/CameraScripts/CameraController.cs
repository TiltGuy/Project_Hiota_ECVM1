using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("-- GLOBAL SETTINGS --")]
    public Transform target;

    public Vector3 offset;

    public float pitch = 2f;



    [Header("-- ZOOM, YAW & PITCH SETTINGS --")]

    public float currentZoom = 10f;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    public bool CanZoom = false;


    private float currentYaw = 0f;
    public float yawSpeed = 100f;

    private float currentPitch = 0f;
    public float pitchSpeed = 100f;
    public float minPitch;
    public float maxPitch;





    private void FixedUpdate()   
    {
        if (CanZoom == true)
        {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }
        

        currentYaw += Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;

        currentPitch -= Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);





        //Press the space bar to apply no locking to the Cursor
        if (Input.GetKey(KeyCode.Space))
            Cursor.lockState = CursorLockMode.None;

    }


    private void LateUpdate()
    {
        //transform.position = target.position - offset * currentZoom;
        Vector3 direction = new Vector3(0, 0, -currentZoom);
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        transform.position = target.position + rotation * direction;
        transform.LookAt(target.position + offset);
        //transform.LookAt(target.position + Vector3.up * pitch);
        //transform.RotateAround(target.position, new Vector3(currentPitch, currentYaw, 0f), currentPitch * currentYaw);
    }

    void OnGUI()
    {
        //Press this button to lock the Cursor
        if (GUI.Button(new Rect(0, 0, 100, 50), "Lock Cursor"))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Press this button to confine the Cursor within the screen
        if (GUI.Button(new Rect(125, 0, 100, 50), "Confine Cursor"))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
