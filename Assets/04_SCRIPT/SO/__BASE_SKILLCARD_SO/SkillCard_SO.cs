using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/04_SCRIPT/SO/SKILLCARD_SO/", menuName ="SkillCard/SkillCard_Template")]
public class SkillCard_SO : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite artwork;
    public List<Effect_SO> effects;


    public void ApplyEffects(Controller_FSM controller_FSM, CharacterSpecs characterSpecs)
    {
        foreach ( Effect_SO effect in effects )
        {
            effect.AddEffect(controller_FSM, characterSpecs);
        }
    }

    
}
