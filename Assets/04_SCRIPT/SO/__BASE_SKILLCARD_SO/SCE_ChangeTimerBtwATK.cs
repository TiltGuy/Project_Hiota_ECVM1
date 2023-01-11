using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/04_SCRIPT/SO/SKILLCARD_SO/_EFFECTS/EFCT_SO", 
    menuName = "SkillCard/SCE_ChangeTimerBtwATK")]
public class SCE_ChangeTimerBtwATK :Effect_SO
{
    public float valueInPercent;

    public override void AddEffect( Controller_FSM controller, CharacterSpecs characterSpecs )
    {
        float tempValue = valueInPercent / 100;
        controller.BrainAI.timerBetweenATK += (controller.BrainAI.timerBetweenATK * tempValue);
    }
}
