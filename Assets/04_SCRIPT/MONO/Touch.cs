using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    public AttackStats_SO AttackStats;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent(typeof(IDamageable)) as IDamageable;
            //Debug.Log(other.gameObject.name, this);
            if (damageable != null)
            {
                damageable.TakeDamages(AttackStats.damages, transform);
                //Debug.Log("Moi : " + gameObject.name + "Je TOUCHE!!! " + other.gameObject.name, this);
            }
            
            
        }
    }

    public void DestroyItSelfAfterUsed()
    {
        Destroy(this.gameObject);
    }
}
