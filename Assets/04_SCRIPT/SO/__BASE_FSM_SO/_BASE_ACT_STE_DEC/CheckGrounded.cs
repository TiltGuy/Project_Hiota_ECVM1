using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/CheckGrounded_Decision")]
public class CheckGrounded : Decision_SO
{
    public override bool Decide(Controller_FSM controller)
    {
        return controller._isGrounded;
    }
}
