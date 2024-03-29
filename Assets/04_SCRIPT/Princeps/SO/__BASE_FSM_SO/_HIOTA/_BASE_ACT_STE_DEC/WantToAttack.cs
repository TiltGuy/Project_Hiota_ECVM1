using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/WantToAttack_Decision")]
public class WantToAttack : Decision_SO
{
    public override bool Decide(Controller_FSM controller)
    {
        return base.CheckAttackInput(controller);
    }
}
