using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.TestTools;

public class ActionCameraPlayer : MonoBehaviour
{
    Controller_FSM controller_FSM;
    Player_InputScript player_InputScript;

    public delegate void MultiDelegate();
    public MultiDelegate OnSwitchTargetPlayerPositionForTargetGroup;
    public MultiDelegate OnNewTargetPlayerPositionForTargetGroup;

    public delegate void MultiDelegateWithGo( GameObject go );
    public MultiDelegateWithGo OnNewCameraTarget;
    public MultiDelegateWithGo OnRemoveCameraTarget;

    [SerializeField]
    private TargetGatherer targetGatherer;

    private Transform currentHiotaActionCameraTarget;

    [SerializeField]
    private bool shakeFocusCamera;
    [SerializeField]
    private GameObject GO_FocusCamera;
    private CinemachineVirtualCamera FocusCamera_cine;
    [SerializeField]
    private bool shakeFreeLookCamera;
    [SerializeField]
    private GameObject GO_CameraFreeLook;
    private CinemachineFreeLook FreeLookCamera_cine;

    public bool b_CameraGoToNextEnemyIfPreviousDead;

    [SerializeField]
    private FMODUnity.EventReference LockFocusSound;

    public Transform CurrentHiotaActionCameraTarget
    {
        get => currentHiotaActionCameraTarget;
        set
        {
            if(value == null)
            {
                OnRemoveCameraTarget?.Invoke(currentHiotaActionCameraTarget.gameObject);
                currentHiotaActionCameraTarget = value;
                return;
            }
            if((value != currentHiotaActionCameraTarget) && (currentHiotaActionCameraTarget != null))
            {
                OnRemoveCameraTarget?.Invoke(currentHiotaActionCameraTarget.gameObject);
                currentHiotaActionCameraTarget = value;
                OnNewCameraTarget?.Invoke(currentHiotaActionCameraTarget.gameObject);
                return;
            }
            currentHiotaActionCameraTarget=value;
            OnNewCameraTarget?.Invoke(currentHiotaActionCameraTarget.gameObject);
        }
    }

    private void Awake()
    {
        controller_FSM = GetComponent<Controller_FSM>();
        player_InputScript = GetComponent<Player_InputScript>();
        FreeLookCamera_cine = GO_CameraFreeLook.GetComponent<CinemachineFreeLook>();
        FocusCamera_cine = GO_FocusCamera.GetComponent<CinemachineVirtualCamera>();
    }
    private void OnEnable()
    {
        controller_FSM.OnTouchedEnemy += CommandShakeCameraWhenTouchingEnemy;
        controller_FSM.OnHittenByEnemy += CommandShakeCameraWhenBeingTouched;
        controller_FSM.charSpecs.OnSomethingKilledMe += UnlockCameraOnDeath;

    }

    private void OnDisable()
    {
        controller_FSM.OnTouchedEnemy -= CommandShakeCameraWhenTouchingEnemy;
        controller_FSM.OnHittenByEnemy -= CommandShakeCameraWhenBeingTouched;
        controller_FSM.charSpecs.OnSomethingKilledMe -= UnlockCameraOnDeath;
    }

    public void DoSomethingWhenCurrentTargetGetKilled()
    {
        //print("ALED!!!");
        //if(targetGatherer.TargetableEnemies.Count >1 && b_CameraGoToNextEnemyIfPreviousDead)
        //{
        //    Transform tempTarget = currentHiotaTarget;
        //    currentHiotaTarget = targetGatherer.CheckoutClosestEnemyToCenterCam();

        //    controller_FSM.currentCharacterTarget = currentHiotaTarget;
        //    OnChangeTargetPlayerPositionForTargetGroup();
        //}
        //else if ( !b_CameraGoToNextEnemyIfPreviousDead && targetGatherer.TargetableEnemies.Count <= 1 )
        //{
        //    ToggleCameraMode();
        //    controller_FSM.b_IsFocusing = false;
        //    controller_FSM.characterAnimator.SetBool("Is_Focusing", false);
        //    controller_FSM.currentCharacterTarget = null;
        //}
        //else
        //{

        //}

        //Debug.Log("currentHiotaActionCameraTarget" + currentHiotaActionCameraTarget);
        if(CurrentHiotaActionCameraTarget != null)
        {
            Transform temptarget = CurrentHiotaActionCameraTarget;
            if ( temptarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe != null
                && temptarget != null )
            {
                temptarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe -= DoSomethingWhenCurrentTargetGetKilled;
            }
            ToggleCameraMode();
        }
        //controller_FSM.currentCharacterTarget = null;
        

    }

    private void UnlockCameraOnDeath()
    {
        if(controller_FSM.currentCharacterTarget == null)
        {
            ToggleCameraMode();
        }
    }

    public void InputCommandToChangeTargetOfPlayer(Vector2 input)
    {
        if (controller_FSM.b_IsFocusing)
        {
            if (controller_FSM.b_CanChangeFocusTarget)
            {

                UpdateHiotaCurrentTarget(input);
                //Debug.Log("Je passe dedans = " + input.magnitude);
                controller_FSM.b_CanChangeFocusTarget = false;

            }
        }
        
    }

