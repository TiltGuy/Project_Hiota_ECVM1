
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ATT_SetHaveHittenEnnemies")]
public class ATT_SetHaveHittenEnnemies : Action_SO
{
    [SerializeField] private bool value;
    public override void Act(Controller_FSM controller)
    {
        controller.B_HaveSuccessfullyHitten = value;
    }
}
