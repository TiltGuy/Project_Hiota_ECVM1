using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ATT_SetRecoverAnimationBoolean")]
public class ATT_SetRecoverAnimationBoolean : Action_SO
{
    [SerializeField] private bool value;
    public override void Act(Controller_FSM controller)
    {
        controller.b_HaveFinishedRecoveringAnimation = value;
    }
}
