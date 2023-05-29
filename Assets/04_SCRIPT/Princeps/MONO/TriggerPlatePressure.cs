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
    public List<string> tagFilter = new List<string>() { "Player" };
    public GameObject fxPrefab;

    public UnityEvent LaunchTrapActions;
    public List<Collider> others = new List<Collider>();

    public FMODUnity.EventReference Trap_Triggered_ER;
    public Transform pointToSound;

    private void OnTriggerEnter( Collider other )
    {
        if(tagFilter.Contains(other.gameObject.tag))
        {
            if ( !others.Contains(other) )
                others.Add(other);

            if(!b_PlayerIsInTrigger)
            {
                Debug.Log(this + " triggered by " + other);
                if ( fxPrefab != null )
                    Instantiate(fxPrefab, other.transform.position, Quaternion.identity);
                //TextFX.Create(other.transform.position, "TRAP!");
                TriggerTrap();
                b_PlayerIsInTrigger = true;
                animatorTrap?.SetBool("b_PlayerInTrigger", b_PlayerIsInTrigger);
            }
        }
    }

    private void OnTriggerExit( Collider other )
    {
        if ( others.Contains(other) )
        {
            others.Remove(other);

            if ( others.Count == 0 && b_PlayerIsInTrigger )
            {
                b_PlayerIsInTrigger = false;
                animatorTrap?.SetBool("b_PlayerInTrigger", b_PlayerIsInTrigger);
            }
        }

        
    }

    private void TriggerTrap()
    {
        if ( !b_IsTriggered )
        {

            LaunchTrapActions?.Invoke();
            FMODUnity.RuntimeManager.PlayOneShotAttached(Trap_Triggered_ER, pointToSound.gameObject);
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
