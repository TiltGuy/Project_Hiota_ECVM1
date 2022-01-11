using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/ACT_FALL")]
public class ACT_FALL : Action_SO
{
    [SerializeField]
    private float gravityFactor = 1f;

    public override void Act(PlayerController_FSM controller)
    {
        Fall(controller);
    }

    private void Fall(PlayerController_FSM controller)
    {
        

        Vector3 fallGravity = new Vector3(0, controller.gravity, 0);
        controller.characontroller.Move(fallGravity * Time.deltaTime * gravityFactor);
        controller.Hiota_Anim.SetFloat("Input_Move_Vector", controller.m_InputMoveVector.magnitude);

    }


}
