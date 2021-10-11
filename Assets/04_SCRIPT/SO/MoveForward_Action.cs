using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/MoveForward_Action")]
public class MoveForward_Action : Action_SO
{
    [SerializeField]
    private bool b_ConstantMove;
    [SerializeField]
    private bool b_UseActionSpeed;

    [SerializeField]
    private float speedMovementAction;

    public override void Act(PlayerController_FSM controller)
    {
        if(!b_ConstantMove)
        {
            ControlledMove(controller);
        }
    }

    private void ControlledMove(PlayerController_FSM controller)
    {
        float currentSpeed;
        float maxSpeed;
        if(b_UseActionSpeed)
        {
            maxSpeed = speedMovementAction;
        }
        else
        {
            maxSpeed = controller.m_speed;
        }

        if(controller.m_InputMoveVector != Vector2.zero)
        {
            controller.m_camF = controller.m_cameraBaseDirection.forward;
            controller.m_camR = controller.m_cameraBaseDirection.right;

            controller.m_camF.y = 0;
            controller.m_camR.y = 0;
            controller.m_camF = controller.m_camF.normalized;
            controller.m_camR = controller.m_camR.normalized;

            controller.directionToGo = controller.m_camF * controller.m_InputMoveVector.y + controller.m_camR * controller.m_InputMoveVector.x;
            controller.dashDirection = controller.directionToGo.normalized;

            currentSpeed = maxSpeed;

            controller.characontroller.Move(controller.directionToGo * Time.deltaTime * currentSpeed);
            //controller.Hiota_Anim.SetFloat("DirectX_FocusMode", controller.m_InputMoveVector.x);
            //controller.Hiota_Anim.SetFloat("DirectZ_FocusMode", controller.m_InputMoveVector.y);
            Debug.Log(currentSpeed);
            

            Quaternion finalrot = Quaternion.LookRotation(controller.directionToGo, Vector3.up);
            controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, finalrot, controller.m_turnSpeed * Time.deltaTime);

        }

        controller.Hiota_Anim.SetFloat("Input_Move_Vector", controller.m_InputMoveVector.magnitude);

    }

}
