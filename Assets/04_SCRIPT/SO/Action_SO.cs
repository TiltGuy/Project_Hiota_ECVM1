﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action_SO : ScriptableObject
{
    public virtual void Act(PlayerController_FSM controller)
    {
        Debug.Log("Ondebug!!!!");
    }

    public void ChangeAnimatorBoolean(PlayerController_FSM controller, string boolName, bool boolTargetValue)
    {
        controller.Animator.SetBool(boolName, boolTargetValue);
    }
}