using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraShake : MonoBehaviour
{
    enum TypeOfCamera{FocusCamera, FreeLookCamera};
    [SerializeField] TypeOfCamera CameraType = TypeOfCamera.FocusCamera;

    CinemachineFreeLook NormalCamera;
    CinemachineVirtualCamera FocusCamera;
    private float shakerTimer;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private void Awake()
    {
        switch ( CameraType )
        {
            case TypeOfCamera.FocusCamera:
            {
                FocusCamera = GetComponent<CinemachineVirtualCamera>();
                break;
            }

            case TypeOfCamera.FreeLookCamera:
            {
                NormalCamera = GetComponent<CinemachineFreeLook>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(shakerTimer > 0 )
        {
            shakerTimer -= Time.deltaTime;
            if(shakerTimer <= 0 )
            {
                //time out
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }

    public void ShakeCamera( float intensity, float shakeTime )
    {

        if(CameraType == TypeOfCamera.FocusCamera)
        {
            cinemachineBasicMultiChannelPerlin = 
                FocusCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            print("Facus");
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            shakerTimer = shakeTime;
        }
        else
        {
            cinemachineBasicMultiChannelPerlin =
            NormalCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            print("Normal");
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            
            
            shakerTimer = shakeTime;
        }
    }
}
