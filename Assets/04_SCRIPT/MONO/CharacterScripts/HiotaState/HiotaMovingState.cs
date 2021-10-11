using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiotaMovingState : HiotaBaseState
{
    public override void EnterState(HiotaController_FSM hiota)
    {

        hiota.Hiota_Anim.SetBool("Is_Moving_Free", true);

    }

    public override void Exit(HiotaController_FSM hiota)
    {
        hiota.Hiota_Anim.SetBool("Is_Moving_Free", false);
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
        hiota.dashDirection = hiota.directionToGo.normalized;
        //moveDirection = directionToGo;

        //if (Input.GetAxis("Horizontal")!= 0)
        //{
        //    dir1 = hiota.m_cameraBaseDirection.right.x;
        //    dir2 = hiota.m_cameraBaseDirection.right.z;
        //}
        //else if(Input.GetAxis("Vertical") !=0)
        //{
        //    dir1 = hiota.m_cameraBaseDirection.forward.x;
        //    dir2 = hiota.m_cameraBaseDirection.forward.z;
        //}
        //Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //moveDirection = new Vector3(dir1 * inputDirection.x, 0, dir2 * inputDirection.y);
        //if (Input.GetAxis("Horizontal") > 0)
        //{
        //    moveDirection = new Vector3(hiota.m_cameraBaseDirection.right.x, 0, hiota.m_cameraBaseDirection.right.z);
        //}
        //else if(Input.GetAxis("Horizontal") < 0)
        //{
        //    moveDirection = new Vector3(-hiota.m_cameraBaseDirection.right.x, 0, -hiota.m_cameraBaseDirection.right.z);
        //}

        //else if (Input.GetAxis("Vertical") > 0)
        //{
        //    moveDirection = new Vector3(hiota.m_cameraBaseDirection.forward.x, 0, hiota.m_cameraBaseDirection.forward.z);
        //}
        //else if (Input.GetAxis("Vertical") < 0)
        //{
        //    moveDirection = new Vector3(-hiota.m_cameraBaseDirection.forward.x, 0, -hiota.m_cameraBaseDirection.forward.z);
        //}





    }

    public override void LogicUpdate(HiotaController_FSM hiota)
    {
        hiota.characontroller.Move(hiota.directionToGo * Time.deltaTime * hiota.m_speed);
        hiota.Hiota_Anim.SetFloat("DirectX_FocusMode", hiota.m_InputMoveVector.x);
        hiota.Hiota_Anim.SetFloat("DirectZ_FocusMode", hiota.m_InputMoveVector.y);


        if (hiota.b_IsFocusing)
        {
            hiota.RotatePlayerNorY(hiota.currentHiotaTarget);
            hiota.Hiota_Anim.SetBool("Is_Focusing", true);
        }
        else
        {
            Quaternion finalrot = Quaternion.LookRotation(hiota.directionToGo, Vector3.up);
            hiota.transform.rotation = Quaternion.Lerp(hiota.transform.rotation, finalrot, hiota.m_turnSpeed * Time.deltaTime);
            hiota.Hiota_Anim.SetBool("Is_Focusing", false);
        }



        if (hiota.m_InputMoveVector == Vector2.zero)
        {
            hiota.TransitionToState(hiota.IdleState);
        }

        if (!hiota.IsGrounded())
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

    

}
