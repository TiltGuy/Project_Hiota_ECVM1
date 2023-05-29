using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
using UnityEngine.PlayerLoop;

public class Projectile : MonoBehaviour
{
    Rigidbody body;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float damages;
    [SerializeField]
    private LayerMask targetLayerMasktoDamages;
    [SerializeField]
    private LayerMask targetLayerMasktoDestroy;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter( Collider other )
    {
        if( targetLayerMasktoDamages == (targetLayerMasktoDamages | (1 << other.gameObject.layer)) )
        {
            DoDamages(other);
        }
        if ( targetLayerMasktoDestroy == (targetLayerMasktoDestroy | (1 << other.gameObject.layer)) )
        {
            print("Destroying Myself " + gameObject.name);
            Destroy(gameObject);
        }
    }

    private void DoDamages( Collider other )
    {
        IDamageable damageable = other.GetComponent(typeof(IDamageable)) as IDamageable;
        //Debug.Log(other.gameObject.name, this);
        if ( damageable != null )
        {
            damageable.TakeDamagesParriable(damages, transform, false);
        }
    }

    private void FixedUpdate()
    {
        body.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }
}
