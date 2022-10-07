using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/Player/DelegateEvent/ATT_OnAttackBegin")]
public class ATT_OnAttackBegin : Action_SO
{

    private Transform _currentTarget;
    public enum AttachTypeCall 
    { 
        AllAttackType, 
        AT_Basic, 
        CA_Front, 
        CA_Side, 
        CA_Back, 
        CA_Parry 
    }
    public AttachTypeCall attackType;

    public override void Act(Controller_FSM controller)
    {
        UpdateCurrentTarget(controller);
        CallOnAttackBegin(controller);
        Debug.Log("BTM");
    }

    void UpdateCurrentTarget(Controller_FSM controller)
    {
        if (controller.currentCharacterTarget)
        {
            _currentTarget = controller.currentCharacterTarget;
        }
        else
        {
            Debug.LogWarning("Try to acces to current Hiota Target but Nobody's here", this);
            _currentTarget = null;
        }
    }

    void CallOnAttackBegin(Controller_FSM controller)
    {
        switch (attackType)
        {
            case AttachTypeCall.AT_Basic:
                controller.OnBasicABegin();
                break;
            case AttachTypeCall.CA_Front:
                controller.OnFrontCABegin();
                break;
            case AttachTypeCall.CA_Side:
                controller.OnSideCABegin();
                break;
            case AttachTypeCall.CA_Back:
                controller.OnBackCABegin();
                break;
            case AttachTypeCall.CA_Parry:
                controller.OnParryCABegin();
                break;
            case AttachTypeCall.AllAttackType:
                Debug.Log("Calling On Attack Begin");
                controller.OnAttackBegin();
                break;
        }
    }


}
