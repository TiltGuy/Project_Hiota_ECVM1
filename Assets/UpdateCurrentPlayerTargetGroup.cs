using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCurrentPlayerTargetGroup : MonoBehaviour
{
    private Controller_FSM controller;
    private Transform currentPlayerTarget;
    public Cinemachine.CinemachineTargetGroup TargetGroup;
    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller_FSM>();
        TargetGroup = GetComponent<Cinemachine.CinemachineTargetGroup>();
    }

    private void Start()
    {
        if(controller == null)
        {
            Debug.LogError("I haven't my ref to controller", this);
        }

    }

    private void OnEnable()
    {
        controller.OnChangeCurrentPlayerTarget += UpdateMyPlayerCurrentTarget;
    }

    private void OnDisable()
    {
        controller.OnChangeCurrentPlayerTarget -= UpdateMyPlayerCurrentTarget;
    }

    void UpdateMyPlayerCurrentTarget()
    {
        Debug.Log("I want to update my current Target", this);
    }
}
