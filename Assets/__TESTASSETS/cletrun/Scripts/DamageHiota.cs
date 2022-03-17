using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private NavMeshAgent enemyAgent;

    [SerializeField]
    private Transform RoundHitBoxPrefab;
    [SerializeField]
    private Transform RoundPreviewBoxPrefab;


    [SerializeField]
    private List<Transform> HitBoxesPresets;
    [SerializeField]
    private List<Transform> PreviewHitBoxesPresets;

    [SerializeField]
    private Transform currentAttackHitbox;
    [SerializeField]
    private Transform previewCurrentAttackHitbox;

    [SerializeField]
    private bool b_HaveLaunchedAnAttack = false;

    public Transform player;

    
    private bool isPreparingAttack = false;

    public delegate void MultiCastDelegate();
    public MultiCastDelegate OnBeginAttack;
    public MultiCastDelegate OnFinishAttack;

    private void Awake()
    {
        hiotaHealth = player.GetComponent<HiotaHealth>();
        enemyAgent = enemyScript.GetComponent<NavMeshAgent>();
    }

    void Start()
	{
        
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
            
        }
    }
    

    //mettre sur un script au même niveau du mesh + animator (animator sur le mesh)
    private void BeginAttack()
    {
        

        if(enemyScript.inRangeOfAttack == true )
        {
            if(!b_HaveLaunchedAnAttack)
            {
                LaunchAttackAnticipation();
            }
        }
        else
            enemyAnimator.SetBool("canAttack", false);



    }

    public void LaunchAttackAnticipation()
    {
        
        enemyAnimator.SetBool("canAttack", true);
        isPreparingAttack = true;
        PickAnAttack();

    }

    private void PickAnAttack()
    {
        int RandomValue = Random.Range(0, HitBoxesPresets.Count);
        enemyAnimator.SetFloat("IDAttack", RandomValue);
        currentAttackHitbox = HitBoxesPresets[RandomValue];
        previewCurrentAttackHitbox = PreviewHitBoxesPresets[RandomValue];
        print("Choose Attack");
    }

    public void AttackHiota()
    {
        OnBeginAttack();
    }

    public void SpawnFX(Transform targetFXPrefab)
    {
        hiotaHealth.SpawnHitReactionFX(targetFXPrefab);
    }

    public void UpdateBasicAttackStatutTrue()
    {
        currentAttackHitbox = Instantiate(currentAttackHitbox, enemyScript.transform.position, Quaternion.identity);
        // Set the parent
        currentAttackHitbox.SetParent(enemyScript.transform);
        // be sure to reset TRANSFORM and ROTATION
        currentAttackHitbox.transform.localPosition = Vector3.zero;
        currentAttackHitbox.transform.localRotation = Quaternion.identity;

    }

    public void UpdatePreviewAttackTrue()
    {
        previewCurrentAttackHitbox = Instantiate(previewCurrentAttackHitbox, enemyScript.transform.position, Quaternion.identity);
        // Set the parent
        previewCurrentAttackHitbox.SetParent(enemyScript.transform);
        // be sure to reset TRANSFORM and ROTATION
        previewCurrentAttackHitbox.transform.localPosition = Vector3.zero;
        previewCurrentAttackHitbox.transform.localRotation = Quaternion.identity;

        b_HaveLaunchedAnAttack = true;

    }

    public void UpdateBasicAttackStatutFalse()
    {
        if (currentAttackHitbox)
        {
            TouchHiota currentInstance = currentAttackHitbox.GetComponent<TouchHiota>();
            if(!currentInstance)
            {
                currentInstance = currentAttackHitbox.GetComponentInChildren<TouchHiota>();
            }
            currentInstance.DestroyItSelfAfterUsed();
        }
    }

    public void UpdatePreviewAttackFalse()
    {
        if(previewCurrentAttackHitbox)
        {
            previewCurrentAttackHitbox.gameObject.SetActive(false);
        }

    }

    public void HaveFinishedPreviousAttack()
    {
        TimerForNextAttack = attackCooldown;
        OnFinishAttack();
        b_HaveLaunchedAnAttack = false;
    }

    public void SetSpeedValue(float speed)
    {
        enemyAgent.speed = speed;
    }

    public void ResetSpeedValue()
    {
        enemyAgent.speed = enemyScript.baseSpeed;
    }

    public void ResetIsPreparingAttack()
    {
        isPreparingAttack = false;
    }

    public void SetIsAttacking()
    {

    }

}
