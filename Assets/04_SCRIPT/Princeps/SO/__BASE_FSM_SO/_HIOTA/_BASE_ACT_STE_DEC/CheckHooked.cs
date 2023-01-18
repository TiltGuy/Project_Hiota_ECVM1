using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/CheckHooked_Decision")]
public class CheckHooked : Decision_SO
{
    public override bool Decide( Controller_FSM controller )
    {
        return controller.b_Hooked;
    }
}
