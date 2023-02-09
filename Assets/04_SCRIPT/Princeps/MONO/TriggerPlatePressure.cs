using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPlatePressure : MonoBehaviour
{
    private float cooldown = 3f;
    private bool b_IsTriggered = false;

    public UnityEvent LaunchTrapActions;

    private void OnTriggerEnter( Collider other )
    {
        if(other.CompareTag("Player"))
        {
            TriggerTrap();
        }
    }

    private void TriggerTrap()
    {
        if ( !b_IsTriggered )
        {

            LaunchTrapActions?.Invoke();
            StartCoroutine(LaunchCooldown_Coroutine());
            //LaunchCooldown
        }
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
