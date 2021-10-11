using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiotaIdleState : HiotaBaseState
{

    public override void EnterState(HiotaController_FSM hiota)
    {

    }

    public override void HandleInput(HiotaController_FSM hiota)
    {
    }

    public override void LogicUpdate(HiotaController_FSM hiota)
    {

        if (hiota.b_IsFocusing)
        {
            hiota.RotatePlayerNorY(hiota.currentHiotaTarget);
            hiota.Hiota_Anim.SetBool("Is_Focusing", true);
        }
        else
        {
            Quaternion finalrot = Quaternion.LookRotation(hiota.directionToGo, hiota.transform.up);
            //hiota.transform.rotation = Quaternion.Lerp(hiota.transform.rotation, finalrot, hiota.m_turnSpeed * Time.deltaTime);
        }

        if (hiota.m_InputMoveVector != Vector2.zero)
        {
            hiota.TransitionToState(hiota.MovingState);
        }

        if(!hiota.IsGrounded())
        {
            hiota.TransitionToState(hiota.FallingState);
        }

        if (hiota.b_WantDash)
        {
            hiota.TransitionToState(hiota.DashingState);
            hiota.b_WantDash = false;
        }
    }

    public override void OnCollisionEnter(HiotaController_FSM hiota)
    {
        
    }

    public override void PhysicsUpdate(HiotaController_FSM hiota)
    {
        
    }

    public override void Exit(HiotaController_FSM hiota)
    {
        //Debug.Log("Exit_BaseState");
    }

}
