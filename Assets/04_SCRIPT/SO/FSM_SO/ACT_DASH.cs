using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/ACT_DASH")]
public class ACT_DASH : Action_SO
{

    [SerializeField]
    private float speedMovementAction;
    private Vector3 dashDirection;

    public override void Act(PlayerController_FSM controller)
    {
        ConstraintMove(controller);
    }

    private void ConstraintMove(PlayerController_FSM controller)
    {
        
        

        dashDirection = controller.lastDirectionInput;
        controller.characontroller.Move(dashDirection * Time.deltaTime * speedMovementAction);

    }
}
