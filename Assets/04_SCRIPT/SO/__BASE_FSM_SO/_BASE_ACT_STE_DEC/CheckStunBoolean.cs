using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/CheckStunBoolean_Decision")]
public class CheckStunBoolean : Decision_SO
{
    public override bool Decide(Controller_FSM controller)
    {
        return controller.b_Stunned;
    }
}
