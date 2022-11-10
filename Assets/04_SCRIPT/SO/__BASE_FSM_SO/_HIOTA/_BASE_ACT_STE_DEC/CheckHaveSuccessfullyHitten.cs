using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Player/CheckHaveSuccessfullyHitten_Decision")]
public class CheckHaveSuccessfullyHitten : Decision_SO
{
    public override bool Decide(Controller_FSM controller)
    {
        //Debug.Log(controller.B_HaveSuccessfullyHitten);
        return controller.B_HaveSuccessfullyHitten;
    }
}
