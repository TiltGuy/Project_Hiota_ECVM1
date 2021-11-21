using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/CheckParryBoolean_Decision")]
public class CheckParryBoolean : Decision_SO
{
    public override bool Decide(PlayerController_FSM controller)
    {
        return controller.b_Parry;
    }
}
