using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeZone : MonoBehaviour
{
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("NarrativeZone", false);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("NarrativeZone", true);
        }

    }
}
