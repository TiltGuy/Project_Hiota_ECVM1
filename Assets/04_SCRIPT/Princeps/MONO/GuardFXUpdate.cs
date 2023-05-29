﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFXUpdate : StateMachineBehaviour
{
    public GameObject ShieldFXToInstantiate;
    private CharacterSpecs specs;
    [SerializeField]
    private FMODUnity.EventReference GuardShield_ER;
    [SerializeField]
    private FMOD.Studio.EventInstance GuardShield_Instance;
    [SerializeField]
    GameObject currentFX;
    [SerializeField]
    private MeshRenderer renderer;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        if(!currentFX && stateInfo.IsName("Basic_GuardStance"))
        {
            GuardShield_Instance = FMODUnity.RuntimeManager.CreateInstance(GuardShield_ER);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(GuardShield_Instance, animator.transform);
            currentFX = Instantiate(ShieldFXToInstantiate,
               animator.transform.position,
               animator.transform.rotation);
            renderer = currentFX.GetComponentInChildren<MeshRenderer>();
            specs = animator.GetComponent<CharacterSpecs>();
            GuardShield_Instance.start();
            GuardShield_Instance.release();
            //Debug.Log("Je fais mon fx! " + specs.CurrentGuard / specs.MaxGuard, this);
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        if ( currentFX)
        {
            //renderer = currentFX.GetComponent<MeshRenderer>();
            renderer.sharedMaterial.SetFloat("_Shield_Slider", specs.CurrentGuard / specs.MaxGuard);

            //Debug.Log("Je fais mon fx! " + specs.CurrentGuard / specs.MaxGuard, this);
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        if(currentFX)
        {
            //currentFX.SetActive(false);
            Destroy(currentFX);

        }
        GuardShield_Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    //{
    //    if ( currentFX && renderer )
    //    {
    //        renderer.material.SetFloat("Shield_Slider", specs.CurrentGuard / specs.MaxGuard);

    //        //Debug.Log("Je fais mon fx! " + specs.CurrentGuard / specs.MaxGuard, this);
    //    }
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter( Animator animator, int stateMachinePathHash )
    //{
    //    currentFX = Instantiate(ShieldFXToInstantiate,
    //        animator.transform.position,
    //        animator.transform.rotation);
    //    renderer = currentFX.GetComponentInChildren<MeshRenderer>();
    //    specs = animator.GetComponent<CharacterSpecs>();
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit( Animator animator, int stateMachinePathHash )
    {
        if(currentFX)
        {
            currentFX.SetActive(false);
            Destroy(currentFX);
        }
        //GuardShield_Instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        //Debug.Log("Je détruis mon fx!", this);
    }
}
