using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiota_LightAttack_02 : HiotaBaseState
{
    private float attacktimer = 0f;

    public override void EnterState(HiotaController_FSM hiota)
    {
        hiota.Hiota_Anim.SetBool("Attack_02", true);
    }

    public override void Exit(HiotaController_FSM hiota)
    {
        attacktimer = 0f;
        hiota.Hiota_Anim.SetBool("Attack_02", false);
    }

    public override void HandleInput(HiotaController_FSM hiota)
    {

    }

    public override void LogicUpdate(HiotaController_FSM hiota)
    {
        Debug.Log("Attack");
        attacktimer += Time.deltaTime;

        if (hiota.maxAttackTime < attacktimer)
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
