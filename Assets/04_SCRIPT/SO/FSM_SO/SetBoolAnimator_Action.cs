using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableStateMachine/Actions/SetBoolAnimator_Action")]
public class SetBoolAnimator_Action : Action_SO
{
    public string NameofBoolean;
    public bool TargetValue;

    public override void Act(PlayerController_FSM controller)
    {
        ChangeAnimatorBoolean(controller, NameofBoolean, TargetValue);
    }
    


}
