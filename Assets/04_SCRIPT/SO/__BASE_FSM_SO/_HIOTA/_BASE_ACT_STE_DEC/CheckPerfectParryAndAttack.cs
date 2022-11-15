using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/CheckPerfectParryAndAttack")]
public class CheckPerfectParryAndAttack : Decision_SO
{
    public override bool Decide(Controller_FSM controller)
    {
        return ParryingPerfectlyAndAttacking( controller);
    }

    private bool ParryingPerfectlyAndAttacking(Controller_FSM controller)
    {
        if (CheckIsParryingPerfectlyBoolean(controller) && CheckAttackInput(controller))
        {
            return true;
        }
        else
            return false;
    }
}
