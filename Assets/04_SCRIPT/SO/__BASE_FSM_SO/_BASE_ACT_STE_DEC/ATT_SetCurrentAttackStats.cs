using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ATT_SetCurrentAttack_Action")]
public class ATT_SetCurrentAttackStats : Action_SO
{
    private AttackStats_SO currentAttackStats;
    public enum AttackChosen {AT_Basic, CA_Front, CA_Side, CA_Back, CA_Parry }
    public AttackChosen attackChosen;
    public override void Act(Controller_FSM controller)
    {
        switch(attackChosen)
        {
            case AttackChosen.AT_Basic:
                currentAttackStats = controller.charSpecs.BasicAttackStats;
                break;
            case AttackChosen.CA_Front:
                currentAttackStats = controller.charSpecs.FrontAttackStats;
                break;
            case AttackChosen.CA_Side:
                currentAttackStats = controller.charSpecs.SideAttackStats;
                break;
            case AttackChosen.CA_Back:
                currentAttackStats = controller.charSpecs.BackAttackStats;
                break;
            case AttackChosen.CA_Parry:
                currentAttackStats = controller.charSpecs.ParryAttackStats;
                break;
        }
        base.SetCurrentAttackStats(controller, currentAttackStats);
    }
}
