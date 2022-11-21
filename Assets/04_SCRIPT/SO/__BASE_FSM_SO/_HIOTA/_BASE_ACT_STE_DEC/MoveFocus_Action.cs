using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/MoveFocus_Action")]
public class MoveFocus_Action : Action_SO
{
    public bool b_ConstantMove;
    public bool b_UseActionSpeed;
    public float speedMovement;
    public override void Act(Controller_FSM controller)
    {
        if(!b_ConstantMove)
        {
            ControlledMoveFocusMode(controller);
        }
    }

    private void ControlledMoveFocusMode(Controller_FSM controller)
    {
        float speed;
        if(b_UseActionSpeed)
        {
            speed = speedMovement;
        }
        else
        {
            speed = controller.CurrentSpeed;
        }
        controller.m_camF = controller.m_cameraBaseDirection.forward;
        controller.m_camR = controller.m_cameraBaseDirection.right;

        controller.m_camF.y = 0;
        controller.m_camR.y = 0;
        controller.m_camF = controller.m_camF.normalized;
        controller.m_camR = controller.m_camR.normalized;

        controller.directionToGo = controller.m_camF * controller.m_InputMoveVector.y + controller.m_camR * controller.m_InputMoveVector.x;
        controller.dashDirection = controller.directionToGo.normalized;

        controller.characontroller.Move(controller.directionToGo * Time.deltaTime * speed);
        controller.characterAnimator.SetFloat("DirectX_FocusMode", controller.m_InputMoveVector.x);
        controller.characterAnimator.SetFloat("DirectZ_FocusMode", controller.m_InputMoveVector.y);
    }
}
