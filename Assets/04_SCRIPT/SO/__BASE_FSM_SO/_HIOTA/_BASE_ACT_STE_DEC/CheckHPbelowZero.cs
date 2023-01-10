using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/CheckHPbelowZero")]

public class CheckHPbelowZero : Decision_SO
{
    public override bool Decide( Controller_FSM controller )
    {
        if ( controller.charSpecs.Health <= 0 )
        {
            return true;
        }
        return false;
    }
}
