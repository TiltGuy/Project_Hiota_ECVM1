using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/State")]
public class State_SO : ScriptableObject
{
    #region --- ACTION SETTINGS ---
    [Header(" --- ACTION SETTINGS ---")]

    public Action_SO[] actionsEnter;
    public Action_SO[] actionsUpdate;
    public Action_SO[] actionsExit;
    public Transition[] transitions;

    [Tooltip("At least One Transitions is necessary if State's Timed")]
    public Transition[] transitionsAfterCountdown;
    #endregion

    #region --- TRANSITIONS SETTINGS ---

    [Header("--- TRANSITIONS SETTINGS ---")]

    [Tooltip("If ticked the state will bypass all of his transitions in the inspector")]
    [SerializeField] private bool b_NormalTransitions = true;

    public bool b_TransitionsAfterCountdown;

    [Tooltip("The number of seconds the timed State lasts")]
    public float stateDuration;
    public float stateTimer = 0f;
    #endregion

    #region --- DEBUG ---
    [Header("--- DEBUG ---")]

    public Color sceneGizmosColor = Color.grey;
    #endregion

    public void UpdtateState(Controller_FSM controller)
    {
        if(b_TransitionsAfterCountdown)
        {
            if(stateTimer <= stateDuration)
            {
                stateTimer += Time.deltaTime;
            }
            else
            {
                //CheckTimedStateTransitions
                CheckTimedStateTransitions(controller);
            }
        }

        DoActions(controller, actionsUpdate);
        if(b_NormalTransitions)
        {
            CheckTransitions(controller);
        }

        //Debug.Log(stateTimer);
    }

    private void DoActions(Controller_FSM controller, Action_SO[] actions)
    {
        foreach(Action_SO a in actions)
        {
            a.Act(controller);
        }
    }

    private void CheckTransitions(Controller_FSM controller)
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

    private void CheckTimedStateTransitions(Controller_FSM controller)
    {
        if (transitionsAfterCountdown == null)
        {
            //Debug.LogError("There isn't State Timed Transitions in this " + Name + " State");
            return;
        }

        foreach (Transition transition in transitionsAfterCountdown)
        {
            bool b_DecisionSucceded = transition.decision.Decide(controller);

            if (b_DecisionSucceded)
            {
                controller.TransitionToState(transition.true_State);
            }
            else
            {
                controller.TransitionToState(transition.False_State);
            }
        }
    }

    public void EnterState(Controller_FSM controller)
    {
        if(b_TransitionsAfterCountdown)
        {
            stateTimer = 0;
        }
        //Debug.Log(controller.currentState + "ENTER");
        DoActions(controller, actionsEnter);
    }

    public void ExitState (Controller_FSM controller)
    {
        //Debug.Log(controller.currentState + "EXIT");
        DoActions(controller, actionsExit);
    }
}
