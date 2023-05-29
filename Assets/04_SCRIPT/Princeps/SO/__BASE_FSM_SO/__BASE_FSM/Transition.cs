using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition
{
    public Decision_SO decision;
    public Utilitary utilitary;
    public State_SO true_State;
    public State_SO False_State;
}
