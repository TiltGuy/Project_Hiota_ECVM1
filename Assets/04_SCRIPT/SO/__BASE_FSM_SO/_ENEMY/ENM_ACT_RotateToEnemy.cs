using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Enemy/ENM_ACT_RotateToEnemy")]
public class ENM_ACT_RotateToEnemy : Action_SO
{
    public override void Act( Controller_FSM controller )
    {
        //Move to Player
        RotateToPlayer(controller);
        Debug.Log("In Attack");
    }

    private void RotateToPlayer( Controller_FSM controller )
    {
        if(controller.currentCharacterTarget)
        {
            Vector3 DistToEnemy = controller.transform.position - controller.currentCharacterTarget.position;
            controller.NavAgent.updateRotation = false;
            Vector3 lookPos = -DistToEnemy;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, rotation, Time.deltaTime * controller.BrainAI.speedTurningWhenAttacking);
            
        }
        controller.NavAgent.updatePosition = false;
        controller.NavAgent.velocity = Vector3.zero;
    }

}
