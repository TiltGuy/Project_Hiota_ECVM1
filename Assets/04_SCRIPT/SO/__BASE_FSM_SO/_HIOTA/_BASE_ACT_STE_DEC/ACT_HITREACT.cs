using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_HITREACT")]
public class ACT_HITREACT : Action_SO
{
    private Vector3 dashDirection;
    [SerializeField]
    private float speedMovementAction = 1f;

    public override void Act(Controller_FSM controller)
    {
        //do the push Dummy
        if (controller.B_IsTouched)
        {
            dashDirection = controller.DirectionHitReact;
            controller.characontroller.Move(dashDirection * Time.deltaTime * speedMovementAction);
        }
    }
}
