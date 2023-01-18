using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Effect_SO: ScriptableObject
{
    public string cardMessage;
    public virtual void AddEffect(Controller_FSM controller, CharacterSpecs characterSpecs)
    {
        Debug.Log("Do Something My Card Effect", controller);
    }
}
