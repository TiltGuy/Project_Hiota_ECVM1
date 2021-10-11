using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiotaLAHoldState : HiotaBaseState
{
    public override void EnterState(HiotaController_FSM hiota)
    {
        hiota.Hiota_Anim.SetBool("LAHoldStance", true);
    }

    public override void Exit(HiotaController_FSM hiota)
    {
        hiota.Hiota_Anim.SetBool("LAHoldStance", false);
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

        //INITIALISATION AND UPDATE OF THE INPUTS ON THE KEYBOARD
        hiota.m_inputsKeyBoard = Vector3.zero;
        hiota.m_inputsKeyBoard.x = Input.GetAxis("Horizontal");

        hiota.m_inputsKeyBoard.z = Input.GetAxis("Vertical");
        hiota.m_inputsKeyBoard = Vector3.ClampMagnitude(hiota.m_inputsKeyBoard, 1);

        hiota.directionToGo = hiota.m_camF * hiota.m_inputsKeyBoard.z + hiota.m_camR * hiota.m_inputsKeyBoard.x;
        hiota.dashDirection = hiota.directionToGo.normalized;

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("GO ATTACK");
            hiota.TransitionToState(hiota.LightAttack1State);
        }

        if (Input.GetMouseButtonDown(1))
        {
            hiota.TransitionToState(hiota.MovingState);
        }
    }

    public override void LogicUpdate(HiotaController_FSM hiota)
    {
        hiota.characontroller.Move(hiota.directionToGo * Time.deltaTime * hiota.m_HoldAttackSpeed);
        hiota.Hiota_Anim.SetFloat("DirectX_FocusMode", hiota.m_inputsKeyBoard.x);
        hiota.Hiota_Anim.SetFloat("DirectZ_FocusMode", hiota.m_inputsKeyBoard.z);


        if (hiota.b_IsFocusing)
        {
            hiota.RotatePlayerNorY(hiota.currentHiotaTarget);
            hiota.Hiota_Anim.SetBool("Is_Focusing", true);
        }
        else if(hiota.directionToGo != Vector3.zero)
        {
            Quaternion finalrot = Quaternion.LookRotation(hiota.directionToGo, Vector3.up);
            hiota.transform.rotation = Quaternion.Lerp(hiota.transform.rotation, finalrot, hiota.m_turnSpeed * Time.deltaTime);
            hiota.Hiota_Anim.SetBool("Is_Focusing", false);
        }
    }

    public override void OnCollisionEnter(HiotaController_FSM hiota)
    {
        
    }

    public override void PhysicsUpdate(HiotaController_FSM hiota)
    {
        
    }
}
