using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_HOOKED")]
public class ACT_HOOKED :Action_SO
{
    private Vector3 HookedDirection;
    [SerializeField]
    private float speedMovementAction = 1f;
    [SerializeField] private AnimationCurve MovementSpeedCurve;

    public override void Act( Controller_FSM controller )
    {
        //do the push Dummy
        if ( controller.B_IsTouched )
        {
            float currentStateTime = controller.currentState.stateTimer/controller.currentState.stateDuration;
            float currentSpeedMovement = MovementSpeedCurve.Evaluate( currentStateTime );
            if(controller.characontroller)
            {
                Vector3 currentMovement = Vector3.MoveTowards(controller.transform.position, controller.currentStriker.position, currentSpeedMovement);
                controller.characontroller.Move(currentMovement * Time.deltaTime);
            }
            else if(controller.NavAgent)
            {
                controller.NavAgent.updatePosition = false;

            }

        }
    }
}
