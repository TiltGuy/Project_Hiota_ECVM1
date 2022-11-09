using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Enemy/CHK_IfGoATK")]
public class CHK_IfGoATK : Decision_SO
{
    public override bool Decide( Controller_FSM controller )
    {
        return controller.BrainAI.b_WantToAttack;
    }
}
