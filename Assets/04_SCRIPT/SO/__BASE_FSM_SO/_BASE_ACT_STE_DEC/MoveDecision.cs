using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/MoveInputVector")]
public class MoveDecision : Decision_SO
{
    public override bool Decide(PlayerController_FSM controller)
    {
        return base.IsWantingToMove(controller);
    }
}
