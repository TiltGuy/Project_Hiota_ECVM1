using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_ResetPerfectlyParrying")]
public class ACT_ResetPerfectlyParrying: Action_SO
{

    public override void Act(Controller_FSM controller)
    {
        controller.b_IsPerfectlyParrying = false;
        controller.StopCoroutine("SetIsPerfectlyParryingCoroutine");
    }
}
