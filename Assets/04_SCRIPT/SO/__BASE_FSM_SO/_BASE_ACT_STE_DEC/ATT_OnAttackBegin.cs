using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/DelegateEvent/ATT_OnAttackBegin")]
public class ATT_OnAttackBegin : Action_SO
{

    private Transform _currentTarget;


    public override void Act(Controller_FSM controller)
    {
        UpdateCurrentTarget(controller);
        CallOnAttackBegin(controller);
    }

    void UpdateCurrentTarget(Controller_FSM controller)
    {
        if (controller.currentHiotaTarget)
        {
            _currentTarget = controller.currentHiotaTarget;
        }
        else
        {
            Debug.LogWarning("Try to acces to current Hiota Target but Nobody's here", this);
            _currentTarget = null;
        }
    }

    void CallOnAttackBegin(Controller_FSM controller)
    {
        //Debug.Log("Calling On Attack Begin");
        controller.OnAttackBegin();
    }


}
