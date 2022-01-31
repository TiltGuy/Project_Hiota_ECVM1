using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/MoveInputVector")]
public class MoveDecision : Decision_SO
{
    public override bool Decide(Controller_FSM controller)
    {
        return base.IsWantingToMove(controller);
    }
}
