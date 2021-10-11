﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HiotaController_FSM : MonoBehaviour
{
    # region INPUT SETTINGS

    [Header(" -- INPUT SETTINGS -- ")]
    public InputMaster controls;

    #endregion

    #region GLOBAL MOVEMENT Settings

    [Header(" -- GLOBAL MOVEMENT SETTINGS -- ")]

    [Tooltip("The speed of the player")]
    [SerializeField] public float m_speed = 5f;

    [Tooltip("Height of the jump")]
    [SerializeField] public float m_jumpHeight = 2f;

    [Tooltip("Distance of the dash")]
    [SerializeField] public float m_dashDistance = 5f;

    [Tooltip("It needs the prefab of CameraBase")]
    public Transform m_cameraBaseDirection;

    [Tooltip("the speed of the rotation between the forward of th character and the direction to go")]
    public float m_turnSpeed = 20;

    [Tooltip("The sum of the x axis of the controller and the ZQSD")]
    [SerializeField] private float m_finalInputCharacterX;

    [Tooltip("The sum of the z axis of the controller and the ZQSD")]
    [SerializeField] private float m_finalInputCharacterZ;

    [Tooltip("it's the little time before hiota begin to fall")]
    public float maxCoyoteTime = 0.15f;
    private float coyoteTime = 0;

    [Tooltip("it check if Hiota is touching the ground with the Ground Checker")]
    public bool _isGrounded = false;

    [Tooltip("it the transform which is under the feet of Hiota")]
    public Transform _groundChecker;

    [Tooltip("it's the radius of the sphere checking the IsGrounded function")]
    public float distanceCheckGround = 0.25f;

    [Tooltip("it's the layer used to test the ground")]
    public LayerMask Ground;

    [Tooltip("If the player is focusing a enemy")]
    [SerializeField] public bool b_IsFocusing = false;

    [Tooltip("it catches Hiota when he falls")]
    [SerializeField] public float gravity = -9.17f;

    #endregion

    #region Dash Settings

    [Header(" -- DASH SETTINGS -- ")]

    public float dashVectorMultiplicator = 1f;
    public float dashDistance = 2f;
    public float maxDashTime = .5f;
    public float dashSpeed = 5f;
    public Vector3 dashDirection;
    public bool b_WantDash = false;

    #endregion

    #region MOVEMENT Settings

    [Header(" -- MOVEMENT SETTINGS -- ")]

    ///<summary> The Forward vector of the camera</summary>
    [Tooltip("The Forward vector of the camera")]
    [SerializeField] public Vector3 m_camF;

    ///<summary> The Right vector of the camera</summary>
    [Tooltip("The Right vector of the camera")]
    [SerializeField] public Vector3 m_camR;

    [Tooltip("The direction where Hiota want to go")]
    [HideInInspector] public Vector3 directionToGo;

    [Tooltip("The direction where Hiota goes actually ==> directionToGo + Gravity")]
    [HideInInspector] public Vector3 currentDirection;

    [Tooltip("The focus of hiota if he have to")]
    public Transform currentHiotaTarget;

    ///<summary> The character controller of the player</summary>
    public CharacterController characontroller;

    ///<summary> The animator of the player</summary>
    public Animator Hiota_Anim;

    ///<summary> The character controller of the player</summary>
    //public Rigidbody rbody;

    private HiotaBaseState currentState;

    ///<summary> The vector3 of the inputs. The inputs are by axis</summary>
    public Vector3 m_inputsKeyBoard = Vector3.zero;

    [Tooltip("The sum of the z axis of the controller and the ZQSD")]
    public Vector2 m_InputMoveVector = Vector2.zero;

    ///<summary> the float of the inputs of the controller. The inputs are by axis but seperated into 2 variables</summary>
    public float m_LeftStickControllerX;
    public float m_LeftStickControllerZ;
    #endregion


    #region ATTACK Settings

    [Header(" -- ATTACK SETTINGS -- ")]

    [Tooltip("The speed of the player")]
    [SerializeField] public float m_HoldAttackSpeed = 5f;

    public float maxAttackTime = .2f;
       

    #endregion

    ///////////////////////////////
    ///SETUP OF THE BASIC STATES///
    ///////////////////////////////
    public readonly HiotaIdleState IdleState = new HiotaIdleState();
    public readonly HiotaJumpingState JumpingState = new HiotaJumpingState();
    public readonly HiotaMovingState MovingState = new HiotaMovingState();
    public readonly HiotaFallingState FallingState = new HiotaFallingState();
    public readonly HiotaDashingState DashingState = new HiotaDashingState();
    public readonly Hiota_LightAttack_01 LightAttack1State = new Hiota_LightAttack_01();

    ///////////////////////////////
    ///SETUP OF THE LIGHT STATES///
    ///////////////////////////////
    public readonly HiotaLAHoldState LAHoldState = new HiotaLAHoldState();


    public HiotaBaseState CurrentState
    {
        get { return currentState; }
    }

    public Animator Animator
    {
        get { return Hiota_Anim; }
    }

    /*public Rigidbody Rigidbody
    {
        get { return rbody; }
    }*/

    private void Awake()
    {
        characontroller = GetComponent<CharacterController>();
        //rbody = GetComponent<Rigidbody>();

        //Initialisation of ALL the Bindings with InputMaster
        controls = new InputMaster();

        controls.Player.Movement.performed += ctx => m_InputMoveVector = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => m_InputMoveVector = Vector2.zero;

        controls.Player.Dash.started += ctx => b_WantDash = true;

        controls.Player.FocusTarget.started += ctx => ToggleFocusTarget();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitializeState(this.MovingState);
    }

    // Update is called once per frame
    private void Update()
    {
        if(IsDetectingGround())
        {
            coyoteTime = 0;
        }
        else
        {
            coyoteTime += Time.deltaTime;
        }

        //if(b_IsFocusing)
        //{
        //    m_cameraBaseDirection.gameObject.GetComponent<MainCameraFollow>().m_cameraTarget.SetParent(hiotaTarget);
        //    m_cameraBaseDirection.gameObject.GetComponent<MainCameraFollow>().m_cameraTarget.transform.localPosition = Vector3.zero;
        //}
        //else
        //{
        //    m_cameraBaseDirection.gameObject.GetComponent<MainCameraFollow>().m_cameraTarget.SetParent(transform);
        //    m_cameraBaseDirection.gameObject.GetComponent<MainCameraFollow>().m_cameraTarget.transform.localPosition = Vector3.zero;
        //}

        currentState.HandleInput(this);

        currentState.LogicUpdate(this);
        //Debug.Log("CurrentState = " + currentState);
        //Debug.Log(directionToGo);

    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this);
    }

    public void TransitionToState(HiotaBaseState state)
    {
        currentState.Exit(this);

        currentState = state;
        currentState.EnterState(this);
    }

    public void InitializeState(HiotaBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }


    //Detection ground with a sphere
    public bool IsDetectingGround()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, distanceCheckGround, Ground, QueryTriggerInteraction.Ignore);
        if (_isGrounded)
        {
            return true;
        }
        else
            return false;
        //Debug.DrawLine(transform.position,)
        //return coyoteTime < maxCoyoteTime;
    }

    //Detection ground with the function IsDetectingGround and the Coyote Time
    public bool IsGrounded()
    {
        if (IsDetectingGround() && (coyoteTime < maxCoyoteTime))
        {
            return true;
        }
        else
            return false;
        //Debug.DrawLine(transform.position,)
        //return coyoteTime < maxCoyoteTime;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_groundChecker.position, distanceCheckGround);
    }

    public Vector3 CalculateMidVector(Vector3 Origin, Vector3 Target)
    {
        return Origin + Target;
    }

    public void RotatePlayerNorY(Transform target)
    {
        Vector3 focusDirection = target.position - transform.position;
        focusDirection.y = 0;
        var rotation = Quaternion.LookRotation(focusDirection, transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_turnSpeed);
    }

    private void ToggleFocusTarget()
    {
        if (!b_IsFocusing)
        {
            b_IsFocusing = true;
            m_cameraBaseDirection.gameObject.GetComponent<MainCameraFollow>().b_CameraFocused = b_IsFocusing;
            //Debug.Log(m_cameraBaseDirection.gameObject.GetComponent<MainCameraFollow>().b_CameraFocused);
        }
        else
        {
            b_IsFocusing = false;
            m_cameraBaseDirection.gameObject.GetComponent<MainCameraFollow>().b_CameraFocused = b_IsFocusing;
            //Debug.Log(m_cameraBaseDirection.gameObject.GetComponent<MainCameraFollow>().b_CameraFocused);
        }
    }

}