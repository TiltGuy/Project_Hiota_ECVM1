using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHiota : MonoBehaviour
{
    public float TimerForNextAttack, attackCooldown;
    HiotaHealth hiotaHealth;
    private bool canAttack;
    private Animator enemyAnimator;
    public Transform player;
    public int attackDamage = 1;
    public EnemyAI enemyScript;

    void Start()
	{
        attackCooldown = 3;
        TimerForNextAttack = attackCooldown;
    }

	void Update()
	{
        if (TimerForNextAttack > 0)
        {
            TimerForNextAttack -= Time.deltaTime;
        }
        else if (TimerForNextAttack <= 0)
        {
            AttackHiota();
            TimerForNextAttack = attackCooldown;
        }
    }
    void OnTriggerStay(Collider other)
    {
        //HiotaHealth player = other.GetComponent<HiotaHealth>();
        if(other.gameObject == player)
		{
            AttackHiota();
		}


    }

    //mettre sur un script au même niveau du mesh + animator (animator sur le mesh)
    public void AttackHiota()
    {
        hiotaHealth = player.GetComponent<HiotaHealth>();

        if(enemyScript.canAttack == true)
		{
            hiotaHealth.Hurt(attackDamage);
            //Attack
            enemyAnimator.SetBool("canAttack", true);
        }
        
        
    }
}
