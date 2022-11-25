using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/04_SCRIPT/SO/SKILLCARD_SO", menuName = "SkillCard/SCE_ChangeMaxHealth")]
public class SCE_ChangeMaxHealth : Effect_SO
{
    public float targetValue;

    public override void AddEffect( Controller_FSM controller, CharacterSpecs characterSpecs )
    {
        base.AddEffect( controller, characterSpecs );
        Debug.Log(characterSpecs.MaxHealth, controller);
        characterSpecs.MaxHealth += targetValue;
        Debug.Log(characterSpecs.MaxHealth, controller);
    }
}
