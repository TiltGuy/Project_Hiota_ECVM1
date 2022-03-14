using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSetFollowLookAtFocusCamera : MonoBehaviour
{
    Transform playerTransform;
    Controller_FSM playerController;
    Cinemachine.CinemachineVirtualCamera FocusCamera;
    private void Awake()
    {
        FocusCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetLookAtAndFollow();
    }

    private void SetLookAtAndFollow()
    {
        FocusCamera.Follow = playerTransform;

        Transform Target = GameObject.FindGameObjectWithTag("CameraTargetGroup").transform;
        FocusCamera.LookAt = Target;
    }
}
