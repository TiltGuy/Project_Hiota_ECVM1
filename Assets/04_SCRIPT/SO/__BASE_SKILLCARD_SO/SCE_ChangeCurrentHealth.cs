using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/04_SCRIPT/SO/SKILLCARD_SO/_EFFECTS/EFCT_SO", menuName = "SkillCard/SCE_ChangeCurrentHealth")]
public class SCE_ChangeCurrentHealth : Effect_SO
{
    public float targetValue;
    public string cardMessage;

    public override void AddEffect( Controller_FSM controller, CharacterSpecs characterSpecs )
    {
        characterSpecs.Health = targetValue;
        cardMessage = cardMessage;
        //Debug.Log("Replenish All HP", this);
    }
}
