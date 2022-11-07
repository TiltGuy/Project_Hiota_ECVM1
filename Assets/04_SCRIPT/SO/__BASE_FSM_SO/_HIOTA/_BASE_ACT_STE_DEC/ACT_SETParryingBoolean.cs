using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_SETParryingBoolean")]
public class ACT_SETParryingBoolean : Action_SO
{
    [SerializeField] private bool targetBooleanValue;
    public override void Act( Controller_FSM controller )
    {
        SetParryingBooleanValue(controller, targetBooleanValue);
    }
}
