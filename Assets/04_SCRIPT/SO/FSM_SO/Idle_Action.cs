using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Idle")]
public class Idle_Action : Action_SO
{
    public override void Act(PlayerController_FSM controller)
    {

    }

    private void Idle(PlayerController_FSM controller)
    {
        Debug.Log("Hi, I'm just Idling somewhere");
    }
}
