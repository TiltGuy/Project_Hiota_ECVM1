using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/04_SCRIPT/SO/SKILLCARD_SO/_EFFECTS/EFCT_SO", menuName = "SkillCard/SCE_ChangeMaxHealth")]
public class SCE_ChangeMaxHealth : Effect_SO
{
    public float targetValueInPercent;

    public override void AddEffect( Controller_FSM controller, CharacterSpecs characterSpecs )
    {
        float tempValue = targetValueInPercent / 100;
        characterSpecs.MaxHealth += (characterSpecs.MaxHealth * tempValue);
        Debug.Log("Max Health = " + characterSpecs.MaxHealth, characterSpecs);
    }
}
