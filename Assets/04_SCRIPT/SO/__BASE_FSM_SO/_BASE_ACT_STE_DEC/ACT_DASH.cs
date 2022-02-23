using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_DASH")]
public class ACT_DASH : Action_SO
{

    [SerializeField]
    private float speedMovementAction;
    private Vector3 dashDirection;
    [SerializeField]
    private bool b_DashToEnemy;
    public bool b_ConstraintDash = false;

    public override void Act(Controller_FSM controller)
    {
        ConstraintMove(controller);
        if(b_DashToEnemy)
        {
            if(controller.b_IsFocusing)
            {
                //Dash to enemy
                RotateEntityToEnemy(controller);
            }
        }
    }

    private void ConstraintMove(Controller_FSM controller)
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

    private void RotateEntityToEnemy(Controller_FSM controller)
    {
        Vector3 hiotaPos = controller.transform.position;
        Vector3 dir = (controller.currentHiotaTarget.position - hiotaPos).normalized;
        dir.y = 0;
        controller.directionToFocus = dir;
        //Debug.DrawLine(hiotaPos, hiotaPos + dir * 10, Color.red, Mathf.Infinity);
        Quaternion finalrot = Quaternion.LookRotation(controller.directionToFocus, Vector3.up);
        //controller.transform.rotation = Quaternion.LookRotation(controller.directionToFocus, Vector3.up);
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation,finalrot, 0.0085f);
    }
}
