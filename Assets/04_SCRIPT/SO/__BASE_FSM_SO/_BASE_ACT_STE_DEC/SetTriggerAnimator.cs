using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/SetTriggerAnimator_Action")]
public class SetTriggerAnimator : Action_SO
{
    public string NameofTrigger;

    public override void Act(Controller_FSM controller)
    {
        ChangeAnimatorTrigger(controller, NameofTrigger);
    }
}
