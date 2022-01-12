using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHiota : MonoBehaviour
{
    private Animator enemyAnimator;
    public Transform player;
    private bool canAttack;
    public int attackDamage = 1;

    

    /*public void HurtHiota(float damages)
    {
        if (player != null && canAttack == true)
        {
            
            player.Hurt(attackDamage);
            //Attack
            enemyAnimator.SetBool("canAttack", true);
        }
    }*/
}
