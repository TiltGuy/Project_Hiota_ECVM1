using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/CheckAttackBetweenScalarVector_Decision")]

public class CheckAttackBetweenScalarVector : Decision_SO
{
    public float min;
    public float max;

    public override bool Decide(PlayerController_FSM controller)
    {
        return AttackWithScalarVector(controller, controller.scalarVector);
    }

    private bool AttackWithScalarVector(PlayerController_FSM controller, float scalarVector)
    {
        if (base.CheckInBetween(scalarVector, min, max))
        {
            if (base.CheckAttackInput(controller))
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }
}
