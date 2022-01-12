using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision_SO : ScriptableObject
{
    public virtual bool Decide(PlayerController_FSM controller)
    {
        bool checkBoolean = true;
        Debug.Log(checkBoolean);
        return checkBoolean;
    }

    public bool IsWantingToMove(PlayerController_FSM controller)
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

    public bool NotWantingToMove(PlayerController_FSM controller)
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

    public bool FocusModeCheckBoolean(PlayerController_FSM controller)
    {
        return controller.b_IsFocusing;
    }

    public bool CheckAttackInput(PlayerController_FSM controller)
    {
        return controller.b_AttackInput;
    }

    public bool CheckStunBoolean(PlayerController_FSM controller)
    {
        return controller.b_Stunned;
    }

    public bool CheckBoolean(PlayerController_FSM controller)
    {
        return controller.b_Parry;
    }

    public bool CheckDashBoolean(PlayerController_FSM controller)
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
}
