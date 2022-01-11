using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/CheckGrounded_Decision")]
public class CheckGrounded : Decision_SO
{
    public override bool Decide(PlayerController_FSM controller)
    {
        return controller._isGrounded;
    }
}
