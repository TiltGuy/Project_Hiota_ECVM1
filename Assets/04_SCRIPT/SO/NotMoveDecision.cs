using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/NotMoveInputVector_Decision")]
public class NotMoveDecision : Decision_SO
{
    public override bool Decide(PlayerController_FSM controller)
    {
        return base.NotWantingToMove(controller);
    }
}
