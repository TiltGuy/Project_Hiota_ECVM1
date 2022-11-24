using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( Controller_FSM ))]
[RequireComponent(typeof( CharacterSpecs ))]
public class SkillCardScript : MonoBehaviour
{
    private Controller_FSM controller;
    private CharacterSpecs characterSpecs;

    public float ArmorImprove = 2f;

    private void Awake()
    {
        controller = GetComponent<Controller_FSM>();
        characterSpecs = GetComponent<CharacterSpecs>();
    }

    private void Start()
    {
        characterSpecs.CurrentArmor += ArmorImprove;
        Debug.Log(characterSpecs.CurrentArmor,this);
    }
}
