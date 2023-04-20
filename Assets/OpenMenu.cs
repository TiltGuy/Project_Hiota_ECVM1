using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class OpenMenu : MonoBehaviour
{
    private InputMaster controls;

    public UnityEvent OnOpen;
    public UnityEvent OnClose;

    private bool b_IsOpen;

    private void Awake()
    {

        //Initialisation of ALL the Bindings with InputMaster
        if ( InputManager.inputMaster != null )
        {
            controls = InputManager.inputMaster;
            controls.UI.Enable();
        }
        else
        {
            controls = new InputMaster();
            controls.UI.Enable();
        }

        controls.UI.Select.performed += ctx => ToggleMenu();

        

    }

    private void ToggleMenu()
    {
        if(!b_IsOpen)
        {
            OnOpen?.Invoke();
            b_IsOpen = true;
        }
        else
        {
            OnClose?.Invoke();
            b_IsOpen = false;
            DeckManager.instance._RunDeck = DeckManager.instance._CurrentDeck.ToList();
        }
    }
}
