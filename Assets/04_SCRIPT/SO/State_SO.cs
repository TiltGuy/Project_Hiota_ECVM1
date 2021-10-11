using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/State")]
public class State_SO : ScriptableObject
{
    public Action_SO[] actionsEnter;
    public Action_SO[] actionsUpdate;
    public Action_SO[] actionsExit;
    public Transition[] transitions;
    public bool b_TimedState;
    public float stateDuration;
    public Color sceneGizmosColor = Color.grey;

    public void UpdtateState(PlayerController_FSM controller)
    {
        DoActions(controller, actionsUpdate);
        CheckTransitions(controller);
    }

    private void DoActions(PlayerController_FSM controller, Action_SO[] actions)
    {
        foreach(Action_SO a in actions)
        {
            a.Act(controller);
        }
    }

    private void CheckTransitions(PlayerController_FSM controller)
    {
        foreach(Transition transition in transitions)
        {
            bool b_DecisionSucceded = transition.decision.Decide(controller);

            if(b_DecisionSucceded)
            {
                controller.TransitionToState(transition.true_State);
            }
            else
            {
                controller.TransitionToState(transition.False_State);
            }
        }
    }

    public void EnterState(PlayerController_FSM controller)
    {
        //Debug.Log(controller.currentState + "ENTER");
        DoActions(controller, actionsEnter);
    }

    public void ExitState (PlayerController_FSM controller)
    {
        //Debug.Log(controller.currentState + "EXIT");
        DoActions(controller, actionsExit);
    }
}
