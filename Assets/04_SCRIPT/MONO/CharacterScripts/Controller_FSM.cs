using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Controller_FSM : MonoBehaviour, IDamageable
{
    # region DEPENDENCIES

    [Header(" -- DEPENDENCIES -- ")]

    [Tooltip("The Target for the FreeLook Camera")]
    public Transform cameraTarget;

    [HideInInspector]
    [Tooltip("It needs the prefab of CameraBase")]
    public Transform m_cameraBaseDirection;

    [Tooltip("it's the layer used to test the ground")]
    private LayerMask Ground;

    [SerializeField]
    private TargetGatherer targetGatherer;

    [SerializeField]
    [Tooltip("it the transform which is under the feet of Hiota")]
    private Transform _groundChecker;
    
    ///<summary> The character controller of the player</summary>
    public CharacterController characontroller;

    ///<summary> The animator of the player</summary>
    public Animator Hiota_Anim;

    public Transform HolsterSword;

    public Transform HandOfSword;

    //TO REWORK ==> Delocalise this reference to HiotaHealth
    public CharacterStats_SO HiotaStats;

    public PlayerController_Animator controllerAnim;

    #endregion

    #region STATS VARIABLES

    [HideInInspector]
    public float statCurrentHealth;
    private float currentArmor;
    [HideInInspector]
    public bool b_IsInvicible = false;

    #endregion

    #region INPUT SETTINGS

    [Header(" -- INPUT SETTINGS -- ")]

    private InputMaster controls;

    private bool b_CursorInvisible = true;

    #endregion

    #region GLOBAL MOVEMENT Settings

    [Header(" -- GLOBAL MOVEMENT SETTINGS -- ")]

    [Tooltip("The speed of the player")]
    [SerializeField] public float m_speed = 5f;

    [Tooltip("Distance of the dash")]
    [SerializeField] public float m_dashDistance = 5f;

    [Tooltip("the speed of the rotation between the forward of the character and the direction to go")]
    public float m_turnSpeed = 20;

    [Tooltip("it's the little time before hiota begin to fall")]
    public float maxCoyoteTime = 0.15f;

    [Tooltip("it check if Hiota is touching the ground with the Ground Checker")]
    public bool _isGrounded = false;

    [Tooltip("it's the radius of the sphere checking the IsGrounded function")]
    public float distanceCheckGround = 0.25f;

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
    public float dashCooldown = 0.75f;
    public Vector3 dashDirection;
    public Vector3 lastDirectionInput;
    public float scalarVector;
    public bool b_WantDash = false;
    public bool b_CanDash = true;
    [HideInInspector]
    public bool b_IsDashing = false;

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

    [Tooltip("The direction where Hiota want to go")]
    [HideInInspector] public Vector3 directionToFocus;

    [Tooltip("The direction where Hiota goes actually ==> directionToGo + Gravity")]
    [HideInInspector] public Vector3 currentDirection;

    ///<summary> The vector3 of the inputs. The inputs are by axis</summary>
    public Vector3 m_inputsKeyBoard = Vector3.zero;

    [Tooltip("The sum of the z axis of the controller and the ZQSD")]
    public Vector2 m_InputMoveVector = Vector2.zero;

    ///<summary> the float of the inputs of the controller. The inputs are by axis but seperated into 2 variables</summary>
    public float m_LeftStickControllerX;
    public float m_LeftStickControllerZ;

    public float m_accelMovController;
    #endregion

    #region ATTACK Settings

    [Header(" -- ATTACK SETTINGS -- ")]

    [Tooltip("The speedTurn of the player when it attack")]
    [SerializeField] public float m_speedTurnWhenAttack = 5f;

    [Tooltip("the speed of the rotation between the forward of the character and the direction to go when it's in Focus")]
    public float m_turnSpeedWhenFocused = 20;

    [Tooltip("the Boolean that check the input")]
    public bool b_AttackInput = false;

    [Tooltip("the Boolean that if the player is stunned")]
    public bool b_Stunned = false;

    [Tooltip("the current Stats of the Basic Attack that will be used for the next or current hit")]
    public AttackStats_SO BasicAttackStats;

    [Tooltip("the current Stats and HitBox of the Side Attack that will be used for the next or current hit")]
    public AttackStats_SO SideAttackStats;

    [Tooltip("the current Stats and HitBox of the Front Attack that will be used for the next or current hit")]
    public AttackStats_SO FrontAttackStats;

    [Tooltip("the current Stats and HitBox of the Back Attack that will be used for the next or current hit")]
    public AttackStats_SO BackAttackStats;

    [Tooltip("the current Stats and HitBox of the Parry Attack that will be used for the next or current hit")]
    public AttackStats_SO ParryAttackStats;

    [Tooltip("the time unitl the input b_AttackInput will become false")]
    public float timeBufferAttackInput = .5f;

    //[Tooltip("The speed of the player")]
    //public float m_HoldAttackSpeed = 5f;

    //[Tooltip("The time remaining of a Attack")]
    //public float maxAttackTime = .2f;


    #endregion

    #region PARRY Settings

    [Header(" -- PARRY SETTINGS -- ")]


    [HideInInspector]
    public bool b_IsParrying = false;
    public bool b_CanParry = false;
    [HideInInspector]
    public bool b_PerfectParry = false;
    public bool b_IsPerfectlyParrying = false;
    public float timeForPerfectParry = .25f;
    [SerializeField]
    private float timeAfterPerfectlyParrying = .5f;
    [HideInInspector]
    public float perfectTimer = 0f;
    public bool b_NormalParry = false;
    [HideInInspector]
    public float statCurrentMaxGuard = 10f;
    [HideInInspector]
    public float statCurrentGuard = 1f;
    public float guardIncreaseSpeed = 1f;
    public float guardIncreaseSpeedWhenGuarding = 1f;
    public bool b_CanRecoverParry = true;


    #endregion

    #region CAMERA SETTINGS & DEPENDENDCIES

    [Header(" -- CAMERA SETTINGS & DEPENDENDCIES -- ")]

    public GameObject GO_CameraFreeLook;

    public GameObject GO_FocusCamera;

    [HideInInspector]
    [Tooltip("The focus of hiota if he have to")]
    public Transform currentHiotaTarget;

    private bool b_CanChangeFocusCameraTarget = true;

    #endregion

    #region STATE MACHINE SETTINGS

    [Header(" -- STATE MACHINE SETTINGS -- ")]

    public State_SO currentState;
    public State_SO remainState;
    public Transform eyes;

    #endregion

    #region DELEGATE INSTANCIATION

    public delegate void MultiDelegate();
    public MultiDelegate OnChangeCurrentPlayerTarget;

    public delegate void MultiDelegateWithfloat(float something);
    public MultiDelegateWithfloat LoseHPDelegate;
    public MultiDelegateWithfloat UpdateGuardAmountDelegate;

    public delegate void MultiDelegateWithVector2(Vector2 vector);
    public MultiDelegateWithVector2 OnChangeTargetFocus;

    public delegate void OnEventCombatSystem();
    public OnEventCombatSystem OnAttackBegin;
    public OnEventCombatSystem OnDeathEnemy;

    #endregion


    public Animator Animator
    {
        get { return Hiota_Anim; }
    }

    private void Awake()
    {
        characontroller = GetComponent<CharacterController>();
        //rbody = GetComponent<Rigidbody>();
        SetMainCameraBaseDirectionTransform();

        SetGOCameraFreeLook();

        SetGOCameraFocus();

        //Initialisation of ALL the Bindings with InputMaster
        controls = new InputMaster();

        controls.Player.Movement.performed += ctx => m_InputMoveVector = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => m_InputMoveVector = Vector2.zero;

        controls.Player.Dash.started += ctx => b_WantDash = true;
        controls.Player.Dash.canceled += ctx => b_WantDash = false;

        controls.Player.ChangeFocusCameraTarget.started += ctx => ChangeTargetFocusCamera();
        controls.Player.ChangeFocusCameraTarget.canceled += ctx => ResetFocusCameraTargetFactor();

        controls.Player.DebugInput.started += ctx => b_Stunned = true;
        //controls.Player.Dash.started += ctx => TakeDamages(3);
        controls.Player.DebugInput.canceled += ctx => b_Stunned = false;

        controls.Player.Parry.started += ctx => b_IsParrying = true;
        controls.Player.Parry.canceled += ctx => b_IsParrying = false;

        controls.Player.Attack.started += ctx => TakeAttackInputInBuffer();
        //  controls.Player.Attack.canceled += ctx => b_AttackInput = false;

        controls.Player.FocusTarget.started += ctx => ToggleFocusTarget();
        controls.Player.DebugCursorBinding.started += ctx => HideCursor();



    }

    private void SetMainCameraBaseDirectionTransform()
    {
        m_cameraBaseDirection = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (m_cameraBaseDirection == null)
        {
            Debug.LogError("I Haven't a MainCamera", this);
        }
    }

    private void SetGOCameraFreeLook()
    {
        GO_CameraFreeLook = GameObject.FindGameObjectWithTag("CameraFreeLook");

        if (GO_CameraFreeLook == null)
        {
            Debug.LogError("I Haven't a GO_CameraFreeLook", this);
        }
    }

    private void SetGOCameraFocus()
    {
        GO_FocusCamera = GameObject.FindGameObjectWithTag("CameraFocus");
        GO_FocusCamera.SetActive(false);
        if (GO_FocusCamera == null)
        {
            Debug.LogError("I Haven't a GO_FocusCamera", this);
        }
    }

    private void OnEnable()
    {
        controls.Enable();
        OnAttackBegin += DebugOnEventCombatSystem;
        OnDeathEnemy += ToggleFocusTarget;
        OnChangeTargetFocus += UpdateHiotaCurrentTarget;
    }

    private void OnDisable()
    {
        controls.Disable();
        OnAttackBegin -= DebugOnEventCombatSystem;
        OnDeathEnemy -= ToggleFocusTarget;
        OnChangeTargetFocus -= UpdateHiotaCurrentTarget;
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitializationState(currentState);
        //Debug.Log("Player controller says : " + BasicAttackStats.hitBoxPrefab, this);Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //GO_FocusCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = currentHiotaTarget;
        //initialisation of ALL the STATS SETTINGS
        // In the AWAKE METHOD because other scripts take the stats in start method
        statCurrentHealth = HiotaStats.baseHealth;
        currentArmor = HiotaStats.baseArmor;
        statCurrentGuard = HiotaStats.baseGuard;
        statCurrentMaxGuard = HiotaStats.maxGuard;
    }

    public void HideCursor()
    {
        if(!b_CursorInvisible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            b_CursorInvisible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            b_CursorInvisible = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (PauseManager.b_IsPaused) return;

        scalarVector = Vector3.Dot(transform.forward, directionToGo);
        currentState.UpdtateState(this);

        if(m_InputMoveVector!=Vector2.zero)
        {
            lastDirectionInput = directionToGo;
        }

        IsDetectingGround();

        Debug.DrawRay(transform.position, directionToFocus, Color.red);
        if (statCurrentGuard> 0)
        {
            UpdateGuardVariable();

        }
        else
        {
            //IncreaseParryVariableWhenUnderZero(guardIncreaseSpeed);
            //b_CanParry = false;
            StartCoroutine("ChockingTime");
        }



    }

    private void UpdateGuardVariable()
    {
        b_CanParry = true;
        if (!b_IsParrying)
        {
            IncreaseParryVariable(guardIncreaseSpeed);
        }
        else
            IncreaseParryVariable(guardIncreaseSpeedWhenGuarding);
    }

    public void TransitionToState(State_SO NextState)
    {
        if (NextState != remainState)
        {
            currentState.ExitState(this);
            currentState = NextState;
            currentState.EnterState(this);
        }
    }

    public void InitializationState(State_SO InitState)
    {
        if (InitState != remainState)
        {
            currentState = InitState;
            currentState.EnterState(this);
        }
    }

    //Detection ground with a sphere
    public void IsDetectingGround()
    {
        //_isGrounded = Physics.CheckSphere(_groundChecker.position, distanceCheckGround, Ground, QueryTriggerInteraction.Ignore);
        //if (_isGrounded)
        //{
        //    return true;
        //}
        //else
        //    return false;
        if (characontroller.isGrounded)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
        //Debug.DrawLine(transform.position,)
        //return coyoteTime < maxCoyoteTime;
    }

    //Detection ground with the function IsDetectingGround and the Coyote Time
    

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

    public void ToggleFocusTarget()
    {
        if (!b_IsFocusing )
        {
            if(targetGatherer.TargetableEnemies.Count > 0)
            {
                b_IsFocusing = true;
                Hiota_Anim.SetBool("Is_Focusing", b_IsFocusing);
                // there use a function in the target Gatherer to check the target nearest to the center of MainCam
                currentHiotaTarget = targetGatherer.CheckoutClosestEnemyToCenterCam();
                OnChangeCurrentPlayerTarget();
                GO_FocusCamera.SetActive(true);
                GO_CameraFreeLook.SetActive(false);
                Debug.Log(currentHiotaTarget);
            }
        }
        else
        {
            b_IsFocusing = false;
            Hiota_Anim.SetBool("Is_Focusing", b_IsFocusing);
            GO_FocusCamera.SetActive(false);
            GO_CameraFreeLook.SetActive(true);
            Debug.Log(b_IsFocusing, this);
        }
    }

    

    private void OnDrawGizmos()
    {
        if (currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGizmosColor;
            Gizmos.DrawWireSphere(eyes.position, .5f);
        }
    }

    public void TakeDamages(float damageTaken, Transform striker)
    {
        if (!b_IsParrying && !b_IsDashing)
        {
            float damageOuput = CalculateFinalDamages(damageTaken, currentArmor);
            LoseHP(damageOuput);
            b_Stunned = true;
            Debug.Log("ARGH!!! j'ai pris : " + damageOuput + " points de Dommages", this);
        }
        else if (b_IsParrying)
        {
            TestGuard(damageTaken);
        }
        else if (b_IsDashing)
        {
            // do Something
        }
        //LoseHP(damageTaken, currentHealth);
        //Debug.Log("il ne me reste plus que " + statCurrentHealth + " d'HP", this);
    }

    private float CalculateFinalDamages(float damages, float Armor)
    {
        float OutputDamage = Mathf.Clamp(damages - Armor, 0, damages);
        return OutputDamage;
    }

    private void LoseHP(float damageTaken)
    {
        if (statCurrentHealth > 0)
        {
            statCurrentHealth -= damageTaken;
            statCurrentHealth = Mathf.Clamp(statCurrentHealth, 0, statCurrentHealth);
            LoseHPDelegate(statCurrentHealth);
        }
    }

    private void TestGuard(float damageTaken)
    {
        if(b_PerfectParry)
        {
            StartCoroutine(SetIsPerfectlyParryingCoroutine(timeAfterPerfectlyParrying));
            print("Perfect PARRRY !!!");
        }
        else if(!b_PerfectParry)
        {
            if (statCurrentGuard > 0)
            {
                
                if(statCurrentGuard < damageTaken)
                {
                    
                    statCurrentGuard = 0;
                    StartCoroutine("ChockingTime");
                    float damageOutput = CalculateFinalDamages(damageTaken, currentArmor);
                    LoseHP(damageOutput);
                }
                statCurrentGuard -= damageTaken;
                Debug.Log("ARGH!!! j'ai bloqué : " + damageTaken + " points de Dommages", this);
                print("j'en suis à " + statCurrentGuard);
                statCurrentGuard = Mathf.Clamp(statCurrentGuard, 0, statCurrentMaxGuard);
                UpdateGuardAmountDelegate(statCurrentGuard);
            }
        }

        

    }

    private IEnumerator BufferingAttackInputCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        b_AttackInput = false;
    }

    private IEnumerator SetIsPerfectlyParryingCoroutine(float time)
    {
        StopCoroutine("SetIsPerfectlyParryingCoroutine");
        b_IsPerfectlyParrying = true;
        yield return new WaitForSeconds(time);
        b_IsPerfectlyParrying = false;
    }

    private void TakeAttackInputInBuffer()
    {
        StopCoroutine(BufferingAttackInputCoroutine(timeBufferAttackInput));
        b_AttackInput = true;
        StartCoroutine(BufferingAttackInputCoroutine(timeBufferAttackInput));
    }
    public IEnumerator BufferingDashEvent()
    {
        b_CanDash = false;
        yield return new WaitForSeconds(dashCooldown);
        b_CanDash = true;
        b_WantDash = false;
    }

    public IEnumerator ChockingTime()
    {
        b_CanRecoverParry = false;
        b_CanParry = false;
        yield return new WaitForSeconds(1f);
        b_CanRecoverParry = true;
        statCurrentGuard = 0.1f;

    }

    private void IncreaseParryVariable(float guardIncreaseSpeed)
    {
        if(statCurrentGuard<= statCurrentMaxGuard)
        {
            statCurrentGuard += Time.deltaTime * guardIncreaseSpeed;
            UpdateGuardAmountDelegate(statCurrentGuard);
        }
    }

    void DebugOnEventCombatSystem()
    {
        //Debug.Log("Currently On Event Delegate", this);
    }

    void ChangeTargetFocusCamera()
    {
        
        if(b_IsFocusing)
        {
            Vector2 input = controls.Player.ChangeFocusCameraTarget.ReadValue<Vector2>();
            //print(input.normalized);
            if (b_CanChangeFocusCameraTarget)
            {
                b_CanChangeFocusCameraTarget = false;
                OnChangeTargetFocus(input);
                OnChangeCurrentPlayerTarget();
            }
            //print("CHANGE");

        }
    }

    private void UpdateHiotaCurrentTarget(Vector2 input)
    {
        if(targetGatherer.CheckoutNextTargetedEnemy(input) != null)
        {
            currentHiotaTarget = targetGatherer.CheckoutNextTargetedEnemy(input);
            Debug.Log(targetGatherer.CheckoutNextTargetedEnemy(input));
        }
    }

    void ResetFocusCameraTargetFactor()
    {
        b_CanChangeFocusCameraTarget = true;
        //print("Reset");
    }


}
