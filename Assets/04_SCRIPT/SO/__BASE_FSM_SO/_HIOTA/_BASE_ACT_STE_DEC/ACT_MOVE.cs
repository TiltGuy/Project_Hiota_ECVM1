using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_MOVE")]
public class ACT_MOVE : Action_SO
{
    [SerializeField]
    private bool b_ConstantMove;
    [SerializeField]
    private bool b_UseActionSpeed;

    [SerializeField]
    private float speedMovementAction;

    public override void Act(Controller_FSM controller)
    {
        if(!b_ConstantMove)
        {
            ControlledMove(controller);
        }
        else
        {
            ConstraintMove(controller);
        }
    }

    private void ControlledMove(Controller_FSM controller)
    {
        float currentSpeed;
        float maxSpeed;
        if(b_UseActionSpeed)
        {
            maxSpeed = speedMovementAction;
        }
        else
        {
            maxSpeed = controller.currentSpeed;
        }

        if(controller.m_InputMoveVector != Vector2.zero)
        {
            controller.m_camF = controller.m_cameraBaseDirection.forward;
            controller.m_camR = controller.m_cameraBaseDirection.right;

            controller.m_camF.y = 0;
            controller.m_camR.y = 0;
            controller.m_camF = controller.m_camF.normalized;
            controller.m_camR = controller.m_camR.normalized;

            currentSpeed = maxSpeed;

            controller.directionToGo = controller.m_camF * controller.m_InputMoveVector.y + controller.m_camR * controller.m_InputMoveVector.x;
            controller.currentDirection = new Vector3(controller.directionToGo.x * currentSpeed, controller.gravity, controller.directionToGo.z * currentSpeed);
            controller.dashDirection = controller.directionToGo.normalized;

            

            controller.characontroller.Move(controller.currentDirection * Time.deltaTime );
            //controller.Hiota_Anim.SetFloat("DirectX_FocusMode", controller.m_InputMoveVector.x);
            //controller.Hiota_Anim.SetFloat("DirectZ_FocusMode", controller.m_InputMoveVector.y);
            //Debug.Log(currentSpeed);
            
            //Update of the Orientation of Hiota
            if (controller.b_IsFocusing)
            {
                Vector3 hiotaPos = controller.transform.position;
                Vector3 dir = (controller.currentCharacterTarget.position - hiotaPos).normalized;
                dir.y = 0;
                controller.directionToFocus = dir;
                //Debug.DrawLine(hiotaPos, hiotaPos + dir * 10, Color.red, Mathf.Infinity);
                Quaternion finalrot = Quaternion.LookRotation(controller.directionToFocus, Vector3.up);
                controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, finalrot, controller.m_turnSpeed * Time.deltaTime);
                controller.characterAnimator.SetFloat("Input_Move_VectorX", controller.m_InputMoveVector.x);
                controller.characterAnimator.SetFloat("Input_Move_VectorZ", controller.m_InputMoveVector.y);
            }
            else
            {
                Quaternion finalrot = Quaternion.LookRotation(controller.directionToGo, Vector3.up);
                controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, finalrot, controller.m_turnSpeed * Time.deltaTime);
            }

        }
        else
        {
            if (controller.b_IsFocusing)
            {
                Vector3 hiotaPos = controller.transform.position;
                Vector3 dir = (controller.currentCharacterTarget.position - hiotaPos).normalized;
                dir.y = 0;
                controller.directionToFocus = dir;
                //Debug.DrawLine(hiotaPos, hiotaPos + dir * 10, Color.red, Mathf.Infinity);
                Quaternion finalrot = Quaternion.LookRotation(controller.directionToFocus, Vector3.up);
                controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, finalrot, controller.m_turnSpeed * Time.deltaTime);
                controller.characterAnimator.SetFloat("Input_Move_VectorX", controller.m_InputMoveVector.x);
                controller.characterAnimator.SetFloat("Input_Move_VectorZ", controller.m_InputMoveVector.y);
            }

            Vector3 fallGravity = new Vector3(0, controller.gravity, 0);
            controller.characontroller.Move(fallGravity * Time.deltaTime);
        }

        controller.characterAnimator.SetFloat("Input_Move_Vector", controller.m_InputMoveVector.magnitude);

    }

    private void ConstraintMove(Controller_FSM controller)
    {
        float currentSpeed;
        float maxSpeed;
        if (b_UseActionSpeed)
        {
            maxSpeed = speedMovementAction;
        }
        else
        {
            maxSpeed = controller.currentSpeed;
        }

        if (controller.m_InputMoveVector != Vector2.zero)
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

            //controller.Hiota_Anim.SetFloat("DirectX_FocusMode", controller.m_InputMoveVector.x);
            //controller.Hiota_Anim.SetFloat("DirectZ_FocusMode", controller.m_InputMoveVector.y);
            Debug.Log(currentSpeed);

            //Update of the Orientation of Hiota
            if (controller.b_IsFocusing)
            {
                Vector3 hiotaPos = controller.transform.position;
                Vector3 dir = (controller.currentCharacterTarget.position - hiotaPos).normalized;
                dir.y = 0;
                controller.directionToFocus = dir;
                //Debug.DrawLine(hiotaPos, hiotaPos + dir * 10, Color.red, Mathf.Infinity);
                Quaternion finalrot = Quaternion.LookRotation(controller.directionToFocus, Vector3.up);
                controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, finalrot, controller.m_speedTurnWhenAttack * Time.deltaTime);
                controller.characterAnimator.SetFloat("Input_Move_VectorX", controller.m_InputMoveVector.x);
                controller.characterAnimator.SetFloat("Input_Move_VectorZ", controller.m_InputMoveVector.y);
                controller.characontroller.Move(controller.transform.forward * Time.deltaTime * currentSpeed);
            }
            else
            {
                Quaternion finalrot = Quaternion.LookRotation(controller.directionToGo, Vector3.up);
                controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, finalrot, controller.m_speedTurnWhenAttack * Time.deltaTime);
                controller.characontroller.Move(controller.transform.forward * Time.deltaTime * currentSpeed);
            }

        }
        else
        {
            if (controller.b_IsFocusing)
            {
                Vector3 hiotaPos = controller.transform.position;
                Vector3 dir = (controller.currentCharacterTarget.position - hiotaPos).normalized;
                dir.y = 0;
                controller.directionToFocus = dir;
                //Debug.DrawLine(hiotaPos, hiotaPos + dir * 10, Color.red, Mathf.Infinity);
                Quaternion finalrot = Quaternion.LookRotation(controller.directionToFocus, Vector3.up);
                controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, finalrot, controller.m_turnSpeedWhenFocused * Time.deltaTime);
                controller.characterAnimator.SetFloat("Input_Move_VectorX", controller.m_InputMoveVector.x);
                controller.characterAnimator.SetFloat("Input_Move_VectorZ", controller.m_InputMoveVector.y);
            }
        }

        controller.characterAnimator.SetFloat("Input_Move_Vector", controller.m_InputMoveVector.magnitude);

    }

}
