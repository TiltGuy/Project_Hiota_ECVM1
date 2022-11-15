using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_HOOKED")]
public class ACT_HOOKED :Action_SO
{
    private Vector3 HookedDirection;
    [SerializeField]
    private float speedMovementAction = 1f;

    public override void Act( Controller_FSM controller )
    {
        //do the push Dummy
        if ( controller.B_IsTouched )
        {
            HookedDirection = -controller.DirectionHitReact;
            if(controller.characontroller)
            {
                controller.characontroller.Move(HookedDirection * Time.deltaTime * speedMovementAction);
            }
            else if(controller.NavAgent)
            {

            }

        }
    }
}
