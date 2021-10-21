using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/FocusModeDecision")]
public class FocusModeDecision : Decision_SO
{
    public override bool Decide(PlayerController_FSM controller)
    {
        return FocusModeCheckBoolean(controller);
    }
}
