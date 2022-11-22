using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent(typeof(IDamageable)) as IDamageable;
            if (damageable != null)
            {
                damageable.TakeDamages(2, transform, false);
            }

            Debug.Log("Je TOUCHE!!!", this);
        }
    }
}
