using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterSpecs))]
public class Controller_FSM : ActionHandler, IDamageable
{

    #region EXTERNAL DEPENDENCIES

    [Header(" -- EXTERNAL DEPENDENCIES -- ")]

    //[Tooltip("The Target for the FreeLook Camera... Normally the GO's name is Camera Target")]
    //public Transform cameraTarget;

    [HideInInspector]
    [Tooltip("It needs the prefab of CameraBase")]
    public Transform m_cameraBaseDirection;
    [SerializeField]
    private Transform FX_ReactionGuard;
    [SerializeField]
    private Transform FX_PerfectParry;
    [SerializeField]
    private Transform FX_HitReact;
    #endregion

    #region COMMUN DEPENDENCIES

    [Header(" -- COMMUN DEPENDENCIES -- ")]

    public PlayerController_Animator controllerAnim;
    #endregion


    #region DEPENDENCIESPlayer

    [Header(" -- DEPENDENCIES (If Player) -- ")]

    [SerializeField]
    [Tooltip("it's the layer used to test the ground")]
    private LayerMask Ground;


    [SerializeField]
    [Tooltip("it the transform which is under the feet of Hiota")]
    private Transform _groundChecker;
    
    ///<summary> The character controller of the player</summary>
    public CharacterController characontroller;

    [HideInInspector] public CharacterSpecs charSpecs;

    

    public Transform HITTouchedPosition;

    #endregion

    #region DEPENDENCIESEnemy

    [Header(" -- DEPENDENCIES (If Enemy) -- ")]

    public NavMeshAgent NavAgent;
    public Rigidbody CharRigidbody;
    public List<Transform> waypoints;
    public IABrain BrainAI;


    #endregion

    #region STATS VARIABLES

    [HideInInspector]
    public bool b_IsInvicible = false;

    [HideInInspector] private bool b_IsTouched = false;

    #endregion

    #region GLOBAL MOVEMENT Settings

    [Header(" -- GLOBAL MOVEMENT SETTINGS -- ")]

    [Tooltip("The current speed used by the FSM")]
    public float baseSpeed = 5f;

    [Tooltip("The current speed used by the FSM")]
    private float currentSpeed = 5f;

    [Tooltip("the speed of the rotation between the forward of the character and the direction to go")]
    [HideInInspector] public float m_turnSpeed = 20;

    [Tooltip("it check if Hiota is touching the ground with the Ground Checker")]
    [HideInInspector] public bool _isGrounded = false;

    [Tooltip("it's the radius of the sphere checking the IsGrounded function")]
    [HideInInspector] public float distanceCheckGround = 0.25f;

    [Tooltip("it catches Hiota when he falls")]
    [SerializeField] public float gravity = -9.17f;

    #endregion

    #region Dash Settings

    [Header(" -- DASH SETTINGS -- ")]

    public float dashCooldown = 0.75f;
    [HideInInspector] public Vector3 dashDirection;
    [HideInInspector] public Vector3 lastDirectionInput;
    [HideInInspector] public float scalarVector;
    [HideInInspector] public bool b_CanDash = true;
    [HideInInspector]
    public bool b_IsDashing = false;

    #endregion

    #region MOVEMENT Settings

    [Header(" -- MOVEMENT SETTINGS -- ")]

    ///<summary> The Forward vector of the camera</summary>
    [Tooltip("The Forward vector of the camera")]
    [HideInInspector] public Vector3 m_camF;

    ///<summary> The Right vector of the camera</summary>
    [Tooltip("The Right vector of the camera")]
    [HideInInspector] public Vector3 m_camR;

    [Tooltip("The direction where Hiota want to go")]
    [HideInInspector] public Vector3 directionToGo;

    [Tooltip("The direction where Hiota want to go")]
    [HideInInspector] public Vector3 directionToFocus;

    [Tooltip("The direction where Hiota goes actually ==> directionToGo + Gravity")]
    [HideInInspector] public Vector3 currentDirection;
    #endregion

    #region ATTACK Settings

    [Header(" -- ATTACK SETTINGS -- ")]

    [Tooltip("The speedTurn of the player when it attack")]
    [SerializeField] public float m_speedTurnWhenAttack = 5f;

    [Tooltip("the speed of the rotation between the forward of the character and the direction to go when it's in Focus")]
    public float m_turnSpeedWhenFocused = 20;

    [Tooltip("the current Stats and HitBox that the Character will use in the next move")]
    public AttackStats_SO CurrentAttackStats;

