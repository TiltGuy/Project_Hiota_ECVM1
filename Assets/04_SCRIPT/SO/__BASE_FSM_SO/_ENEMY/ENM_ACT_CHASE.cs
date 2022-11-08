using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Enemy/ACT_CHASE")]
public class ENM_ACT_CHASE : Action_SO
{
    public float stopDistance = 2f;
    public override void Act( Controller_FSM controller )
    {

        //Move to Player
        MoveToPlayer(controller);
    }

    private void MoveToPlayer (Controller_FSM controller)
    {
        Vector3 DistToEnemy = controller.transform.position - controller.currentCharacterTarget.position;

        if(DistToEnemy.magnitude > stopDistance)
        {

            controller.NavAgent.SetDestination(controller.currentCharacterTarget.position);
        }
        else
        {
            Vector3 dir = Vector3.Cross(DistToEnemy, Vector3.up);
            controller.NavAgent.SetDestination(controller.transform.position + dir);
            Vector3 lookPos = -DistToEnemy;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            controller.transform.rotation =
                Quaternion.Slerp(controller.transform.rotation, rotation, Time.deltaTime * 50f);
        }
    }
}
