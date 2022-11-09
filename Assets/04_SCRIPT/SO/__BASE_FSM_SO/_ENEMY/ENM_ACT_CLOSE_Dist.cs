using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Enemy/ENM_ACT_CLOSE_Dist")]
public class ENM_ACT_CLOSE_Dist : Action_SO
{
    public float stopDistance = 2f;
    public override void Act( Controller_FSM controller )
    {

        //Move to Player
        MoveToPlayer(controller);
    }

    private void MoveToPlayer( Controller_FSM controller )
    {
        Vector3 DistToEnemy = controller.transform.position - controller.currentCharacterTarget.position;
        controller.NavAgent.updateRotation = false;
        Vector3 dir = Vector3.Cross(DistToEnemy, Vector3.up);
        dir *= controller.BrainAI.factorStrafecrossDirection;
        Vector3 lookPos = -DistToEnemy;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, rotation, Time.deltaTime * controller.BrainAI.speedOfTurningEnemyWhenFocus);
        //controller.transform.rotation = rotation;
        Debug.Log(controller.GetLocalVelocity().normalized, controller);
        UpdateInputMoveVectorAnimator(controller);

        if ( DistToEnemy.magnitude > stopDistance )
        {
            controller.NavAgent.SetDestination(controller.currentCharacterTarget.position);
        }
        else
        {
            controller.NavAgent.SetDestination(controller.transform.position + dir);
        }
    }

    private static void UpdateInputMoveVectorAnimator( Controller_FSM controller )
    {
        Vector3 LocalVel = controller.GetLocalVelocity();
        controller.characterAnimator.SetFloat("Input_Move_VectorX", LocalVel.x);
        controller.characterAnimator.SetFloat("Input_Move_VectorZ", LocalVel.z);
    }
}
