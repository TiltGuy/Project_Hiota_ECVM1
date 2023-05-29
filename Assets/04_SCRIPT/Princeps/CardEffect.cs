using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardEffect
{

    public CardEffect( object effect_SO)
    {
        object machin = effect_SO.GetType();

    }

    public string cardMessage;
    public virtual void AddEffect( Controller_FSM controller, CharacterSpecs characterSpecs )
    {
        Debug.Log("Do Something My Card Effect", controller);
    }
}
