using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action_SO : ScriptableObject
{
    public virtual void Act(PlayerController_FSM controller)
    {
        //Debug.Log("Ondebug!!!!", this);
    }

    public void ChangeAnimatorBoolean(PlayerController_FSM controller, string boolName, bool boolTargetValue)
    {
        controller.Animator.SetBool(boolName, boolTargetValue);
    }

    public void ChangeAnimatorTrigger(PlayerController_FSM controller, string triggerName)
    {
        controller.Animator.SetTrigger(triggerName);
    }

    public void SetCurrentAttackStats(PlayerController_FSM controller, AttackStats_SO attackStats_SO)
    {
        controller.BasicAttackStats = attackStats_SO;
    }

    public void DebugCurrenState(PlayerController_FSM controller)
    {
        Debug.Log("Hiota est dans l'état " + controller.currentState, this);
    }
}
