using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionHandler))]
public class Player_InputScript : MonoBehaviour
{
    #region DEPENDENCIES

    Controller_FSM controller_FSM;
    ActionCameraPlayer actionCameraPlayer;

    #endregion

    #region INPUT SETTINGS

    [Header(" -- INPUT SETTINGS -- ")]

    static public InputMaster controls;

    private bool b_CursorInvisible = true;

    [HideInInspector]
    public bool b_InputDash = false;
    [HideInInspector]
    public bool b_DebugInput = false;
    [HideInInspector]
    public bool b_WantToParry = false;

    #endregion

    [SerializeField]
    bool b_IsInUI;
    public bool B_IsInUI
    {
        get => b_IsInUI;
        set
        {
            if (b_IsInUI != value)
            {
                b_IsInUI = value;
            }
        }
    }


    public delegate void MultiDelegate();
    public MultiDelegate OnChangeCurrentPlayerTarget;
    private bool b_ChangeTargetInput;

    [SerializeField]
    private bool b_IsControllable = true;

    public bool B_IsControllable
    {
        get => b_IsControllable;
        set
        {
            if(b_IsControllable != value)
                b_IsControllable = value;

            if(b_IsControllable == false)
            {
                InputMovement(Vector2.zero);
            }
        }
    }


    private void Awake()
    {
        controller_FSM = GetComponent<Controller_FSM>();
        actionCameraPlayer = GetComponent<ActionCameraPlayer>();

        controller_FSM.charSpecs.OnSomethingKilledMe += UpdateHiotaControlModeOnDeath;

        //Initialisation of ALL the Bindings with InputMaster
        if(InputManager.inputMaster != null)
        {
            controls = InputManager.inputMaster;
        }
        else
        {
            controls = new InputMaster();
        }

        controls.Player.Movement.performed += ctx => InputMovement(ctx.ReadValue<Vector2>());
        controls.Player.Movement.canceled += ctx => InputMovement(Vector2.zero);

        controls.Player.Dash.started += ctx => SetInputDash(true);
        controls.Player.Dash.canceled += ctx => SetInputDash(false);

        controls.Player.ChangeFocusCameraTarget.performed += ctx => WantToChangeTarget(ctx.ReadValue<Vector2>());
        controls.Player.ChangeFocusCameraTarget.canceled += ctx => actionCameraPlayer.ResetFocusCameraTargetFactor();

        controls.Player.DebugInput.started += ctx => controller_FSM.DebugAction(true);
        controls.Player.DebugInput.canceled += ctx => controller_FSM.DebugAction(false);

        controls.Player.Parry.started += ctx => WantingToParry(true);
        controls.Player.Parry.canceled += ctx => WantingToParry(false);

        controls.Player.Attack.started += ctx => InputAttack();

        controls.Player.FocusTarget.started += ctx => ToggleHiotaFocusMode();
        controls.Player.DebugCursorBinding.started += ctx => controller_FSM.HideCursor();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void InputMovement(Vector2 value)
    {
        if ( !b_IsControllable )
        {
            return;
        }
        controller_FSM.m_InputMoveVector = value;
    }

    void InputAttack()
    {
        if(!b_IsControllable)
        {
            return;
        }

        if ( b_IsInUI )
            return;
        if(controller_FSM != null)
        {
            controller_FSM.TakeAttackInputInBuffer();
        }
    }

    void WantingToParry(bool value)
    {
        if ( !b_IsControllable )
        {
            controller_FSM.b_IsInputParry = false;
            return;
        }
        if (b_IsInUI)
        {
            controller_FSM.b_IsInputParry = false;
            Debug.Log(controller_FSM.b_IsInputParry);
            return;
        }
        controller_FSM.b_IsInputParry = value;
        //print("PARRRY INPUt ! & b_IsUIing = " + b_IsInUI);
    }

    void SetInputDash(bool value)
    {
        if ( !b_IsControllable )
        {
            controller_FSM.b_WantDash = false;
            return;
        }
        if ( b_IsInUI)
        {
            controller_FSM.b_WantDash = false;
            return;
        }
        controller_FSM.b_WantDash = value;
    }

    void WantToChangeTarget(Vector2 value)
    {
        if ( !b_IsControllable )
        {
            return;
        }
        Vector2 input = controls.Player.ChangeFocusCameraTarget.ReadValue<Vector2>();
        if(input.x > 0.4f || input.x < -0.4f)
        {
            actionCameraPlayer.InputCommandToChangeTargetOfPlayer(input);
            //Debug.Log("Vector2 = " + input.magnitude);
        }
    }

    void ToggleHiotaFocusMode()
    {
        if(!b_IsControllable)
        {
            return;
        }
        //appeler la fonction dans la cameraAction script pour changer le comportement camera
        actionCameraPlayer.ToggleCameraMode();
        //appeler la fonction dans l'action handler qui va changer le comportement SM et behaviour anim
        //controller_FSM.ToggleFocusTarget();
        //print(controller_FSM.b_IsFocusing);
        
    }

    private void UpdateHiotaControlModeOnDeath()
    {
        b_IsControllable = false;
    }


    private void OnGUI()
    {
        // PG: debug input "kill target"
        if ( Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.K )
        {
            if ( controller_FSM.CurrentCharacterTarget != null )
                controller_FSM.CurrentCharacterTarget.SendMessage("Kill");
        }
    }

}
