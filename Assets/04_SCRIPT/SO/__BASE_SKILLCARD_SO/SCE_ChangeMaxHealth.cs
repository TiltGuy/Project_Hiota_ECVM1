using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/04_SCRIPT/SO/SKILLCARD_SO/_EFFECTS/EFCT_SO", menuName = "SkillCard/SCE_ChangeMaxHealth")]
public class SCE_ChangeMaxHealth : Effect_SO
{
    public float targetValueInPercent;

    public override void AddEffect( Controller_FSM controller, CharacterSpecs characterSpecs )
    {
        //base.AddEffect( controller, characterSpecs );
        //Debug.Log(characterSpecs.MaxHealth, controller);
        float tempValue = targetValueInPercent / 100;
        //tempValue = Mathf.Round(tempValue * 100f) / 100f;
        characterSpecs.MaxHealth += (characterSpecs.MaxHealth * tempValue);
        Debug.Log(characterSpecs.MaxHealth, characterSpecs);
        //cardMessage = cardMessage + targetValue;
        //Debug.Log(characterSpecs.MaxHealth, controller);
    }
}
