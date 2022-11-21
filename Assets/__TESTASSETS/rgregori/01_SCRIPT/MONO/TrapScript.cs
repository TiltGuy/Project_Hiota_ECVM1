using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    [SerializeField] private List<string> TargetTag = new List<string>();
    private Animator animator;
    private float damages;

    private void OnTriggerEnter( Collider other )
    {
        foreach( string tag in TargetTag )
        {
            if(other.CompareTag(tag))
            {
                IDamageable damageable = other.GetComponent(typeof(IDamageable)) as IDamageable;
                //Debug.Log(other.gameObject.name, this);
                if ( damageable != null )
                {
                    damageable.TakeDamages(damages, transform);
                }
            }
        }
    }

}
