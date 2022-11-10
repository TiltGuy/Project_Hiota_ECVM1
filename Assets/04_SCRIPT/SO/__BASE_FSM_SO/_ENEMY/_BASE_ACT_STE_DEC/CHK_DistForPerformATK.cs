using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/Enemy/CHK_DistForPerformATK")]
public class CHK_DistForPerformATK : Decision_SO
{
    private float distBetweenContndEnemy;

    public override bool Decide( Controller_FSM controller )
    {
        distBetweenContndEnemy = Vector3.Distance
            (controller.transform.position,controller.currentCharacterTarget.transform.position);
        Debug.Log(distBetweenContndEnemy < controller.BrainAI.minDistForPerformingAttack);
        if (distBetweenContndEnemy < controller.BrainAI.minDistForPerformingAttack)
            return true;
        else
            return false;
    }
}
