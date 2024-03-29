﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/FocusModeDecision")]
public class FocusModeDecision : Decision_SO
{
    public override bool Decide(Controller_FSM controller)
    {
        return FocusModeCheckBoolean(controller);
    }
}
