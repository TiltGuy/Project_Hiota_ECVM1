using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVisualEffects : MonoBehaviour
{

    public Transform ps;
    public Animator animator;

    public void WindUpGlow()
    {
        Instantiate(ps, transform.position, Quaternion.identity);
    }

}
