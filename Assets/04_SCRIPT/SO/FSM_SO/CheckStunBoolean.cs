using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/CheckStunBoolean_Decision")]
public class CheckStunBoolean : Decision_SO
{
    public override bool Decide(PlayerController_FSM controller)
    {
        return controller.b_Stunned;
    }
}
