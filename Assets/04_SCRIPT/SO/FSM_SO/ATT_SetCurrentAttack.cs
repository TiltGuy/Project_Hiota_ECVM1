using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/ATT_SetCurrentAttack_Action")]
public class ATT_SetCurrentAttack : Action_SO
{
    public AttackStats_SO currentAttackStats;

    public override void Act(PlayerController_FSM controller)
    {
        base.SetCurrentAttackStats(controller, currentAttackStats);
    }
}
