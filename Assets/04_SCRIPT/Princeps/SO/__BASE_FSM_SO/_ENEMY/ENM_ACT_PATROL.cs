using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Enemy/ACT_PATROL")]
public class ENM_ACT_PATROL : Action_SO
{
    public override void Act( Controller_FSM controller )
    {
        if(controller.waypoints.Count > 0)
        {
            PatrolBetweenPoints( controller, controller.waypoints);
        }
        UpdateAnimatorBlendValue(controller);

    }

    private void PatrolBetweenPoints( Controller_FSM controller, List<Transform> waypoints)
    {
        if ( (controller.NavAgent.destination - controller.transform.position).magnitude < 2f )
        {
            if ( waypoints.Count >= 1 )
            {
                controller.NavAgent.SetDestination(controller.RandomNavmeshLocation(4f));
            }
        }
    }

    private void UpdateAnimatorBlendValue(Controller_FSM controller)
    {
        float currentAgentSpeed = 0f;
        currentAgentSpeed = Mathf.Clamp01((controller.NavAgent.destination - controller.transform.position).magnitude);
        controller.characterAnimator.SetFloat("Input_Move_Vector", currentAgentSpeed);
    }
}