    private void UpdateHiotaCurrentTarget(Vector2 input)
    {
        if (targetGatherer.CheckoutNextTargetedEnemy(input) != null)
        {
            Transform tempTarget = targetGatherer.CheckoutNextTargetedEnemy(input);
            DeSyncDelegateIfCurrentTargetDead(tempTarget);
            //Debug.Log(currentHiotaActionCameraTarget, currentHiotaActionCameraTarget);
            print("Change");

            OnSwitchTargetPlayerPositionForTargetGroup?.Invoke();

            FMODUnity.RuntimeManager.PlayOneShotAttached(LockFocusSound, Camera.main.gameObject);
        }
    }

    private void DeSyncDelegateIfCurrentTargetDead( Transform tempTarget )
    {
        if ( tempTarget != CurrentHiotaActionCameraTarget && tempTarget != null )
        {
            CurrentHiotaActionCameraTarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe -= DoSomethingWhenCurrentTargetGetKilled;
            CurrentHiotaActionCameraTarget = tempTarget;
            CurrentHiotaActionCameraTarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe += DoSomethingWhenCurrentTargetGetKilled;
            controller_FSM.CurrentCharacterTarget = CurrentHiotaActionCameraTarget;
        }
    }

    public void ResetFocusCameraTargetFactor()
    {
        controller_FSM.b_CanChangeFocusCameraTarget = true;
        controller_FSM.b_CanChangeFocusTarget = true;
        //print("Reset");
    }

    public void ToggleCameraMode()
    {
        if(!controller_FSM.b_IsFocusing)
        {
            AssignCurrentCameraTargetIfAvailable();

        }
        else
        {
            FocusCamera_cine.Priority = 0;
            FreeLookCamera_cine.Priority = 10;
            if(controller_FSM.CurrentCharacterTarget != null)
            {
                IABrain currentBrain = controller_FSM.CurrentCharacterTarget.GetComponent<IABrain>();
                if ( currentBrain != null )
                {
                    currentBrain.DisplayHealthBar(false, false);
                }
            }
            controller_FSM.CurrentCharacterTarget = null;
            CurrentHiotaActionCameraTarget = null;
            //Debug.Log("FreeLook Mode Camera Activated", this);
        }
    }

    private void AssignCurrentCameraTargetIfAvailable()
    {
        if ( targetGatherer.TargetableEnemies.Count > 0 )
        {
            CurrentHiotaActionCameraTarget = targetGatherer.CheckoutClosestEnemyToCenterCam();
            //Debug.Log(currentHiotaActionCameraTarget);
            CharacterSpecs targetSpecsScript = CurrentHiotaActionCameraTarget.GetComponent<CharacterSpecs>();
            targetSpecsScript.OnSomethingKilledMe += DoSomethingWhenCurrentTargetGetKilled;

            //Debug.Log(currentHiotaTarget, this);
            controller_FSM.CurrentCharacterTarget = CurrentHiotaActionCameraTarget;
            OnNewTargetPlayerPositionForTargetGroup?.Invoke();
            FocusCamera_cine.Priority = 10;
            FreeLookCamera_cine.Priority = 0;

            FMODUnity.RuntimeManager.PlayOneShotAttached(LockFocusSound, Camera.main.gameObject);
            //WaitForFocusLoose();
            //Debug.Log(currentHiotaTarget, this);
        }
    }

    //TODO: scotch
    //private void WaitForFocusLoose()
    //{
    //    if ( !GO_FocusCamera.activeInHierarchy )
    //        return;

    //    if(currentHiotaActionCameraTarget == null || currentHiotaActionCameraTarget.gameObject.activeInHierarchy == false)
    //    {
    //        ToggleCameraMode();
    //    }
    //    else
    //    {
    //        Invoke("WaitForFocusLoose", .1f);
    //    }
    //}

    private void CommandShakeCameraWhenTouchingEnemy()
    {
        //Debug.Log("SHAKE CAMERA TOUCHED !!!");
        if ( GO_CameraFreeLook != null && shakeFocusCamera )
        {
            GO_CameraFreeLook.GetComponent<CinemachineCameraShake>().ShakeCamera(.5f, .1f);
        }
        if ( GO_FocusCamera != null && shakeFreeLookCamera )
        {
            GO_FocusCamera.GetComponent<CinemachineCameraShake>().ShakeCamera(1f, .25f);
        }

    }

    private void CommandShakeCameraWhenBeingTouched()
    {
        //Debug.Log("SHAKE CAMERA HITTEN !!!");
        if ( GO_CameraFreeLook != null && shakeFocusCamera )
        {
            GO_CameraFreeLook.GetComponent<CinemachineCameraShake>().ShakeCamera(1f, .25f);
        }
        if ( GO_FocusCamera != null && shakeFreeLookCamera )
        {
            GO_FocusCamera.GetComponent<CinemachineCameraShake>().ShakeCamera(2, .5f);
        }
    }
}
