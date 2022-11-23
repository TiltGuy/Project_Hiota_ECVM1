using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Enemy/CHK_currCharaTarget")]
public class CHK_currCharaTarget : Decision_SO
{
    public override bool Decide( Controller_FSM controller )
    {
        //Debug.Log(controller.currentCharacterTarget, this);
        if ( controller.currentCharacterTarget != null )
            return true;
        else
            return false;
    }
}
