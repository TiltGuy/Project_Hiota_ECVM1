using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCurrentPlayerTargetGroup : MonoBehaviour
{
    private Controller_FSM controller;
    [SerializeField]
    private Transform currentPlayerTarget;
    private Transform newPlayerTarget;
    private Transform lastTargetPostion;
    ActionCameraPlayer actionCameraPlayer;
    public float timeOfTransition;
    private float currentTransitionTimer;
    private bool b_InTransition =false;

    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller_FSM>();
        actionCameraPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionCameraPlayer>();
    }

    private void Start()
    {
        if(controller == null)
        {
            Debug.LogError("I haven't my ref to controller", this);
        }
        else
        {
            currentPlayerTarget = controller.currentCharacterTarget;
        }

        currentTransitionTimer = timeOfTransition;

    }

    private void OnEnable()
    {
        controller.OnChangeCurrentPlayerTarget += SwitchMyPlayerCurrentTargetGroup;
        actionCameraPlayer.OnSwitchTargetPlayerPositionForTargetGroup += SwitchMyPlayerCurrentTargetGroup;
        actionCameraPlayer.OnNewTargetPlayerPositionForTargetGroup += NewMyPlayerCurrentTargetGroup;


    }

    private void OnDisable()
    {
        controller.OnChangeCurrentPlayerTarget -= SwitchMyPlayerCurrentTargetGroup;
        actionCameraPlayer.OnSwitchTargetPlayerPositionForTargetGroup -= SwitchMyPlayerCurrentTargetGroup;
        actionCameraPlayer.OnNewTargetPlayerPositionForTargetGroup -= NewMyPlayerCurrentTargetGroup;
    }

    private void Update()
    {
        if(currentPlayerTarget != null)
        {
            UpdatePositionOfTheTargetGroup();
        }

    }

    private void UpdatePositionOfTheTargetGroup()
    {
        if(!b_InTransition)
        {
            transform.position = currentPlayerTarget.position;
        }
    }

    void SwitchMyPlayerCurrentTargetGroup()
    {
        lastTargetPostion = currentPlayerTarget;
        currentPlayerTarget = actionCameraPlayer.currentHiotaActionCameraTarget;
        if(b_InTransition)
        {
            StopCoroutine(TransitionToAnotherTarget(Time.deltaTime));
        }
        StartCoroutine(TransitionToAnotherTarget(Time.deltaTime));
    }

    void NewMyPlayerCurrentTargetGroup()
    {
        currentPlayerTarget = actionCameraPlayer.currentHiotaActionCameraTarget;
        //Debug.Log("I want to update my current Target", this);
    }

    private IEnumerator TransitionToAnotherTarget( float StartTime)
    {
        b_InTransition = true;
        transform.position = Vector3.Lerp(lastTargetPostion.position, currentPlayerTarget.position, (Time.time - StartTime) / timeOfTransition);
        yield return new WaitForSeconds(timeOfTransition);
        b_InTransition = false;
    }

}
