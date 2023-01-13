using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ActionCameraPlayer : MonoBehaviour
{
    Controller_FSM controller_FSM;
    Player_InputScript player_InputScript;

    public delegate void MultiDelegate();
    public MultiDelegate OnSwitchTargetPlayerPositionForTargetGroup;
    public MultiDelegate OnNewTargetPlayerPositionForTargetGroup;

    [SerializeField]
    private TargetGatherer targetGatherer;

    public Transform currentHiotaActionCameraTarget;

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
    }

    private void OnDisable()
    {
        controller_FSM.OnTouchedEnemy -= CommandShakeCameraWhenTouchingEnemy;
        controller_FSM.OnHittenByEnemy -= CommandShakeCameraWhenBeingTouched;
    }

    public void DoSomethingWhenCurrentTargetGetKilled()
    {
        print("ALED!!!");
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
        if(currentHiotaActionCameraTarget != null)
        {
            Transform temptarget = currentHiotaActionCameraTarget;
            if ( temptarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe != null
                && temptarget != null )
            {
                temptarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe -= DoSomethingWhenCurrentTargetGetKilled;
            }
            ToggleCameraMode();
        }
        //controller_FSM.currentCharacterTarget = null;
        

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
        }
    }

    private void DeSyncDelegateIfCurrentTargetDead( Transform tempTarget )
    {
        if ( tempTarget != currentHiotaActionCameraTarget && tempTarget != null )
        {
            currentHiotaActionCameraTarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe -= DoSomethingWhenCurrentTargetGetKilled;
            currentHiotaActionCameraTarget = tempTarget;
            currentHiotaActionCameraTarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe += DoSomethingWhenCurrentTargetGetKilled;
            controller_FSM.CurrentCharacterTarget = currentHiotaActionCameraTarget;
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
            if(targetGatherer.TargetableEnemies.Count > 0)
            {
                currentHiotaActionCameraTarget = targetGatherer.CheckoutClosestEnemyToCenterCam();
                CharacterSpecs targetSpecsScript = currentHiotaActionCameraTarget.GetComponent<CharacterSpecs>();
                targetSpecsScript.OnSomethingKilledMe += DoSomethingWhenCurrentTargetGetKilled;

                //Debug.Log(currentHiotaTarget, this);
                controller_FSM.CurrentCharacterTarget = currentHiotaActionCameraTarget;
                OnNewTargetPlayerPositionForTargetGroup?.Invoke();
                FocusCamera_cine.Priority = 10;
                FreeLookCamera_cine.Priority = 0;
                //WaitForFocusLoose();
                //Debug.Log(currentHiotaTarget, this);
            }
            
        }
        else
        {
            FocusCamera_cine.Priority = 0;
            FreeLookCamera_cine.Priority = 10;
            controller_FSM.CurrentCharacterTarget = null;
            currentHiotaActionCameraTarget = null;
            //Debug.Log("FreeLook Mode Camera Activated", this);
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
