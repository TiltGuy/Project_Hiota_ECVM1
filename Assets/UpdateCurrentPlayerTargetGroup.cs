using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCurrentPlayerTargetGroup : MonoBehaviour
{
    private Controller_FSM controller;
    [SerializeField]
    private Transform currentPlayerTarget;
    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller_FSM>();
    }

    private void Start()
    {
        if(controller == null)
        {
            Debug.LogError("I haven't my ref to controller", this);
        }
        else
        {
            currentPlayerTarget = controller.currentHiotaTarget;
        }

    }

    private void OnEnable()
    {
        controller.OnChangeCurrentPlayerTarget += UpdateMyPlayerCurrentTargetGroup;
    }

    private void OnDisable()
    {
        controller.OnChangeCurrentPlayerTarget -= UpdateMyPlayerCurrentTargetGroup;
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
        currentPlayerTarget = controller.currentHiotaTarget;
        //Debug.Log("I want to update my current Target", this);
    }
}
