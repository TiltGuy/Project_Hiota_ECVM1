using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/CheckRecoverAnimationFullfilled_Decision")]
public class CheckRecoverAnimationFullfilled : Decision_SO
{
    public override bool Decide(Controller_FSM controller)
    {
        return controller.b_HaveFinishedRecoveringAnimation;
    }
}
