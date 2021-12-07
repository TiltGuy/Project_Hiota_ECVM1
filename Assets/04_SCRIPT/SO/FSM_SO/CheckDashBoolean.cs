using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/CheckDashBoolean_Decision")]
public class CheckDashBoolean : Decision_SO
{
    public override bool Decide(PlayerController_FSM controller)
    {
        return controller.b_WantDash;
    }
}