    [HideInInspector] public Vector3 DirectionHitReact;

    public float hitReactTime = .1f;


    //[Tooltip("The speed of the player")]
    //public float m_HoldAttackSpeed = 5f;

    //[Tooltip("The time remaining of a Attack")]
    //public float maxAttackTime = .2f;


    #endregion

    #region PARRY Settings

    [Header(" -- PARRY SETTINGS -- ")]


    [HideInInspector] public bool b_CanParry = false;
    [HideInInspector]
    public bool b_PerfectParry = false;
    public float timeForPerfectParry = .25f;
    [SerializeField]
    private float timeAfterPerfectlyParrying = .5f;
    [HideInInspector]
    public float perfectTimer = 0f;
    [HideInInspector] public bool b_NormalParry = false;
    [HideInInspector]
    public float statCurrentMaxGuard = 10f;
    [HideInInspector] public float statCurrentGuard = 1f;
    public float guardIncreaseSpeed = 1f;
    public float guardIncreaseSpeedWhenGuarding = 1f;
    [HideInInspector] public bool b_CanRecoverParry = true;
    [HideInInspector] public bool b_isParrying;


    #endregion

    #region CAMERA SETTINGS & DEPENDENDCIES

    //[Header(" -- CAMERA SETTINGS & DEPENDENDCIES -- ")]

    //public GameObject GO_CameraFreeLook;

    //public GameObject GO_FocusCamera;
    #endregion

    #region STATE MACHINE SETTINGS

    [Header(" -- STATE MACHINE SETTINGS -- ")]

    public State_SO currentState;
    public State_SO remainState;
    public Transform eyes;
    public bool b_HaveFinishedRecoveringAnimation;
    public bool b_HaveSuccessfullyHitten;
    private Transform lastHooker;

    #endregion

    #region DELEGATE INSTANCIATION

    public delegate void MultiDelegate();
    public MultiDelegate OnChangeCurrentPlayerTarget;

    public delegate void MultiDelegateWithfloat(float something);
    //public MultiDelegateWithfloat LoseHPDelegate;
    public MultiDelegateWithfloat UpdateGuardAmountDelegate;

    public delegate void OnEventCombatSystem();
    public OnEventCombatSystem OnAttackBegin;
    public OnEventCombatSystem OnBasicABegin;
    public OnEventCombatSystem OnFrontCABegin;
    public OnEventCombatSystem OnSideCABegin;
    public OnEventCombatSystem OnBackCABegin;
    public OnEventCombatSystem OnParryCABegin;
    public OnEventCombatSystem OnDeathEnemy;

    public OnEventCombatSystem OnTouchedEnemy;
    public OnEventCombatSystem OnHittenByEnemy;

    #endregion

    public Animator Animator
    {
        get { return characterAnimator; }
    }

    public bool B_HaveSuccessfullyHitten 
    {
        set
        { 
            b_HaveSuccessfullyHitten = value;
        }

        get => b_HaveSuccessfullyHitten;
    }

    public bool B_IsTouched
    {
        get => b_IsTouched;
        set
        {
            b_IsTouched = value;
            if (value == false)
            {
                DirectionHitReact = Vector3.zero;
            }
            //print(DirectionHitReact);
        }
    }

    public float CurrentSpeed
    {
        get => currentSpeed;
        set
        {
            currentSpeed = value;
            if(NavAgent)
            {
                NavAgent.speed = value;
            }
        }
    }

    private float timerOfHook;

    public Transform LastHooker
    {
        get => lastHooker;
        set => lastHooker = value;
    }
    public float TimerOfHook
    {
        get => timerOfHook;
        set => timerOfHook = value;
    }

    private void Awake()
    {
        characontroller = GetComponent<CharacterController>();
        charSpecs = GetComponent<CharacterSpecs>();
        //rbody = GetComponent<Rigidbody>();
        SetMainCameraBaseDirectionTransform();

        //SetGOCameraFreeLook();

        //SetGOCameraFocus();
        CurrentAttackStats = charSpecs.BasicAttackStats;
    }

    private void SetMainCameraBaseDirectionTransform()
    {
        m_cameraBaseDirection = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (m_cameraBaseDirection == null)
        {
            Debug.LogError("I Haven't a MainCamera", this);
        }
    }

    private void OnEnable()
    {
        OnAttackBegin += DebugOnEventCombatSystem;
        ///TO REFACTO (OnDeathEnemy)
        //OnDeathEnemy += ToggleFocusTarget;
    }

