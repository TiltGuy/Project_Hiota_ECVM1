using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Enemy/ACT_CHASE")]
public class ENM_ACT_CHASE : Action_SO
{
    public float MaxDistance = 2f;
    public float MinDistance;
    public bool b_CanStrafeifCloseEnough = true;

    public override void Act( Controller_FSM controller )
    {

        //Move to Player
        if( controller.currentCharacterTarget )
        {
            MoveToPlayer(controller);
        }
    }

    private void MoveToPlayer (Controller_FSM controller)
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
        //Debug.Log(controller.GetLocalVelocity().normalized, controller);
        UpdateInputMoveVectorAnimator(controller);
        controller.NavAgent.updatePosition = true;
        if ( DistToEnemy.magnitude > MaxDistance )
        {
            ClosingDistance(controller);
           // Debug.Log("CLOSING UWU");
            if(controller.BrainAI.b_WantToAttack)
            {
                controller.BrainAI.AntiBennyHillTimer += Time.deltaTime;
                float bennyHillcount = controller.BrainAI.AntiBennyHillTimer;
                float bennyHillMax = controller.BrainAI.AntiBennyHillCountdown;
                controller.currentSpeed = Mathf.Lerp(controller.baseSpeed, controller.BrainAI.SpeedIncreasedWhenEnemyFleeing, bennyHillcount / bennyHillMax);
            }
            else
            {
                controller.BrainAI.AntiBennyHillTimer = 0;
            }
        }
        else if(DistToEnemy.magnitude < MinDistance)
        {
            IncreaseDistance(controller, DistToEnemy);
          //  Debug.Log("Retreat YAMETE !!!");
        }
        else 
        {
            Strafing(controller, dir);
          //  Debug.Log("NIGERUNDAYOO !!!");
        }
        //Debug.Log(controller.BrainAI.AntiBennyHillTimer, this);
    }

    private static void ClosingDistance( Controller_FSM controller )
    {
        controller.NavAgent.speed = controller.charSpecs.CharStats_SO.BaseSpeed;
        controller.NavAgent.SetDestination(controller.currentCharacterTarget.position);
    }
    private static void IncreaseDistance( Controller_FSM controller, Vector3 dir)
    {
        controller.NavAgent.speed = controller.charSpecs.CharStats_SO.BaseSpeed;
        controller.NavAgent.SetDestination(controller.transform.position + dir);
    }

    private void Strafing( Controller_FSM controller, Vector3 dir )
    {
        if ( b_CanStrafeifCloseEnough )
        {
            controller.NavAgent.speed = controller.charSpecs.CharStats_SO.BaseSpeedWhenStrafing;
            controller.NavAgent.SetDestination(controller.transform.position + dir);
        }
        else
        {
            controller.NavAgent.speed = controller.charSpecs.CharStats_SO.BaseSpeed;
            controller.NavAgent.SetDestination(controller.transform.position);
        }
    }

    private static void UpdateInputMoveVectorAnimator( Controller_FSM controller )
    {
        Vector3 LocalVel = controller.GetLocalVelocity();
        controller.characterAnimator.SetFloat("Input_Move_VectorX", LocalVel.x);
        controller.characterAnimator.SetFloat("Input_Move_VectorZ", LocalVel.z);
    }
}
