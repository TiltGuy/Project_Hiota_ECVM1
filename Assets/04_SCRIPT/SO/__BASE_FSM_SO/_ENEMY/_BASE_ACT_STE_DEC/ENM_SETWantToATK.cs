using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Enemy/ENM_SETWantToATK")]
public class ENM_SETWantToATK : Action_SO
{
    public override void Act( Controller_FSM controller )
    {
        controller.BrainAI.b_WantToAttack = false;
    }
}