    private void OnDisable()
    {
        OnAttackBegin -= DebugOnEventCombatSystem;
        //OnDeathEnemy -= ToggleFocusTarget;
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitializationState(currentState);
        //Debug.Log("Player controller says : " + BasicAttackStats.hitBoxPrefab, this);Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetVariablesFromSO();

        //GO_FocusCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = currentHiotaTarget;
        //initialisation of ALL the STATS SETTINGS
        // In the AWAKE METHOD because other scripts take the stats in start method
    }

    private void SetVariablesFromSO()
    {
        if ( charSpecs.CharStats_SO )
        {
            baseSpeed = charSpecs.CharStats_SO.BaseSpeed;
            CurrentSpeed = baseSpeed;
        }
        else
        {
            Debug.LogError("Canot take charStats_SO in charSpecs !!!", this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //print("B_HaveSuccessfullyHitten = " + b_HaveSuccessfullyHitten);
        if (PauseManager.b_IsPaused) return;

        scalarVector = Vector3.Dot(transform.forward, directionToGo);
        currentState.UpdtateState(this);

        if(m_InputMoveVector!=Vector2.zero)
        {
            lastDirectionInput = directionToGo;
        }

        if(characontroller)
        {
            IsDetectingGround();
        }

        Debug.DrawRay(transform.position, directionToFocus, Color.red);
        if (charSpecs.CurrentGuard> 0)
        {
            UpdateGuardVariable();
        }
        else
        {
            //IncreaseParryVariableWhenUnderZero(guardIncreaseSpeed);
            //b_CanParry = false;
            StartCoroutine("ChockingTime");
        }
        //print("currentState = " + currentState);
        //fDebug.Log(this + " current state = " + currentState, this);
        if ( CharRigidbody )
        {
            //Debug.Log(CurrentSpeed, this);
        }
        //Debug.Log(b_HaveFinishedRecoveringAnimation, this);


    }

    private void UpdateGuardVariable()
    {
        b_CanParry = true;
        if (!b_IsInputParry)
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

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        if(_groundChecker)
        {
            Gizmos.DrawWireSphere(_groundChecker.position, distanceCheckGround);
        }
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

    
    private void OnDrawGizmos()
    {
        if (currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGizmosColor;
            Gizmos.DrawWireSphere(eyes.position, .5f);
        }
    }

    public void TakeDamages(float damageTaken, Transform striker, bool isAHook)
    {

        if ( b_isParrying )
        {
            if ( this.ControllerTargetGetDot(striker) > .25f)
            {
                TestGuard(damageTaken, striker);
            }
            else
            {
                if(!isAHook)
                {
                    HitTaken(damageTaken, striker);
                }
                else
                {
                    HitByHook(damageTaken, striker);
                }
            }
        }
        else if ( b_IsDashing )
        {
            // do Something
        }
        if (!b_isParrying && !b_IsDashing)
        {
            if ( !isAHook )
            {
                HitTaken(damageTaken, striker);
            }
            else
            {
                HitByHook(damageTaken, striker);
            }
            //print(DirectionHitReact);
            //Debug.Log("ARGH!!! j'ai pris : " + damageOuput + " points de Dommages", this);
        }
    }

    private void HitTaken( float damageTaken, Transform striker )
    {
        float damageOuput = CalculateFinalDamages(damageTaken, charSpecs.CurrentArmor);
        LoseHP(damageOuput);

        b_Stunned = true;
        SpawnFXAtPosition(FX_HitReact, eyes.transform.position);
        DirectionHitReact = GetBoolnDirHitReact(striker);
        OnHittenByEnemy?.Invoke();
    }

    private void HitByHook( float damageTaken, Transform striker )
    {
        TimerOfHook = 0f;
        LastHooker = striker;
        Debug.Log(LastHooker, this);
        DirectionHitReact = GetBoolnDirHitReact(LastHooker);
        float damageOuput = CalculateFinalDamages(damageTaken, charSpecs.CurrentArmor);
        LoseHP(damageOuput);

        b_Hooked = true;
        SpawnFXAtPosition(FX_HitReact, eyes.transform.position);
        OnHittenByEnemy?.Invoke();
    }

    private float ControllerTargetGetDot( Transform striker)
    {
        Vector3 directionToStriker = striker.position - transform.position;
        float dot = Vector3.Dot(transform.forward,directionToStriker.normalized);
        //Debug.Log("dot product guard = " + dot, this);
        return dot;
    }

    public Vector3 GetBoolnDirHitReact(Transform striker)
    {
        B_IsTouched = true;
        Vector3 pos = transform.position;
        Vector3 strikerPos = striker.position;
        //Debug.Log(striker.name, striker);
        return ( pos - strikerPos).normalized;
    }

    private float CalculateFinalDamages(float damages, float Armor)
    {
        float OutputDamage = Mathf.Clamp(damages - Armor, 0, damages);
        return OutputDamage;
    }

    private void LoseHP(float damageTaken)
    {
        charSpecs.Health -= damageTaken;
        //LoseHPDelegate(charSpecs.Health);
    }

    private void TestGuard(float damageTaken, Transform striker)
    {
        if(b_PerfectParry)
        {
            StartCoroutine(SetIsPerfectlyParryingCoroutine(timeAfterPerfectlyParrying));
            //print("Perfect PARRRY !!!");
            if ( FX_PerfectParry )
            {
                SpawnFXAtPosition(FX_PerfectParry, eyes.transform.position);
            }
        }
        else if(!b_PerfectParry)
        {
            if (charSpecs.CurrentGuard > 0)
            {
                
                if (charSpecs.CurrentGuard < damageTaken)
                {
                    StopCoroutine("ChockingTime");
                    b_Stunned = true;
                    DirectionHitReact = GetBoolnDirHitReact(striker);
                    //print(DirectionHitReact);
                    SpawnFXAtPosition(FX_HitReact, eyes.transform.position);
                    StartCoroutine("ChockingTime");
                    float damageOutput = CalculateFinalDamages(damageTaken, charSpecs.CurrentArmor);
                    LoseHP(damageOutput - charSpecs.CurrentGuard);

                    OnHittenByEnemy?.Invoke();
                    //charSpecs.CurrentGuard = 0;
                }
                charSpecs.CurrentGuard -= damageTaken;
                //Vector3 ClosestPointToStriker = GetPositionAtLocalBounds(striker);
                SpawnFXAtPosition(FX_ReactionGuard, eyes.transform.position);
                characterAnimator.SetTrigger("t_Parry");
                //Debug.Log("ARGH!!! j'ai bloqué : " + damageTaken + " points de Dommages", this);
                //print("j'en suis à " + charSpecs.CurrentGuard);

            }
        }

        //striker.SendMessage("Parry", gameObject, SendMessageOptions.DontRequireReceiver);

    }

    private Transform SpawnFXAtPosition(Transform FXPrefab,Vector3 positionToSpawn)
    {
        Transform currentObject;
        Quaternion randomRot = new Quaternion(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45,45),0);
        if (FXPrefab != null)
        {
            currentObject = Instantiate(FXPrefab, positionToSpawn, randomRot);

        }
        else
            currentObject = null;
        return currentObject;
    }

    private Vector3 GetPositionClosestAtLocalBounds(Transform striker)
    {
        if(characontroller)
        {
            return characontroller.ClosestPointOnBounds(striker.position);
        }
        else if(CharRigidbody)
        {
            return CharRigidbody.ClosestPointOnBounds(striker.position);
        }
        return Vector3.zero;
    }

    private Vector3 GetPositionFarthestAtLocalBounds( Transform striker )
    {
        return characontroller.ClosestPointOnBounds(-striker.position);
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
        yield return new WaitForSeconds(2f);
        b_CanRecoverParry = true;
        charSpecs.CurrentGuard = 0.1f;
    }

    //used when in Guard to have a GuardReact behaviour
    public IEnumerator HitReactWhenGuardingCoroutine()
    {
        yield return new WaitForSeconds(1f);
        B_IsTouched = false;
    }

    private void IncreaseParryVariable(float guardIncreaseSpeed)
    {
        if(charSpecs.CurrentGuard<= charSpecs.MaxGuard)
        {
            charSpecs.CurrentGuard += Time.deltaTime * guardIncreaseSpeed;
        }
    }

    void DebugOnEventCombatSystem()
    {
        //Debug.Log("Currently On Event Delegate", this);
    }

    public Vector3 RandomNavmeshLocation( float radius )
    {
        Vector3 finalPosition = waypoints[Mathf.RoundToInt(Random.value * (waypoints.Count - 1))].position;
        return finalPosition;

    }

    public Vector3 GetLocalVelocity()
    {
        Vector3 GetLocalVelocity = transform.InverseTransformDirection(NavAgent.velocity);
        return GetLocalVelocity;
    }

    public void TakeDamages( float damageTaken, Transform striker )
    {
        throw new NotImplementedException();
    }
}
