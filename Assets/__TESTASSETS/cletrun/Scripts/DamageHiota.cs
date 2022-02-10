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
    public Controller_FSM playerScript;

    [SerializeField]
    private Transform attackHitBoxPrefab;

    private Transform currentAttackHitbox;

    public Transform player;

    private void Awake()
    {
        hiotaHealth = player.GetComponent<HiotaHealth>();
    }

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
            BeginAttack();
            TimerForNextAttack = attackCooldown;
            
        }
    }
    

    //mettre sur un script au même niveau du mesh + animator (animator sur le mesh)
    private void BeginAttack()
    {
        

        if(enemyScript.canAttack == true && playerScript.b_Parry == false/*|| staticEnemyScript.canAttack == true*/)
		{
            enemyAnimator.SetBool("canAttack", true);
        }
        else
            enemyAnimator.SetBool("canAttack", false);



    }

    public void AttackHiota(float damages)
    {
        //hiotaHealth.Hurt(damages);
    }

    public void SpawnFX(Transform targetFXPrefab)
    {
        hiotaHealth.SpawnHitReactionFX(targetFXPrefab);
    }

    public void UpdateBasicAttackStatutTrue()
    {
        if (attackHitBoxPrefab)
        {
            currentAttackHitbox = Instantiate(attackHitBoxPrefab, enemyScript.transform.position, Quaternion.identity);
            // Set the parent
            currentAttackHitbox.SetParent(enemyScript.transform);
            // be sure to reset TRANSFORM and ROTATION
            currentAttackHitbox.transform.localPosition = Vector3.zero;
            currentAttackHitbox.transform.localRotation = Quaternion.identity;
        }
    }

    public void UpdateBasicAttackStatutFalse()
    {
        if (attackHitBoxPrefab)
        {
            currentAttackHitbox.GetComponent<TouchHiota>().DestroyItSelfAfterUsed();
        }
    }

}
