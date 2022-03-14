using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSetFollowLookAtFreelookCamera : MonoBehaviour
{
    Transform playerTransform;
    Controller_FSM playerController;
    Cinemachine.CinemachineFreeLook FLCamera;
    private void Awake()
    {
        FLCamera = GetComponent<Cinemachine.CinemachineFreeLook>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = playerTransform.GetComponent<Controller_FSM>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetLookAtAndFollow();
    }

    private void SetLookAtAndFollow()
    {
        FLCamera.Follow = playerTransform;

        if (playerController.cameraTarget != null)
        {

            FLCamera.LookAt = playerController.cameraTarget;
            Debug.LogWarning("I haven't a playerController.cameraTarget to lookAt", this);
        }
        else
        {
            FLCamera.LookAt = playerTransform;
            if (FLCamera.LookAt == null)
            {
                Debug.LogError("There isn't a playerTransform in the scene", this);
            }
        }
    }
}
