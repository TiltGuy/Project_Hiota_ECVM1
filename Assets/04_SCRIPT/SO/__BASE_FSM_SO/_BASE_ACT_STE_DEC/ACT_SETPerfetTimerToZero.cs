using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_SETPerfetTimerToZero")]
public class ACT_SETPerfetTimerToZero : Action_SO
{
    public override void Act(Controller_FSM controller)
    {
        ResetPerfectTimerToZero(controller);
    }

    private void ResetPerfectTimerToZero(Controller_FSM controller)
    {
        controller.perfectTimer = 0f;
    }
}
