using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_HOOKED")]
public class ACT_HOOKED :Action_SO
{
    private Vector3 HookedDirection;
    [SerializeField]
    private float TimeOfHook = 1f;
    Transform lastHooker;

    public override void Act( Controller_FSM controller )
    {
        //do the push Dummy
        if ( controller.B_IsTouched )
        {
            controller.TimerOfHook += Time.deltaTime;
            lastHooker = controller.LastHooker;
            Debug.Log("Touched by hook");
            HookedDirection = -controller.DirectionHitReact;
            if(controller.characontroller)
            {
                controller.characontroller.Move(HookedDirection * Time.deltaTime * TimeOfHook);
            }
            else if(controller.NavAgent)
            {
                controller.NavAgent.speed = controller.BrainAI.SpeedIncreasedWhenEnemyFleeing * (1 / controller.TimerOfHook / TimeOfHook);
                Debug.Log(lastHooker.name, controller);
                Vector3 offset = Vector3.Slerp(controller.transform.position, lastHooker.GetComponent<Controller_FSM>().eyes.position, controller.TimerOfHook / TimeOfHook);
                controller.NavAgent.SetDestination(offset);
            }

        }
    }
}
