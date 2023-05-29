using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( Controller_FSM ))]
[RequireComponent(typeof( CharacterSpecs ))]
public class SkillCardScript : MonoBehaviour
{
    private Controller_FSM controller;
    private CharacterSpecs characterSpecs;
    public SkillCard_SO CurrentSkillCard;

    public float ArmorImprove = 2f;

    private void Awake()
    {
        controller = GetComponent<Controller_FSM>();
        characterSpecs = GetComponent<CharacterSpecs>();
    }

    private void Start()
    {
        ApplyEffectsOfSkillCardSO(controller, characterSpecs);
    }

    private void ApplyEffectsOfSkillCardSO(Controller_FSM controller, CharacterSpecs characterSpecs)
    {
        CurrentSkillCard.ApplyEffects(controller, characterSpecs);
        //Debug.Log("Component of the Enemy try to apply effects");
    }
}
