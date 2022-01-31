using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_SETNormalParry")]
public class ACT_SETNormalParry : Action_SO
{

    public override void Act(Controller_FSM controller)
    {
        SetPefectToNormalParry(controller);
    }

    private void SetPefectToNormalParry(Controller_FSM controller)
    {
        if(controller.perfectTimer < controller.timerPerfectParry)
        {
            controller.perfectTimer += Time.deltaTime;
            
        }
        else
        {
            controller.b_PerfectParry = false;
            controller.b_NormalParry = true;
        }
        Debug.Log("PefectTimer : " + controller.perfectTimer);
    }

}
