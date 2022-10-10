using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/ATT_SETAnimationIDsndSpeed")]

public class ATT_SETCurrAtkAnimationIDsndSpeed : Action_SO
{
    public override void Act(Controller_FSM controller)
    {
        SetAnimationIDs(controller);
        SetAnimationSpeeds(controller);
    }

    private void SetAnimationIDs(Controller_FSM controller)
    {
        Animator charAnimator = controller.Animator;

        charAnimator.SetFloat("ID_Anticipation", controller.CurrentAttackStats.AnimID_Anticipation);
        charAnimator.SetFloat("ID_Hitframe", controller.CurrentAttackStats.AnimID_Hitframe);
        charAnimator.SetFloat("ID_Recovery", controller.CurrentAttackStats.AnimID_Recovery);
    }

    private void SetAnimationSpeeds(Controller_FSM controller)
    {
        Animator charAnimator = controller.Animator;

        charAnimator.SetFloat("AntAnimMultiplier", controller.CurrentAttackStats.AnticipationAnimSpeed);
        charAnimator.SetFloat("HFAnimMultiplier", controller.CurrentAttackStats.HitframeAnimSpeed);
        charAnimator.SetFloat("RecovAnimMultiplier", controller.CurrentAttackStats.RecoveryAnimSpeed);

        Debug.Log("Animations speeds Changed");
    }
}
