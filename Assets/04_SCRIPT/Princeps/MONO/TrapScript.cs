using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    [SerializeField] private List<string> TargetTag = new List<string>();
    private Animator animator;
    public float damages;
    public bool b_IsRandomized;

    private void Awake()
    {
        if(b_IsRandomized)
        {
            if(LevelManager.instance != null)
            {
                LevelManager.instance.PropsList.Add(gameObject);
            }
            else
            {
                Debug.Log("TAMERRRRRRRRRRRRR");
            }
        }
    }

    private void OnTriggerEnter( Collider other )
    {
        foreach( string tag in TargetTag )
        {
            
            DoDamages(other, tag);
        }
    }

    private void DoDamages( Collider other, string tag )
    {
        if ( other.CompareTag(tag) )
        {
            IDamageable damageable = other.GetComponent(typeof(IDamageable)) as IDamageable;
            //Debug.Log(other.gameObject.name, this);
            if ( damageable != null )
            {
                damageable.TakeDamagesNonParriable(damages, transform, 0f);
            }
        }
    }
}
