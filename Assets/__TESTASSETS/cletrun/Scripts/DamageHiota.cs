using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHiota : MonoBehaviour
{
    public float TimerForNextAttack, attackCooldown;

    HiotaHealth hiotaHealth;
    private bool canAttack;
    public float attackDamage;

    public Animator enemyAnimator;
    public Enemy enemyScript;
    //public StaticEnemyAI staticEnemyScript;

    public Transform player;
    
    

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
    

    //mettre sur un script au même niveau du mesh + animator (animator sur le mesh)
    public void AttackHiota()
    {
        hiotaHealth = player.GetComponent<HiotaHealth>();

        if(enemyScript.canAttack == true /*|| staticEnemyScript.canAttack == true*/)
		{
            hiotaHealth.Hurt(attackDamage);
            //Attack
            enemyAnimator.SetBool("canAttack", true);
            
        }
		
        
        
    }

}
