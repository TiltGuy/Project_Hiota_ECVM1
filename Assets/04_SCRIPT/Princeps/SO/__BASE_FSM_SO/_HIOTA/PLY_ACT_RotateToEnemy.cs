﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/PLY_ACT_RotateToEnemy")]
public class PLY_ACT_RotateToEnemy : Action_SO
{
    public override void Act( Controller_FSM controller )
    {
        RotateToPlayer(controller);
    }

    private void RotateToPlayer( Controller_FSM controller )
    {
        Vector3 DistToEnemy;
        if (controller.currentCharacterTarget)
        {
            DistToEnemy = controller.transform.position - controller.currentCharacterTarget.position;
        }
        else
        {
            Vector3 inputToPlane = new Vector3(controller.m_InputMoveVector.x,0, controller.m_InputMoveVector.y);
            //DistToEnemy = controller.transform.forward - (controller.transform.forward + inputToPlane);
            DistToEnemy = controller.m_camF * controller.m_InputMoveVector.y + controller.m_camR * controller.m_InputMoveVector.x;
            DistToEnemy *= -1f ;
        }

        Vector3 lookPos = -DistToEnemy;
        lookPos.y = 0;
        if(lookPos.magnitude > 0f)
        {
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, rotation, Time.deltaTime * controller.m_speedTurnWhenAttack);

        }
    }
}
