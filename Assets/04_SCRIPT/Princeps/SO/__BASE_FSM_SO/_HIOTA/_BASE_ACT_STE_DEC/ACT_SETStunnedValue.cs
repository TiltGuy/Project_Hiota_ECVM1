using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_SETStunnedValue")]
public class ACT_SETStunnedValue : Action_SO
{
    [SerializeField]
    private bool targetBooleanValue;

    public override void Act(Controller_FSM controller)
    {
        SetStunnedBooleanValue(controller, targetBooleanValue);
    }
}
