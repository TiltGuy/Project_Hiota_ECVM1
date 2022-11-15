using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision_SO : ScriptableObject
{
    public virtual bool Decide(Controller_FSM controller)
    {
        bool checkBoolean = true;
        Debug.Log(checkBoolean);
        return checkBoolean;
    }

    public bool IsWantingToMove(Controller_FSM controller)
    {
        Vector2 m_InputVector = controller.m_InputMoveVector;

        if(m_InputVector != Vector2.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool NotWantingToMove(Controller_FSM controller)
    {
        Vector2 m_InputVector = controller.m_InputMoveVector;

        if (m_InputVector == Vector2.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool FocusModeCheckBoolean(Controller_FSM controller)
    {
        return controller.b_IsFocusing;
    }

    public bool CheckAttackInput(Controller_FSM controller)
    {
        return controller.b_AttackInput;
    }

    public bool CheckStunBoolean(Controller_FSM controller)
    {
        return controller.b_Stunned;
    }

    public bool CheckIsParryingBoolean(Controller_FSM controller)
    {
        return controller.b_IsInputParry;
    }

    public bool CheckIsParryingPerfectlyBoolean(Controller_FSM controller)
    {
        return controller.b_IsPerfectlyParrying;
    }

    public bool CheckDashBoolean(Controller_FSM controller)
    {
        return controller.b_WantDash;
    }

    public bool CheckInBetween(float numberToTest, float min, float max)
    {
        if (numberToTest >= min && numberToTest <= max)
            return true;
        else
            return false;
    }

    public bool CheckIfCurrentCharacterTarget(Controller_FSM controller)
    {
        if ( controller.currentCharacterTarget )
            return true;
        else
            return false;
    }
}
