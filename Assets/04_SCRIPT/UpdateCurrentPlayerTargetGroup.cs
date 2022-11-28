using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCurrentPlayerTargetGroup : MonoBehaviour
{
    private Controller_FSM controller;
    [SerializeField]
    private Transform currentPlayerTarget;
    ActionCameraPlayer actionCameraPlayer;
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

    }

    private void OnEnable()
    {
        controller.OnChangeCurrentPlayerTarget += UpdateMyPlayerCurrentTargetGroup;
        actionCameraPlayer.OnChangeTargetPlayerPositionForTargetGroup += UpdateMyPlayerCurrentTargetGroup;


    }

    private void OnDisable()
    {
        controller.OnChangeCurrentPlayerTarget -= UpdateMyPlayerCurrentTargetGroup;
        actionCameraPlayer.OnChangeTargetPlayerPositionForTargetGroup -= UpdateMyPlayerCurrentTargetGroup;
    }

    private void Update()
    {
        if(currentPlayerTarget != null)
        {
            transform.position = currentPlayerTarget.transform.position;
        }
    }

    void UpdateMyPlayerCurrentTargetGroup()
    {
        currentPlayerTarget = actionCameraPlayer.currentHiotaTarget;
        //Debug.Log("I want to update my current Target", this);
    }
}
