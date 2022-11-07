using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/NotMoveInputVector_Decision")]
public class NotMoveDecision : Decision_SO
{
    public override bool Decide(Controller_FSM controller)
    {
        return base.NotWantingToMove(controller);
    }
}
