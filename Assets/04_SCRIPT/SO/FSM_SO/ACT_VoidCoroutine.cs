using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/ACT_VoidCoroutine")]
public class ACT_VoidCoroutine : Action_SO
{
    public string coroutineName;

    public override void Act(PlayerController_FSM controller)
    {
        PlayCoroutine(controller);
    }

    public void PlayCoroutine(PlayerController_FSM controller)
    {
        controller.StartCoroutine(coroutineName);
    }
}
