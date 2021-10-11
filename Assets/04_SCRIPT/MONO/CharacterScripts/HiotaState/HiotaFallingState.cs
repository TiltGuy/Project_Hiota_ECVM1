using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiotaFallingState : HiotaBaseState
{
    public override void EnterState(HiotaController_FSM hiota)
    {
        
    }

    public override void Exit(HiotaController_FSM hiota)
    {
        
    }

    public override void HandleInput(HiotaController_FSM hiota)
    {
        //INITIALISATION AND UPDATE OF FORWARD & RIGHT VECTORS OF THE CAMERA
        hiota.m_camF = hiota.m_cameraBaseDirection.forward;
        hiota.m_camR = hiota.m_cameraBaseDirection.right;

        hiota.m_camF.y = 0;
        hiota.m_camR.y = 0;
        hiota.m_camF = hiota.m_camF.normalized;
        hiota.m_camR = hiota.m_camR.normalized;

        hiota.directionToGo = hiota.m_camF * hiota.m_InputMoveVector.y + hiota.m_camR * hiota.m_InputMoveVector.x;
    }

    public override void LogicUpdate(HiotaController_FSM hiota)
    {
        hiota.currentDirection = hiota.directionToGo;
        hiota.currentDirection.y = hiota.gravity;
        hiota.characontroller.Move(hiota.currentDirection * Time.deltaTime * hiota.m_speed);

        if (hiota.b_IsFocusing)
        {
            hiota.RotatePlayerNorY(hiota.currentHiotaTarget);
        }
        else if (hiota.m_inputsKeyBoard != Vector3.zero)
        {
            Quaternion finalrot = Quaternion.LookRotation(hiota.directionToGo, Vector3.up);
            hiota.transform.rotation = Quaternion.Lerp(hiota.transform.rotation, finalrot, hiota.m_turnSpeed * Time.deltaTime);
        }

        if (hiota.IsGrounded())
        {
            hiota.TransitionToState(hiota.IdleState);
        }
    }

    public override void OnCollisionEnter(HiotaController_FSM hiota)
    {
    }

    public override void PhysicsUpdate(HiotaController_FSM hiota)
    {
    }

        
}
