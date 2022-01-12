using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/ACT_DASH")]
public class ACT_DASH : Action_SO
{

    [SerializeField]
    private float speedMovementAction;
    [SerializeField]
    private Vector3 dashDirection;
    public bool b_ConstraintDash = false;

    public override void Act(PlayerController_FSM controller)
    {
        ConstraintMove(controller);
    }

    private void ConstraintMove(PlayerController_FSM controller)
    {
        
        
        if(!b_ConstraintDash)
        {
            dashDirection = controller.lastDirectionInput;
            controller.characontroller.Move(dashDirection * Time.deltaTime * speedMovementAction);
        }
        else
        {
            controller.characontroller.Move(controller.transform.forward * Time.deltaTime * speedMovementAction);
        }

    }
}
