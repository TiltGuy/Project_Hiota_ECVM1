using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/CheckParryBoolean_Decision")]
public class CheckParryBoolean : Decision_SO
{
    public override bool Decide(PlayerController_FSM controller)
    {
        return CanParryAndWantParry(controller);
    }

    private bool CanParryAndWantParry(PlayerController_FSM controller)
    {
        if (controller.b_CanParry && controller.b_Parry)
        {
            return true;
        }
        else
            return false;
    }
}
