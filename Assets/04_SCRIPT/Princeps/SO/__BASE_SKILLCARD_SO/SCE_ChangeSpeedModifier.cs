using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/04_SCRIPT/SO/SKILLCARD_SO/_EFFECTS/EFCT_SO", menuName = "SkillCard/SCE_ChangeSpeedModifier")]
public class SCE_ChangeSpeedModifier : Effect_SO
{
    public float SpeedModiferValueInPercent;

    public override void AddEffect( Controller_FSM controller, CharacterSpecs characterSpecs )
    {
        float tempValue = SpeedModiferValueInPercent / 100;
        
        controller.currentSpeedModifier += (controller.currentSpeedModifier * tempValue);
        //cardMessage = cardMessage + SpeedModiferValue;
        //Debug.Log("Replenish All HP", this);
    }
}
