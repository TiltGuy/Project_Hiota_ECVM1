using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    private InputMaster action;

    public GameObject controlsMenu;

    private void Awake()
    {
        action = new InputMaster();
    }

    private void Start()
    {
        action.UI.Cancel.started += ctx => CloseOptions();
    }

    private void Update()
    {
        
    }


    public void OpenControls()
    {
        if ( controlsMenu.activeInHierarchy == false )
        {
            controlsMenu.SetActive(true);
            Debug.Log("open options");
        }
        else
        {
            CloseOptions();
        }
    }

    public void CloseOptions()
    {
        if ( controlsMenu.activeInHierarchy == true )
        {
            controlsMenu.SetActive(false);
            Debug.Log("close options");
        }
    }
}
