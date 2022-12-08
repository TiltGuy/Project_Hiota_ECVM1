using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/04_SCRIPT/SO/SKILLCARD_SO/_EFFECTS/EFCT_SO", menuName = "SkillCard/SCE_ChangeDamageModifier")]
public class SCE_ChangeDamageModifier : Effect_SO
{
    public float DamageModiferValue;
    public string cardMessage;

    public override void AddEffect( Controller_FSM controller, CharacterSpecs characterSpecs )
    {
        characterSpecs.currentDamagesModifier += DamageModiferValue;
        cardMessage = cardMessage + DamageModiferValue;
        //Debug.Log("Replenish All HP", this);
    }
}
