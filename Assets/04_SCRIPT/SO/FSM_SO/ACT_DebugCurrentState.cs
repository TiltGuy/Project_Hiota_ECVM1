using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/ACT_DebugCurrentState")]
public class ACT_DebugCurrentState : Action_SO
{
    public override void Act(PlayerController_FSM controller)
    {
        base.Act(controller);
        base.DebugCurrenState(controller);
    }
}