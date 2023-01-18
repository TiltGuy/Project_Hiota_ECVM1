using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/Idle")]
public class Idle_Action : Action_SO
{
    public override void Act(Controller_FSM controller)
    {

    }

    private void Idle(Controller_FSM controller)
    {
        Debug.Log("Hi, I'm just Idling somewhere");
    }
}
