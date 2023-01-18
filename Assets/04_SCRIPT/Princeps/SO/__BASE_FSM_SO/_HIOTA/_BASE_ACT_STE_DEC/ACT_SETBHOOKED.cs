using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Both/ACT_SETBHOOKED")]
public class ACT_SETBHOOKED : Action_SO
{
    public bool targetValueOfB_Hooked;

    public override void Act( Controller_FSM controller )
    {
        controller.b_Hooked = targetValueOfB_Hooked;
    }
}
