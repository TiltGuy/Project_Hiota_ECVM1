using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCameraPlayer : MonoBehaviour
{
    Controller_FSM controller_FSM;
    Player_InputScript player_InputScript;

    public delegate void MultiDelegate();
    public MultiDelegate OnChangeTargetPlayerPositionForTargetGroup;

    [SerializeField]
    private TargetGatherer targetGatherer;

    public Transform currentHiotaActionCameraTarget;

    [SerializeField]
    private bool shakeFocusCamera;
    [SerializeField]
    private GameObject GO_FocusCamera;
    [SerializeField]
    private bool shakeFreeLookCamera;
    [SerializeField]
    private GameObject GO_CameraFreeLook;

    public bool b_CameraGoToNextEnemyIfPreviousDead;



    private void Awake()
    {
        controller_FSM = GetComponent<Controller_FSM>();
        player_InputScript = GetComponent<Player_InputScript>();
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
        Transform temptarget = currentHiotaActionCameraTarget;
        ToggleCameraMode();
        //controller_FSM.currentCharacterTarget = null;
        if( temptarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe != null )
        {
            temptarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe -= DoSomethingWhenCurrentTargetGetKilled;
        }

    }

    public void InputCommandToChangeTargetOfPlayer(Vector2 input)
    {
        if (controller_FSM.b_IsFocusing)
        {
            if (controller_FSM.b_CanChangeFocusTarget)
            {
                print("Change");

                UpdateHiotaCurrentTarget(input);
                OnChangeTargetPlayerPositionForTargetGroup();

                controller_FSM.b_CanChangeFocusTarget = false;
            }
        }
        
    }

    private void UpdateHiotaCurrentTarget(Vector2 input)
    {
        if (targetGatherer.CheckoutNextTargetedEnemy(input) != null)
        {
            Transform tempTarget = targetGatherer.CheckoutNextTargetedEnemy(input);
            if(tempTarget != currentHiotaActionCameraTarget && tempTarget!= null)
            {
                currentHiotaActionCameraTarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe -= DoSomethingWhenCurrentTargetGetKilled;
                currentHiotaActionCameraTarget = tempTarget;
                currentHiotaActionCameraTarget.GetComponent<CharacterSpecs>().OnSomethingKilledMe += DoSomethingWhenCurrentTargetGetKilled;
                controller_FSM.CurrentCharacterTarget = currentHiotaActionCameraTarget;
            }
            Debug.Log(currentHiotaActionCameraTarget, currentHiotaActionCameraTarget);
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
                OnChangeTargetPlayerPositionForTargetGroup();
                GO_FocusCamera.SetActive(true);
                GO_CameraFreeLook.SetActive(false);
                //WaitForFocusLoose();
                //Debug.Log(currentHiotaTarget, this);
            }
            
        }
        else
        {
            GO_FocusCamera.SetActive(false);
            GO_CameraFreeLook.SetActive(true);
            controller_FSM.CurrentCharacterTarget = null;
            currentHiotaActionCameraTarget = null;
            //Debug.Log("FreeLook Mode Camera Activated", this);
        }
    }

    //TODO: scotch
    private void WaitForFocusLoose()
    {
        if ( !GO_FocusCamera.activeInHierarchy )
            return;

        if(currentHiotaActionCameraTarget == null || currentHiotaActionCameraTarget.gameObject.activeInHierarchy == false)
        {
            ToggleCameraMode();
        }
        else
        {
            Invoke("WaitForFocusLoose", .1f);
        }
    }

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
