using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion: MonoBehaviour
{
    [SerializeField]
    private SphereCollider TriggerOfExplosion;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private float timerBeforeExplosion;
    public float damages;
    private float currentTimer = 0f;

    private void Start()
    {
        TriggerOfExplosion.enabled = false;
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
        if(currentTimer >= timerBeforeExplosion)
        {
            TriggerOfExplosion.enabled = true;
        }
    }

    private void OnTriggerEnter( Collider other )
    {
        if( ((1 << other.gameObject.layer) & mask) != 0 )
        {
            IDamageable damageable = other.GetComponent(typeof(IDamageable)) as IDamageable;
            if ( damageable != null )
            {
                damageable.TakeDamagesNonParriable(damages, transform, 1f);
            }
        }
    }

}
