using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SC_Template", menuName ="SkillCard/SkillCard_Template")]
public class SkillCard_SO : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite artwork;
    public int shopCost;
    public int moneyBet;
    public int powerLVL = 1;
    public List<Effect_SO> Bonus;
    public List<Effect_SO> Malus;


    public void ApplyEffects(Controller_FSM controller_FSM, CharacterSpecs characterSpecs)
    {
        foreach ( Effect_SO effect in Bonus )
        {
            effect.AddEffect(controller_FSM, characterSpecs);
        }

        foreach ( Effect_SO effect in Malus )
        {
            effect.AddEffect(controller_FSM, characterSpecs);
        }
    }

    
}
