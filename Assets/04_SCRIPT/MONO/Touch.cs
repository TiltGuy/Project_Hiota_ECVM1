using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    [SerializeField]
    private PlayerController_FSM controller_FSM;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent(typeof(IDamageable)) as IDamageable;
            if (damageable != null)
            {
                damageable.TakeDamages(controller_FSM.currentAttackStats.damages);
            }
            
            Debug.Log("Je TOUCHE!!!",this);
        }
    }
}
