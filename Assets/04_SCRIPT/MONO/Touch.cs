using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    [SerializeField] private string Tag = "Enemy";
    public AttackStats_SO AttackStats;

    [HideInInspector] PlayerController_Animator instigatorAnimator;
    Controller_FSM controllerFSM;

    public PlayerController_Animator InstigatorAnimator 
    { 
        get => instigatorAnimator; 
        set
        {
            instigatorAnimator = value;
        } 
    }

    public Controller_FSM ControllerFSM 
    { 
        get => controllerFSM; 
        set
        {
            controllerFSM = value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag) || other.CompareTag("Destructible"))
        {
            IDamageable damageable = other.GetComponent(typeof(IDamageable)) as IDamageable;
            //Debug.Log(other.gameObject.name, this);
            if (damageable != null)
            {
                damageable.TakeDamages(AttackStats.damages, transform);
                //Debug.Log("Moi : " + gameObject.name + "Je TOUCHE!!! " + other.gameObject.name, this);
                //Debug.Log("Dégats : " + AttackStats.damages, this);
                controllerFSM.characterAnimator.SetBool("b_Attack", false);
                controllerFSM.B_HaveSuccessfullyHitten = true;
                controllerFSM.OnTouchedEnemy?.Invoke();
                //InstigatorAnimator.animator.SetTrigger("t_SuccessfullyHitten");
            }
            
            
        }
    }

    public void DestroyItSelfAfterUsed()
    {
        Destroy(this.gameObject);
    }
}
