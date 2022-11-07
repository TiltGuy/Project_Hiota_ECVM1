using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCameraPlayer : MonoBehaviour
{
    Controller_FSM controller_FSM;
    Player_InputScript player_InputScript;

    public delegate void MultiDelegate();
    public MultiDelegate OnChangeTargetPlayerPosition;

    [SerializeField]
    private TargetGatherer targetGatherer;

    public Transform currentHiotaTarget;

    [SerializeField]
    private GameObject GO_FocusCamera;
    [SerializeField]
    private GameObject GO_CameraFreeLook;


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

    public void ChangeTargetOfPlayer(Vector2 input)
    {
        if (controller_FSM.b_IsFocusing)
        {
            if (controller_FSM.b_CanChangeFocusTarget)
            {
                print("Change");

                UpdateHiotaCurrentTarget(input);
                OnChangeTargetPlayerPosition();

                controller_FSM.b_CanChangeFocusTarget = false;
            }

        }
        
    }

    private void UpdateHiotaCurrentTarget(Vector2 input)
    {
        if (targetGatherer.CheckoutNextTargetedEnemy(input) != null)
        {
            currentHiotaTarget = targetGatherer.CheckoutNextTargetedEnemy(input);
            controller_FSM.currentCharacterTarget = targetGatherer.CheckoutNextTargetedEnemy(input);
            Debug.Log(currentHiotaTarget, currentHiotaTarget);
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
                currentHiotaTarget = targetGatherer.CheckoutClosestEnemyToCenterCam();
                //Debug.Log(currentHiotaTarget, this);
                controller_FSM.currentCharacterTarget = currentHiotaTarget;
                OnChangeTargetPlayerPosition();
                GO_FocusCamera.SetActive(true);
                GO_CameraFreeLook.SetActive(false);
                //Debug.Log(currentHiotaTarget, this);
            }
            
        }
        else
        {
            GO_FocusCamera.SetActive(false);
            GO_CameraFreeLook.SetActive(true);
            //Debug.Log("FreeLook Mode Camera Activated", this);
        }
    }

    private void CommandShakeCameraWhenTouchingEnemy()
    {
        //Debug.Log("SHAKE CAMERA TOUCHED !!!");
        if ( GO_CameraFreeLook )
        {
            GO_CameraFreeLook.GetComponent<CinemachineCameraShake>().ShakeCamera(.5f, .1f);
        }
        if ( GO_FocusCamera )
        {
            GO_FocusCamera.GetComponent<CinemachineCameraShake>().ShakeCamera(1f, .25f);
        }

    }

    private void CommandShakeCameraWhenBeingTouched()
    {
        //Debug.Log("SHAKE CAMERA HITTEN !!!");
        if ( GO_CameraFreeLook )
        {
            GO_CameraFreeLook.GetComponent<CinemachineCameraShake>().ShakeCamera(1f, .25f);
        }
        if ( GO_FocusCamera )
        {
            GO_FocusCamera.GetComponent<CinemachineCameraShake>().ShakeCamera(2, .5f);
        }
    }
}
