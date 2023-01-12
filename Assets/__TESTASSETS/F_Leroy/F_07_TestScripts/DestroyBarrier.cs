using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyBarrier : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    [ContextMenu("DoSomething")]

    public void DoSomething()
    {
        animator.SetBool("Destroy", true);
    }
}
