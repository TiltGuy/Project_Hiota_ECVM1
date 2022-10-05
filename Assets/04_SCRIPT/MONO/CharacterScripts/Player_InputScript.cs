﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionHandler))]
public class Player_InputScript : MonoBehaviour
{
    #region DEPENDENCIES

    ActionHandler actionHandler;
    ActionCameraPlayer actionCameraPlayer;

    #endregion

    #region INPUT SETTINGS

    [Header(" -- INPUT SETTINGS -- ")]

    private InputMaster controls;

    private bool b_CursorInvisible = true;

    [Tooltip("The sum of the z axis of the controller and the ZQSD")]
    public Vector2 m_InputMoveVector = Vector2.zero;
    public bool b_InputDash = false;
    public bool b_DebugInput = false;
    public bool b_WantToParry = false;

    #endregion

    public delegate void MultiDelegate();
    public MultiDelegate OnChangeCurrentPlayerTarget;

    private void Awake()
    {
        actionHandler = GetComponent<ActionHandler>();
        actionCameraPlayer = GetComponent<ActionCameraPlayer>();

        //Initialisation of ALL the Bindings with InputMaster
        controls = new InputMaster();

        controls.Player.Movement.performed += ctx => m_InputMoveVector = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => m_InputMoveVector = Vector2.zero;

        controls.Player.Dash.started += ctx => SetInputDash(true);
        controls.Player.Dash.canceled += ctx => SetInputDash(false);

        controls.Player.ChangeFocusCameraTarget.started += ctx => WantToChangeTarget();
        controls.Player.ChangeFocusCameraTarget.canceled += ctx => actionCameraPlayer.ResetFocusCameraTargetFactor();

        controls.Player.DebugInput.started += ctx => b_DebugInput = true;
        controls.Player.DebugInput.canceled += ctx => b_DebugInput = false;

        controls.Player.Parry.started += ctx => WantingToParry(true);
        controls.Player.Parry.canceled += ctx => WantingToParry(false);

        //controls.Player.Attack.started += ctx => actionHandler.TakeAttackInputInBuffer();

        //controls.Player.FocusTarget.started += ctx => actionHandler.ToggleFocusTarget();
        //controls.Player.DebugCursorBinding.started += ctx => actionHandler.HideCursor();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void WantingToParry(bool value)
    {
        actionHandler.b_IsParrying = value;
    }

    void SetInputDash(bool value)
    {
        actionHandler.b_WantDash = value;
    }

    void WantToChangeTarget()
    {
        Vector2 input = controls.Player.ChangeFocusCameraTarget.ReadValue<Vector2>();
        actionCameraPlayer.ChangeTargetOfPlayer(input);
    }


}
