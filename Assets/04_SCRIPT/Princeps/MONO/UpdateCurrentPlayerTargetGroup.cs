using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCurrentPlayerTargetGroup : MonoBehaviour
{
    [SerializeField]
    private Controller_FSM controller;
    private Transform currentPlayerTarget;
    private Transform newPlayerTarget;
    private Transform lastTargetPostion;
    ActionCameraPlayer actionCameraPlayer;
    public float timeOfTransition;
    private float currentTransitionTimer;
    private bool b_InTransition =false;
    public AnimationCurve TransitionCurve;
    private Vector3 offset;
    private Vector3 lastOffset;

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
            transform.position = currentPlayerTarget.position + offset;
            //UpdateCurrentOffset();
        }
    }

    void SwitchMyPlayerCurrentTargetGroup()
    {
        //Debug.Log("Passe dans le switch");
        lastTargetPostion = currentPlayerTarget;
        currentPlayerTarget = actionCameraPlayer.CurrentHiotaActionCameraTarget;
        if(b_InTransition)
        {
            StopAllCoroutines();
            lastTargetPostion = transform;
        }
        StartCoroutine(TransitionToAnotherTarget(lastTargetPostion.position));
    }

    void NewMyPlayerCurrentTargetGroup()
    {
        currentPlayerTarget = actionCameraPlayer.CurrentHiotaActionCameraTarget;
        //Debug.Log("I want to update my current Target", this);
    }

    private IEnumerator TransitionToAnotherTarget( Vector3 LastPos)
    {
        float elapsed = 0f;
        float progress = 0f;
        b_InTransition = true;
        offset = transform.position - LastPos;
        while (progress <= timeOfTransition)
        {
            elapsed += Time.deltaTime;
            progress = elapsed / timeOfTransition;
            transform.position = Vector3.SlerpUnclamped(LastPos + offset, currentPlayerTarget.position, TransitionCurve.Evaluate(progress));
            yield return null;
        }
        offset = transform.position - currentPlayerTarget.position;
        lastOffset = offset;
        b_InTransition = false;
    }

    private void UpdateCurrentOffset()
    {

        offset = Vector3.LerpUnclamped(lastOffset, Vector3.zero, 2.5f);
    }

}
