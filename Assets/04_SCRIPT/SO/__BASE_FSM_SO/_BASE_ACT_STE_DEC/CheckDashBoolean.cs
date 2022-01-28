using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/CheckDashBoolean_Decision")]
public class CheckDashBoolean : Decision_SO
{
    public override bool Decide(PlayerController_FSM controller)
    {
        //Debug.Log("Je checke" + CheckHiotaCanDash(controller));
        return CheckHiotaCanDash(controller);
    }

    public bool CheckHiotaCanDash(PlayerController_FSM controller)
    {
        if (controller.b_WantDash && controller.b_CanDash)
        {
            
            return true;
        }
        else
            return false;
    }
}
