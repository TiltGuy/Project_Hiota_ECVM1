using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_UpdateHitBoxStatut")]
public class ACT_UpdateHitBoxStatut : Action_SO
{
    [SerializeField]
    private bool b_BooleanValue;

    public override void Act(Controller_FSM controller)
    {
        if(controller.controllerAnim.currentAttackHitbox)
        {
            base.SetCurrentHitBoxStatut(controller, b_BooleanValue);
        }
    }
}
