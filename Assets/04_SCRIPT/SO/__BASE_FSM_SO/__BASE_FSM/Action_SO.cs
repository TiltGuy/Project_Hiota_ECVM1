using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action_SO : ScriptableObject
{
    public virtual void Act(Controller_FSM controller)
    {
        //Debug.Log("Ondebug!!!!", this);
    }

    public void ChangeAnimatorBoolean(Controller_FSM controller, string boolName, bool boolTargetValue)
    {
        controller.Animator.SetBool(boolName, boolTargetValue);
    }

    public void ChangeAnimatorTrigger(Controller_FSM controller, string triggerName)
    {
        controller.Animator.SetTrigger(triggerName);
    }

    public void SetCurrentAttackStats(Controller_FSM controller, AttackStats_SO attackStats_SO)
    {
        if(attackStats_SO)
        {
            controller.CurrentAttackStats = attackStats_SO;
        }
        else
        {
            Debug.Log("je n'ai pas de currentAttackStats");
        }
    }

    public void DebugCurrenState(Controller_FSM controller)
    {
        //Debug.Log("Hiota est dans l'état " + controller.currentState, this);
    }

    public void SetDashingBooleanValue(Controller_FSM controller, bool targetValue)
    {
        controller.b_IsDashing = targetValue;
    }

    public void SetParryingBooleanValue( Controller_FSM controller, bool targetValue )
    {
        controller.b_isParrying = targetValue;
    }

    public void SetStunnedBooleanValue(Controller_FSM controller, bool targetValue)
    {
        controller.b_Stunned = targetValue;
    }

    public void SetCurrentHitBoxStatut(Controller_FSM controller, bool targetValue)
    {
        if(targetValue)
        {
            controller.controllerAnim.UpdateBasicAttackHitBoxStatutTrue();
        }
        else
        {
            controller.controllerAnim.UpdateBasicAttackHitBoxStatutFalse();
        }
    }
}
