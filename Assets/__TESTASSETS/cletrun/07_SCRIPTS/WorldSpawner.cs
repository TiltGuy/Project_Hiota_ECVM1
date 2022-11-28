using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("SpawnWorld", false);

    }   

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("SpawnWorld", true);
        }
        
    }
}
