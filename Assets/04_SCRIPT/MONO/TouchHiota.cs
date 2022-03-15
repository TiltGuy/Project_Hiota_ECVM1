using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHiota : MonoBehaviour
{
    public AttackStats_SO AttackStats;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable damageable = other.GetComponent(typeof(IDamageable)) as IDamageable;
            if (damageable != null)
            {
                damageable.TakeDamages(AttackStats.damages, transform);
                //Debug.Log("Player prend " + AttackStats.damages + " points de dégâts brut sans calculer son armure",this);
            }

            //Debug.Log("Je TOUCHE!!!", this);
        }
    }

    public void DestroyItSelfAfterUsed()
    {
        Destroy(this.gameObject);
    }
}
