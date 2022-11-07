using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_SETInvulnerable")]
public class ACT_SETInvulnerable : Action_SO
{
    [SerializeField]
    private bool targetValue;

    public override void Act(Controller_FSM controller)
    {
        SetInvulnerableVariable(controller);
    }

    private void SetInvulnerableVariable(Controller_FSM controller)
    {
        controller.b_IsInvicible = targetValue;
    }

}
