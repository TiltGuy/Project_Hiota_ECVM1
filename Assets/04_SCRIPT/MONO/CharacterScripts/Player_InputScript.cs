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

    private InputMaster controls;

    private bool b_CursorInvisible = true;

    [HideInInspector]
    public bool b_InputDash = false;
    [HideInInspector]
    public bool b_DebugInput = false;
    [HideInInspector]
    public bool b_WantToParry = false;

    #endregion

    public delegate void MultiDelegate();
    public MultiDelegate OnChangeCurrentPlayerTarget;

    private void Awake()
    {
        controller_FSM = GetComponent<Controller_FSM>();
        actionCameraPlayer = GetComponent<ActionCameraPlayer>();

        //Initialisation of ALL the Bindings with InputMaster
        controls = new InputMaster();

        controls.Player.Movement.performed += ctx => InputMovement(ctx.ReadValue<Vector2>());
        controls.Player.Movement.canceled += ctx => InputMovement(Vector2.zero);

        controls.Player.Dash.started += ctx => SetInputDash(true);
        controls.Player.Dash.canceled += ctx => SetInputDash(false);

        controls.Player.ChangeFocusCameraTarget.started += ctx => WantToChangeTarget();
        controls.Player.ChangeFocusCameraTarget.canceled += ctx => actionCameraPlayer.ResetFocusCameraTargetFactor();

        controls.Player.DebugInput.started += ctx => controller_FSM.DebugAction(true);
        controls.Player.DebugInput.canceled += ctx => controller_FSM.DebugAction(false);

        controls.Player.Parry.started += ctx => WantingToParry(true);
        controls.Player.Parry.canceled += ctx => WantingToParry(false);

        controls.Player.Attack.started += ctx => controller_FSM.TakeAttackInputInBuffer();

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
        controller_FSM.m_InputMoveVector = value;
    }

    void WantingToParry(bool value)
    {
        controller_FSM.b_IsParrying = value;
    }

    void SetInputDash(bool value)
    {
        controller_FSM.b_WantDash = value;
    }

    void WantToChangeTarget()
    {
        Vector2 input = controls.Player.ChangeFocusCameraTarget.ReadValue<Vector2>();
        actionCameraPlayer.ChangeTargetOfPlayer(input);
    }

    void ToggleHiotaFocusMode()
    {
        //appeler la fonction dans la cameraAction script pour changer le comportement camera
        actionCameraPlayer.ToggleCameraMode();
        //appeler la fonction dans l'action handler qui va changer le comportement SM et behaviour anim
        controller_FSM.ToggleFocusTarget();
        print(controller_FSM.b_IsFocusing);
        
    }


}
