using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLockControl : MonoBehaviour
{
#if UNITY_EDITOR
    private bool hasFocus;
    private CinemachineInputProvider inputProv;

    private void Start()
    {
        hasFocus = true;
        inputProv = GetComponent<CinemachineInputProvider>();
    }

    private void Update()
    {
        if(hasFocus && Player_InputScript.controls.UI.EscapeUI.IsPressed())
        {
            hasFocus = false;
        }
        else if(!hasFocus && Player_InputScript.controls.UI.LeftClick.IsPressed())
        {
            hasFocus = true;
        }

        Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
        inputProv.enabled = hasFocus;
    }

    private void OnApplicationFocus( bool focus )
    {
        hasFocus = focus;
    }
#endif
}
