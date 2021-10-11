using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class HiotaBaseState
{

    public List<AbilityData_SO> ListAbilityData;

    public void UpdateAll()
    {
        foreach (AbilityData_SO d in ListAbilityData)
        {
            d.UpdateAbility();
        }
    }

    public abstract void EnterState(HiotaController_FSM hiota);
    public abstract void LogicUpdate(HiotaController_FSM hiota);
    public abstract void HandleInput(HiotaController_FSM hiota);
    public abstract void PhysicsUpdate(HiotaController_FSM hiota);
    public abstract void OnCollisionEnter(HiotaController_FSM hiota);
    public abstract void Exit(HiotaController_FSM hiota);
}
