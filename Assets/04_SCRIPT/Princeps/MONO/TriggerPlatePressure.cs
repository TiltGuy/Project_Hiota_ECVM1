using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPlatePressure : MonoBehaviour
{
    private float cooldown = 3f;
    public bool b_IsTriggered = false;
    private bool b_IsTimed = true;
    [SerializeField]
    private Animator animatorTrap;
    private bool b_PlayerIsInTrigger;

    public UnityEvent LaunchTrapActions;

    private void OnTriggerEnter( Collider other )
    {
        if(other.CompareTag("Player"))
        {
            TriggerTrap();
            b_PlayerIsInTrigger=true;
            animatorTrap.SetBool("b_PlayerInTrigger", b_PlayerIsInTrigger);
        }
    }

    private void OnTriggerExit( Collider other )
    {
        if ( other.CompareTag("Player") )
        {
            b_PlayerIsInTrigger = false;
            animatorTrap.SetBool("b_PlayerInTrigger", b_PlayerIsInTrigger);
        }
    }

    private void TriggerTrap()
    {
        if ( !b_IsTriggered )
        {

            LaunchTrapActions?.Invoke();
            if(b_IsTimed)
            {
                StartCoroutine(LaunchCooldown_Coroutine());
            }
            //LaunchCooldown
        }
    }

    public void ResetState()
    {
        Debug.Log("BAM!!");
    }

    private IEnumerator LaunchCooldown_Coroutine()
    {
        StopCoroutine(LaunchCooldown_Coroutine());
        Debug.Log("launch Cooldown");
        b_IsTriggered = true;
        yield return new WaitForSeconds(cooldown);
        b_IsTriggered = false;
    }
}
