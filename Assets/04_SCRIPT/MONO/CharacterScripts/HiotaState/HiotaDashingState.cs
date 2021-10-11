using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiotaDashingState : HiotaBaseState
{

    
    private Vector3 dashVector;
    private Vector3 dragVector;
    private float dashTimer = 0f;
    
    public override void EnterState(HiotaController_FSM hiota)
    {
        dragVector = new Vector3(0.2f, .2f, .2f);
        dashTimer = 0f;
    }

    

    public override void HandleInput(HiotaController_FSM hiota)
    {
        
    }

    public override void LogicUpdate(HiotaController_FSM hiota)
    {
        Debug.Log("Dash");

        dashTimer += Time.deltaTime;
        //Set the Dash Vector Strength
        //dashVector += Vector3.Scale(hiota.characontroller.velocity,
        //                           hiota.dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * dragVector.x + 1)) / -Time.deltaTime),
        //                                                      0,
        //                                                      (Mathf.Log(1f / (Time.deltaTime * dragVector.z + 1)) / -Time.deltaTime)));

        //Set the movement of the character
        hiota.characontroller.Move(hiota.dashDirection * Time.deltaTime * hiota.dashSpeed);

        //Imitation of the Drag
        dashVector.x /= 1 + dragVector.x * Time.deltaTime;
        dashVector.y /= 1 + dragVector.y * Time.deltaTime;
        dashVector.z /= 1 + dragVector.z * Time.deltaTime;

        
        //Debug.Log(dashVector);

        if (hiota.maxDashTime < dashTimer)
        {
            hiota.TransitionToState(hiota.IdleState);
        }


    }

    public override void Exit(HiotaController_FSM hiota)
    {
        dashTimer = 0f;
    }

    public override void OnCollisionEnter(HiotaController_FSM hiota)
    {
        
    }

    public override void PhysicsUpdate(HiotaController_FSM hiota)
    {
        
    }

}
