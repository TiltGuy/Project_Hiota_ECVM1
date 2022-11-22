using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Transform Prefab_DestroyedObject;

    public void TakeDamages( float damageTaken, Transform Striker, bool isAHook )
    {
        Transform ObjectToDestroy = Instantiate(Prefab_DestroyedObject, transform.position, Quaternion.identity);
        Rigidbody[] Rbs = Prefab_DestroyedObject.GetComponentsInChildren<Rigidbody>();

        Debug.Log("je suis passée par là", this);
        foreach ( Rigidbody rb in Rbs )
        {
            rb.AddExplosionForce(500, Striker.position, 1);
        }
        Destroy(gameObject);
    }

    public void TakeDamages( float damageTaken, Transform striker )
    {
        throw new System.NotImplementedException();
    }
}
