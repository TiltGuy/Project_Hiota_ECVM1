using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ACT_VoidCoroutine")]
public class ACT_VoidCoroutine : Action_SO
{
    public string coroutineName;

    public override void Act(Controller_FSM controller)
    {
        PlayCoroutine(controller);
    }

    public void PlayCoroutine(Controller_FSM controller)
    {
        controller.StartCoroutine(coroutineName);
    }
}
