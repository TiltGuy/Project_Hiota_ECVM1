using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_SETDashingValue")]
public class ACT_SETDashingValue : Action_SO
{
    [SerializeField]
    private bool targetBooleanValue;

    public override void Act(Controller_FSM controller)
    {
        SetDashingBooleanValue(controller, targetBooleanValue);
    }


}
