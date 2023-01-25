using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillCard
{
    public string cardName;
    public string description;
    public Sprite artwork;
    public int powerLVL = 1;
    public List<Effect_SO> Bonus;
    public List<Effect_SO> Malus;

    public SkillCard(SkillCard_SO skillCard_SO)
    {
        cardName = skillCard_SO.cardName;
        description = skillCard_SO.description;
        artwork = skillCard_SO.artwork;
        powerLVL = skillCard_SO.powerLVL;
    }

    public void ApplyEffects( Controller_FSM controller_FSM, CharacterSpecs characterSpecs )
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
